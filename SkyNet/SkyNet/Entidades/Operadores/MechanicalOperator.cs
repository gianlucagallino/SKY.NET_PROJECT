using SkyNet.Entidades.Mapa;
using System.Text.Json.Serialization;

/*
    La clase MechanicalOperator representa un operador mecánico en el sistema SkyNet.
    Sus funcionalidades clave incluyen el movimiento a ubicaciones específicas, la transferencia de carga y batería,
    la simulación de daños, y la ejecución de órdenes generales.
    Dentro de sus métodos principales se encuentran el MoveTo, TransferBattery, TransferLoad, GeneralOrder. Otros métodos
    como el calculateDistancie, CalculateBatteryConsumption, IsDamaged, sirven de apoyo para modularizar y refactorizar
    el código.
 */

namespace SkyNet.Entidades.Operadores
{
    public abstract class MechanicalOperator
    {

        private Dictionary<int, Action<MechanicalOperator>> terrainDamages;

        public string Id { get; set; }
        public bool BusyStatus { get; set; }
        public Battery Battery { get; set; }
        public string Status { get; set; }
        public double MaxLoad { get; set; }
        public double MaxLoadOriginal { get; set; }
        public double CurrentLoad { get; set; }
        public double OptimalSpeed { get; set; }
        public Location LocationP { get; set; }
        public DamageSimulator DamageSimulatorP { get; set; }
        //public Node[,] Grid { get; set; }
        public int TimeSpent { get; private set; }

        public Dictionary<int, Action<MechanicalOperator>> TerrainDamages;
        public float KilometersTraveled { get; private set; }
        public float EnergyConsumed { get; private set; }
        public float TotalCarriedLoad { get; private set; }
        public int ExecutedInstructions { get; private set; }
        public int DamagesReceived { get; private set; }
        public bool DistanceFlag { get; private set; }

        [JsonConstructor]
        public MechanicalOperator()
        {
            BusyStatus = false;
            Battery = new Battery();
            Status = "ACTIVE";
            MaxLoad = 1000;
            MaxLoadOriginal = 0;
            CurrentLoad = 0;
            OptimalSpeed = 100;
            DamageSimulatorP = new DamageSimulator();
            terrainDamages = new Dictionary<int, Action<MechanicalOperator>>()
            {
              { 1, (oper) => DamageSimulatorP.SimulateRandomDamage(oper) },
              { 2, (oper) => { if (oper is M8 || oper is K9)
                { Console.WriteLine("M8 and K9 cannot enter the lake."); } return; } },
              { 3, (oper) => DamageSimulatorP.ElectronicLandfillSimulate(oper) }
            };
            KilometersTraveled = 0;
            EnergyConsumed = 0;
            TotalCarriedLoad = 0;
            ExecutedInstructions = 0;
            DamagesReceived = 0;
            LocationP = new Location(0, 0);
        }

        protected MechanicalOperator(double maxLoad, double minLoad, Battery battery, Location location, string status, string id)
        {
            MaxLoad = maxLoad;
            MaxLoadOriginal = minLoad;
            Battery = new Battery();
            LocationP = location;
            Status = status;
            Id = id;
            KilometersTraveled = 0;
            EnergyConsumed = 0;
            TotalCarriedLoad = 0;
            ExecutedInstructions = 0;
            DamagesReceived = 0;
        }

        protected MechanicalOperator(int xposition, int yposition)
        {
            Id = this.Id;
            LocationP = new Location(xposition, yposition);
            Battery = new Battery();
            Status = StatusString(BusyStatus);
            DamageSimulatorP = new DamageSimulator();
            TerrainDamages = new Dictionary<int, Action<MechanicalOperator>>()
            {
              { 1, (oper) => DamageSimulatorP.SimulateRandomDamage(oper) },
              { 2, (oper) => { if (oper is M8 || oper is K9)
                { Console.WriteLine("M8 and K9 cannot enter the lake."); } return; } },
              { 3, (oper) => DamageSimulatorP.ElectronicLandfillSimulate(oper) }
            };
            KilometersTraveled = 0;
            EnergyConsumed = 0;
            TotalCarriedLoad = 0;
            ExecutedInstructions = 0;
            DamagesReceived = 0;
        }

        public string StatusString(bool busy)
        {
            string status = "";
            if (busy)
            {
                status = "Operator is not available";
            }
            else
            {
                status = "Operator is available";
            }
            return status;
        }


