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
    [JsonDerivedType(typeof(UAV), typeDiscriminator: "UAV")]
    [JsonDerivedType(typeof(K9), typeDiscriminator: "K9")]
    [JsonDerivedType(typeof(M8), typeDiscriminator: "M8")]
    public abstract class MechanicalOperator
    {

        private Dictionary<int, Action<MechanicalOperator>> terrainDamages;
        private List<int> LastVisitedLocations { get; set; }
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
        public double TimeSpent { get; private set; }

        public Dictionary<int, Action<MechanicalOperator>> TerrainDamages;
        public float KilometersTraveled { get; private set; }
        public float EnergyConsumed { get; private set; }
        public float TotalCarriedLoad { get; private set; }
        public double ExecutedInstructions { get; private set; }
        public double DamagesReceived { get; private set; }
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
                { Message.OperatorsLakeProhibition(); } return; } },
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
                { Message.OperatorsLakeProhibition(); } return; } },
              { 3, (oper) => DamageSimulatorP.ElectronicLandfillSimulate(oper) }
            };
            KilometersTraveled = 0;
            EnergyConsumed = 0;
            TotalCarriedLoad = 0;
            ExecutedInstructions = 0;
            DamagesReceived = 0;
            LastVisitedLocations = new List<int>();
        }

        //Returns the status via a ternary operator
        public string StatusString(bool busy)
        {
            return busy ? "Operator is not available" : "Operator is available";
        }

        //Adds task time to total time
        public int SimulateTime(TimeSimulator taskType)
        {
            int time = (int)taskType;
            TimeSpent += time;
            return time;
        }

        //Calculates movement speed taking debuffs into account
        public double CalculateMovementSpeed()
        {
            double batteryPercentageSpent = 100 - Battery.CurrentChargePercentage / Battery.MAHCapacity * 100;
            double slownessMultiplier = Math.Floor(batteryPercentageSpent / 10); ; //this line calculates how many times to apply the speed debuff
            double finalSpeed = OptimalSpeed - OptimalSpeed / 10 * slownessMultiplier;
            return finalSpeed;
        }

        //Movement function. 
        public bool MoveTo(Location loc, bool safety, int hqNumber, string opId)
        {
            double finalSpeed = CalculateMovementSpeed();
            OptimalSpeed = finalSpeed;
            DistanceFlag = true;
            int terrainType = Map.Grid[LocationP.LocationX, LocationP.LocationY].TerrainType;

            Node start = new Node(LocationP.LocationX, LocationP.LocationY);
            Node goal = new Node(loc.LocationX, loc.LocationY);

            AStarAlgorithm astar = new AStarAlgorithm();

            bool isWalkingUnit = !Id.Contains("UAV");
            List<Node> path = astar.FindPath(start, goal, Map.Grid, safety, isWalkingUnit);

            if (path.Count == 0)
            {
                Message.PathNotFound();
                DistanceFlag = false;
                return false; // Early return when no path is found
            }

            ProcessMovement(path, loc, hqNumber, opId, terrainType);

            double distance = CalculatePathDistance(path);
            double batteryConsumption = CalculateBatteryConsumption(distance);

            Battery.DecreaseBattery(batteryConsumption);
            KilometersTraveled += (float)distance;
            EnergyConsumed += (float)batteryConsumption;
            ExecutedInstructions++;
            AddToLastVisitedLocations(loc);

            return true;
        }

        //Interacts with each node in the path and applies the corresponding debuffs. 
        private void ProcessMovement(List<Node> path, Location destination, int hqNumber, string opId, int terrainType)
        {
            int nodeCounter = 0;
            foreach (Node node in path)
            {
                nodeCounter++;
                Location tempLocation = node.NodeLocation;

                if (tempLocation.Equals(destination))
                {
                    Message.DestinationReached();

                    Map.Grid[LocationP.LocationX, LocationP.LocationY].OperatorsInNode.Remove(this);
                    Map.Grid[node.NodeLocation.LocationX, node.NodeLocation.LocationY].OperatorsInNode.Add(this);

                    foreach (MechanicalOperator op in Map.GetInstance().HQList[hqNumber].Operators)
                    {
                        if (op.Id == opId) op.LocationP = node.NodeLocation;
                    }
                }

                // Verifica si el tipo de terreno está en el diccionario y ejecuta la función correspondiente
                if (TerrainDamages.TryGetValue(terrainType, out var action))
                {
                    action.Invoke(this);
                }

                int timeSpentMoveToPerNode = SimulateTime(TimeSimulator.MoveToPerNode) * nodeCounter;
                TimeSpent += timeSpentMoveToPerNode + timeSpentMoveToPerNode * ((100 - OptimalSpeed) / 100);//This alters the time spent in relation to the speed of the operator. 
            }
        }

        //Calculates battery consumption to simulate realistic energy consumption during operator movement per nodes.
        private double CalculateBatteryConsumption(double distance)
        {
            return 0.05 * (distance / 10);
        }

        // Method designed for future-proofing functionalities. Loads the specified weight if it is valid and the servo is not stuck. (Unused, but relevant)
        public void LoadWeight(double amountKG)
        {
            if (IsValidWeight(amountKG) && !DamageSimulatorP.StuckServo)
            {
                CurrentLoad = amountKG;
                TotalCarriedLoad += (int)amountKG;
                ExecutedInstructions++;
            }
        }

        private bool IsValidWeight(double amountKG)
        {
            return amountKG >= 0;
        }


        public void TransferBattery(MechanicalOperator destination, double amountPercentage, bool safety, int whatHq, string opId)
        {
            try
            {
                // Set both operators to busy
                destination.BusyStatus = true;
                BusyStatus = true;

                if (amountPercentage < 0)
                {
                    Message.AmontNonNegative();
                    return;
                }

                if (AreOperatorsInSameLocation(destination))
                {
                    PerformBatteryTransferSameLocation(destination, amountPercentage);
                }
                else
                {
                    // Move the current operator to the destination location
                    MoveTo(destination.LocationP, safety, whatHq, opId);

                    // Calculate the path and distance to the destination
                    AStarAlgorithm astar = new AStarAlgorithm();
                    Node currNode = Map.Grid[this.LocationP.LocationX, this.LocationP.LocationY];
                    Node destinationNode = Map.Grid[destination.LocationP.LocationX, destination.LocationP.LocationY];
                    List<Node> path = astar.FindPath(currNode, destinationNode, Map.Grid, safety, !this.Id.Contains("UAV"));
                    double distance = CalculatePathDistance(path);

                    // Validate and perform battery transfer
                    PerformBatteryTransferDifferentLocation(destination, amountPercentage, distance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the battery transfer: {ex.Message}");
            }
            finally
            {
                destination.BusyStatus = false;
                BusyStatus = false;
            }
        }

        //Performs a battery transefer
        private void PerformBatteryTransferSameLocation(MechanicalOperator destination, double amountPercentage)
        {
            if (ValidateBatteryTransfer(amountPercentage))
            {
                // Perform battery transfer between operators
                destination.Battery.ChargeBattery(amountPercentage);
                Battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));

                // Simulate time, update energy consumed, increment executed instructions
                SimulateTime(TimeSimulator.TransferBattery);
                ExecutedInstructions++;
            }
            else
            {
                Message.BatteryValidationFailure();
            }
        }

        //Performs a battery transfer while accounting for travel costs
        private void PerformBatteryTransferDifferentLocation(MechanicalOperator destination, double amountPercentage, double distance)
        {
            if (ValidateBatteryTransfer(amountPercentage))
            {
                // Perform battery transfer between operators
                destination.Battery.ChargeBattery(amountPercentage);
                Battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                Battery.DecreaseBattery(CalculateBatteryConsumption(distance));

                // Simulate time, update energy consumed, and increment executed instructions
                SimulateTime(TimeSimulator.TransferBattery);
                EnergyConsumed += (float)CalculateBatteryConsumption(distance);
                ExecutedInstructions++;
            }
            else
            {
                Message.BatteryValidationFailure();
            }
        }

        //Transfers a physical load between operators. 
        public void TransferLoad(MechanicalOperator destination, double amountKG, bool safety, int whatHq, string opId)
        {
            try
            {
                // Set both operators to busy
                destination.BusyStatus = true;
                BusyStatus = true;

                if (amountKG < 0)
                {
                    Message.AmontNonNegativeLoad();
                    return;
                }

                if (AreOperatorsInSameLocation(destination))
                {
                    PerformLoadTransfer(destination, amountKG);
                }
                else
                {
                    // Move the current operator to the destination location
                    MoveTo(destination.LocationP, safety, whatHq, opId);

                    // Calculate the path and distance to the destination
                    AStarAlgorithm astar = new AStarAlgorithm();
                    Node currNode = Map.Grid[this.LocationP.LocationX, this.LocationP.LocationY];
                    Node destinationNode = Map.Grid[destination.LocationP.LocationX, destination.LocationP.LocationY];
                    List<Node> path = astar.FindPath(currNode, destinationNode, Map.Grid, safety, !this.Id.Contains("UAV"));
                    double distance = CalculatePathDistance(path);

                    // Validate and perform load transfer with battery consumption
                    PerformLoadTransferWithBatteryConsumption(destination, amountKG, distance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the load transfer: {ex.Message}");
            }
            finally
            {
                destination.BusyStatus = false;
                BusyStatus = false;
            }
        }

        //This is used for load transfers in the same location
        private void PerformLoadTransfer(MechanicalOperator destination, double amountKG)
        {
            // Check if destination operator can hold the load
            if (destination.CurrentLoad + amountKG <= destination.MaxLoad)
            {
                // Perform load transfer
                destination.CurrentLoad += amountKG;
                CurrentLoad -= amountKG;

                // Simulate time and increment executed instructions
                SimulateTime(TimeSimulator.TransferLoad);
                ExecutedInstructions++;
            }
            else
            {
                Message.MuchLoad();
            }
        }

        //This is used for load transfers in different locations
        private void PerformLoadTransferWithBatteryConsumption(MechanicalOperator destination, double amountKG, double distance)
        {
            // Check if destination operator can hold the load and validate battery transfer
            if (destination.CurrentLoad + amountKG <= destination.MaxLoad && ValidateBatteryTransfer(CalculateBatteryConsumption(distance)))
            {
                // Perform load transfer with battery consumption
                destination.CurrentLoad += amountKG;
                CurrentLoad -= amountKG;
                Battery.DecreaseBattery(CalculateBatteryConsumption(distance));

                // Simulate time, update energy consumed, and increment executed instructions
                SimulateTime(TimeSimulator.TransferLoad);
                EnergyConsumed += (float)CalculateBatteryConsumption(distance);
                ExecutedInstructions++;
            }
            else
            {
                Message.MuchLoad();
            }
        }

        //Calculates the distance in real units between nodes (10km)
        private double CalculatePathDistance(List<Node> nodes)
        {
            double totalDistance = 0;
            const double distanceInterval = 10; //Distance interval between nodes. 

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                totalDistance += distanceInterval;
            }

            return totalDistance;
        }

        //This function returns the Manhattan estimate between two node locations
        private double CalculateDistanceToNode(Location loc, Node node)
        {
            return Math.Abs(node.NodeLocation.LocationX - loc.LocationX) + Math.Abs(node.NodeLocation.LocationY - loc.LocationY);
        }

        private bool AreOperatorsInSameLocation(MechanicalOperator destination)
        {
            return LocationP.Equals(destination.LocationP);
        }

        //Calculates battery drain
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
                Message.BatteryCapacity();
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

        //Handles general orders requiring weight load
        private void HandleOrderWeight(Node[,] grid, int terrainType, double loadAmount, bool safety, int whatHq, string opId)
        {
            List<Node> closestNodes = GetLocal(LocationP, terrainType);
            Node mostClosestNode = FindClosestNode(closestNodes);
            MoveToAndProcess(mostClosestNode, loadAmount, safety, whatHq, opId);
        }

        //Handles general orders requiring healing 
        private void HandleOrderHeal(Node[,] grid, int terrainType, bool safety, int whatHq, string opId)
        {
            List<Node> closestNodes = GetLocal(LocationP, terrainType);
            Node mostClosestNode = FindClosestNode(closestNodes);
            MoveTo(mostClosestNode.NodeLocation, safety, whatHq, opId);
        }



        private Node FindClosestNode(List<Node> nodes)
        {
            if (nodes.Count == 0)
            {
                Message.NodeListEmpty();
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


        //Checks for operator damages
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

        //Finds the nearest HQ
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

        //Chances the battery
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

        //Wrapper for the healing general order
        public void GeneralOrderHeal(Node[,] grid, string opId, int whatHq, bool safety)
        {
            if (IsDamaged())
            {
                HandleOrderHeal(grid, 5, safety, whatHq, opId); //5 is the headquarter terrain type code
                DamageSimulatorP.Repair(this);
                SimulateTime(TimeSimulator.DamageRepair);
                ExecutedInstructions++;
            }
            else
            {
                Message.OperatorNotDamaged();
            }
        }

        //Wrapper for the loading weight general order
        public void GeneralOrderWeight(Node[,] grid, string opId, int whatHq, bool safety)
        {
            if (!BusyStatus)
            {
                HandleOrderWeight(grid, 1, MaxLoad, safety, whatHq, opId); //1 is the dumpster terrain type code
                if (DistanceFlag)
                {
                    ExecutedInstructions++;
                    TotalCarriedLoad += (float)MaxLoad;
                }
            }
        }

        //Stores the last 3 visited locations to be logged in SQL
        private void AddToLastVisitedLocations(Location location)
        {
            int loc = Convert.ToInt32(location.ToString());
            if (loc != null)
            {
                LastVisitedLocations.Add(loc);

                if (LastVisitedLocations.Count > 3)
                {
                    LastVisitedLocations.RemoveAt(0);
                }
            }
            else { loc = 0; }
        }

        public List<int> GetAndClearLastVisitedLocations()
        {
            List<int> lastVisited = new List<int>(LastVisitedLocations);

            LastVisitedLocations.Clear();

            return lastVisited;
        }

    }
}