        public int SimulateTime(TimeSimulator taskType)
        {
            int time = (int)taskType;
            TimeSpent += time;
            return time;
        }
        public double CalculateMovementSpeed()
        {
            double batteryPercentageSpent = 100 - Battery.CurrentChargePercentage / Battery.MAHCapacity * 100;
            double slownessMultiplier = Math.Floor(batteryPercentageSpent / 10); ; //this line calculates how many times to apply the speed debuff
            double finalSpeed = OptimalSpeed - OptimalSpeed / 10 * slownessMultiplier;
            return finalSpeed;
        }

        public void MoveTo(Location loc, bool safety, int hqNumber, string opId)
        {
            //double finalSpeed = CalculateMovementSpeed();
            // OptimalSpeed = finalSpeed;
            DistanceFlag = true;
            int terrainType = Map.Grid[LocationP.LocationX, LocationP.LocationY].TerrainType;

            Node start = new Node(LocationP.LocationX, LocationP.LocationY);
            Node goal = new Node(loc.LocationX, loc.LocationY);

            AStarAlgorithm astar = new AStarAlgorithm();

            bool isWalkingUnit = true;
            if (Id.Contains("UAV"))
            {
                isWalkingUnit = false;
            }

            List<Node> path = astar.FindPath(start, goal, Map.Grid, safety, isWalkingUnit);


            if (path.Count == 0)
            {
                Console.WriteLine("No path found for this unit.");
                DistanceFlag = false;
            }

            if (path != null)
            {
                foreach (Node node in path)
                {
                    Location TempLocation = node.NodeLocation;

                    //aca actualiza la posicion del operador
                    if (TempLocation.LocationX == loc.LocationX && TempLocation.LocationY == loc.LocationY)
                    {
                        Console.WriteLine("Destination reached!");

                        Map.Grid[LocationP.LocationX, LocationP.LocationY].OperatorsInNode.Remove(this);
                        Map.Grid[goal.NodeLocation.LocationX, goal.NodeLocation.LocationY].OperatorsInNode.Add(this);
                        foreach (MechanicalOperator op in Map.GetInstance().HQList[hqNumber].Operators)
                        {
                            if (op.Id == opId) op.LocationP = goal.NodeLocation;
                        }
                    }

                    // Verifica si el tipo de terreno está en el diccionario y ejecuta la función correspondiente
                    if (TerrainDamages.TryGetValue(terrainType, out var action))
                    {
                        action.Invoke(this);
                    }

                    int timeSpentMoveToPerNode = SimulateTime(TimeSimulator.MoveToPerNode) * 10;
                    TimeSpent += timeSpentMoveToPerNode;

                }
            }

            double distance = CalculatePathDistance(path);
            double batteryConsumption = CalculateBatteryConsumption(distance);
            Battery.DecreaseBattery(batteryConsumption);
            KilometersTraveled += (float)distance;
            EnergyConsumed += (float)CalculateBatteryConsumption(distance);
            ExecutedInstructions++;


        }
        private double CalculateBatteryConsumption(double distance)
        {
            return 0.05 * (distance / 10);
        }

        public void LoadingLoad(double amountKG)
        {
            if (amountKG > 0 && DamageSimulatorP.StuckServo == false)
            {
                CurrentLoad = amountKG;
                TotalCarriedLoad += (float)amountKG;
                ExecutedInstructions++;
            }
        }

        public void TransferBattery(MechanicalOperator destination, double amountPercentage, bool safety, int whatHq, string opId)
        {
            destination.BusyStatus = true;
            BusyStatus = true;

            if (amountPercentage < 0)
            {
                Console.WriteLine("Amount must be non-negative for Transfer Battery.");
            }
            if (AreOperatorsInSameLocation(destination))
            {
                if (ValidateBatteryTransfer(amountPercentage))
                {
                    destination.Battery.ChargeBattery(amountPercentage);
                    Battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                    destination.BusyStatus = false;
                    BusyStatus = false;
                    SimulateTime(TimeSimulator.TransferBattery);
                    EnergyConsumed += (float)CalculateBatteryConsumption(amountPercentage);
                    ExecutedInstructions++;
                }
                else
                {
                    Console.WriteLine("Transfer Battery aborted due to battery validation failure.");
                    BusyStatus = false;
                }
            }
            else
            { // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP, safety, whatHq, opId);


                AStarAlgorithm astar = new AStarAlgorithm();
                Node currNode = Map.Grid[this.LocationP.LocationX, this.LocationP.LocationY];
                Node destinationNode = Map.Grid[destination.LocationP.LocationX, destination.LocationP.LocationY];
                List<Node> path = astar.FindPath(currNode, destinationNode, Map.Grid, safety, !this.Id.Contains("UAV"));
                double distance = CalculatePathDistance(path);

                if (ValidateBatteryTransfer(amountPercentage))
                {
                    destination.Battery.ChargeBattery(amountPercentage);
                    Battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                    Battery.DecreaseBattery(CalculateBatteryConsumption(distance));
                    destination.BusyStatus = false;
                    BusyStatus = false;
                    SimulateTime(TimeSimulator.TransferBattery);
                    EnergyConsumed += (float)CalculateBatteryConsumption(distance);
                    ExecutedInstructions++;
                }
            }

        }
        public void TransferLoad(MechanicalOperator destination, double amountKG, bool safety, int whatHq, string opId)
        {
            destination.BusyStatus = true;
            BusyStatus = true;
            if (amountKG < 0)
            {
                Console.WriteLine("Amount must be non-negative for TransferLoad.");
                destination.BusyStatus = false;
                BusyStatus = false;
                return;
            }

            if (AreOperatorsInSameLocation(destination))
            {
                //calcula que la carga actual mas lo que se quiera sumar no supere la carga maxima del operador
                if (destination.CurrentLoad + amountKG < destination.MaxLoad)
                {
                    destination.CurrentLoad += amountKG;
                    CurrentLoad -= amountKG;
                    destination.BusyStatus = false;
                    BusyStatus = false;
                    SimulateTime(TimeSimulator.TransferLoad);
                    ExecutedInstructions++;
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                    destination.BusyStatus = false;
                    BusyStatus = false;
                }
            }
            else
            {

                MoveTo(destination.LocationP, safety, whatHq, opId);
                AStarAlgorithm astar = new AStarAlgorithm();
                Node currNode = Map.Grid[this.LocationP.LocationX, this.LocationP.LocationY];
                Node destinationNode = Map.Grid[destination.LocationP.LocationX, destination.LocationP.LocationY];
                List<Node> path = astar.FindPath(currNode, destinationNode, Map.Grid, safety, !this.Id.Contains("UAV"));
                double distance = CalculatePathDistance(path);


                if (destination.CurrentLoad + amountKG <= destination.MaxLoad && ValidateBatteryTransfer(CalculateBatteryConsumption(distance)))
                {
                    destination.CurrentLoad += amountKG;
                    CurrentLoad -= amountKG;
                    Battery.DecreaseBattery(CalculateBatteryConsumption(distance));
                    destination.BusyStatus = false;
                    BusyStatus = false;
                    SimulateTime(TimeSimulator.TransferLoad);
                    EnergyConsumed += (float)CalculateBatteryConsumption(distance);
                    ExecutedInstructions++;
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                    destination.BusyStatus = false;
                    BusyStatus = false;
                }
            }
        }
        private double CalculatePathDistance(List<Node> nodes)
        {
            double totalDistance = 0;
            double distance = 10;

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                totalDistance += distance;
            }

            return totalDistance;
        }

        private double CalculateDistanceToNode(Location loc, Node node) //Returns Manhattan distance (distance estimate)
        {
            return Math.Abs(node.NodeLocation.LocationX - loc.LocationX) + Math.Abs(node.NodeLocation.LocationY - loc.LocationY);
        }
        private bool AreOperatorsInSameLocation(MechanicalOperator destination)
        {
            return LocationP == destination.LocationP;
        }

        public double CalculatePercentage(MechanicalOperator destination, double amountPercentage)
        {
            double increaseAmperes = destination.Battery.MAHCapacity * amountPercentage / 100;
            double decreasePercentage = 100 * increaseAmperes / Battery.MAHCapacity;

            return decreasePercentage;
        }
        public bool ValidateBatteryTransfer(double amountPercentage)
        {
            double decreasePercentage = CalculatePercentage(this, amountPercentage);

            if (Battery.CurrentChargePercentage >= decreasePercentage && !DamageSimulatorP.DisconnectedBatteryPort)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Battery validation failed. Not enough battery capacity for the transfer.");
                return false;
            }
        }
        public List<Node> GetLocal(Location A, int terrainType)
        {
            List<Node> nodeList = new List<Node>();

            for (int i = 0; i < Map.MapSize; i++)
            {
                for (int x = 0; x < Map.MapSize; x++)
                {
                    // Check if the terrain type matches
                    if (Map.Grid[i, x] != null && Map.Grid[x, i].TerrainType == terrainType)
                    {
                        // Add the node to the list
                        nodeList.Add(Map.Grid[x, i]);
                    }
                }
            }

            return nodeList;
        }
        private void MoveToAndProcess(Node destination, double loadAmount, bool safety, int whatHq, string opId)
        {
            MoveTo(destination.NodeLocation, safety, whatHq, opId);
            CurrentLoad = loadAmount;
        }
        private void HandleOrderWeight(Node[,] grid, int terrainType, double loadAmount, bool safety, int whatHq, string opId)
        {
            List<Node> closestNodes = GetLocal(LocationP, terrainType);
            Node mostClosestNode = FindClosestNode(closestNodes);
            MoveToAndProcess(mostClosestNode, loadAmount, safety, whatHq, opId);
        }
        private void HandleOrderHeal(Node[,] grid, int terrainType, double loadAmount, bool safety, int whatHq, string opId)
        {
            List<Node> closestNodes = GetLocal(LocationP, terrainType);
            Node mostClosestNode = FindClosestNode(closestNodes);
            MoveTo(mostClosestNode.NodeLocation, safety, whatHq, opId);
        }



        private Node FindClosestNode(List<Node> nodes)
        {
            if (nodes.Count == 0)
            {
                Console.WriteLine("The list of nodes is empty. Unable to find the closest node.");
            }

            Node closestNode = nodes[0];
            double minDistance = CalculateDistanceToNode(LocationP, nodes[0]);

            foreach (var node in nodes)
            {
                double distance = CalculateDistanceToNode(LocationP, node);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestNode = node;
                }
            }

            return closestNode;
        }



        private bool IsDamaged()
        {
            if (DamageSimulatorP.DamagedEngine || DamageSimulatorP.StuckServo || DamageSimulatorP.PerforatedBattery
                || DamageSimulatorP.DisconnectedBatteryPort || DamageSimulatorP.PaintScratch)
            {
                DamagesReceived++;
                return true;
            }
            else
            { return false; }

        }
        public Location FindHeadquartersLocation(Node[,] grid)
        {
            Location nearestHeadquarters = null;
            double minDistance = double.MaxValue;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null && grid[i, j].TerrainType == 5)
                    {
                        Location headquartersLocation = grid[i, j].NodeLocation;
                        double distance = CalculateDistanceToNode(new Location(LocationP.LocationX, LocationP.LocationY),
                          new Node(headquartersLocation.LocationX, headquartersLocation.LocationY));

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestHeadquarters = headquartersLocation;
                        }
                    }
                }
            }
            return nearestHeadquarters;
        }

        public Location FindDumpsterLocation(Node[,] grid)
        {
            Location nearestHeadquarters = null;
            double minDistance = double.MaxValue;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null && grid[i, j].TerrainType == 1)
                    {
                        Location headquartersLocation = grid[i, j].NodeLocation;
                        double distance = CalculateDistanceToNode(new Location(LocationP.LocationX, LocationP.LocationY),
                          new Node(headquartersLocation.LocationX, headquartersLocation.LocationY));

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestHeadquarters = headquartersLocation;
                        }
                    }
                }
            }
            return nearestHeadquarters;
        }

        public void BatteryChange(Node[,] grid, bool safety, int whatHq, string opId)
        {
            if (DamageSimulatorP.PerforatedBattery)
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters, safety, whatHq, opId);
                DamageSimulatorP.RepairBatteryOnly(this);
                SimulateTime(TimeSimulator.BatteryChange);
                ExecutedInstructions++;
            }
        }
        public void GeneralOrderHeal(Node[,] grid, string opId, int whatHq, bool safety)
        {
            if (IsDamaged())
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters, safety, whatHq, opId);

                DamageSimulatorP.Repair(this);
                SimulateTime(TimeSimulator.DamageRepair);
                ExecutedInstructions++;
            }
            else
            {
                Console.WriteLine("This operator is not damaged.");
            }
        }

        public void GeneralOrderWeight(Node[,] grid, string opId, int whatHq, bool safety)
        {
            if (!BusyStatus)
            {
                HandleOrderWeight(grid, 1, MaxLoad, safety, whatHq, opId);
                if (DistanceFlag)
                {
                    ExecutedInstructions++;
                    TotalCarriedLoad += (float)MaxLoad;
                }
            }
        }

    }
}
