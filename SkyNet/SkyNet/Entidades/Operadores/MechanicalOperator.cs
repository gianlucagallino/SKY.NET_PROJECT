using SkyNet.Entidades.Mapa;

namespace SkyNet.Entidades.Operadores
{
    public abstract class MechanicalOperator
    {
        /*
        protected string id;
        protected bool busyStatus;
        protected Battery battery;
        protected string status;
        protected double maxLoad;
        protected double maxLoadOriginal;
        protected double currentLoad;
        protected float optimalSpeed;
        private DamageSimulator damageSimulator;
        private Node[,] grid;*/
        private Dictionary<int, Action<MechanicalOperator>> terrainDamages;
        //protected int timeSpent;

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

        }

        protected MechanicalOperator(double maxLoad, double minLoad, Battery battery, Location location, string status, string id)
        {
            MaxLoad = maxLoad;
            MaxLoadOriginal = minLoad;
            Battery = new Battery();
            LocationP = location;
            Status = status;
            Id = id;

        }

        protected MechanicalOperator(int xposition, int yposition)
        {
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

        public void MoveTo(Location loc, bool safety)
        {
            //double finalSpeed = CalculateMovementSpeed();
            // OptimalSpeed = finalSpeed;

            int terrainType = Map.Grid[LocationP.LocationX, LocationP.LocationY].TerrainType; //ERROR: Object reference not set to an instance of an object

            Node start = new Node(LocationP.LocationX, LocationP.LocationY);
            Node goal = new Node(loc.LocationX, loc.LocationY);

            AStarAlgorithm astar = new AStarAlgorithm();

            bool isWalkingUnit = true;
            if (Id.Contains("UAV"))
            {
                isWalkingUnit = false;
            }

            List<Node> path = astar.FindPath(start, goal, Map.Grid, safety, isWalkingUnit);


            if (path.Count==null){
                Console.WriteLine("No se encontro camino");
            } //path fue null. safe search

            if (path != null)
            {
                foreach (Node node in path)
                {
                    LocationP = node.NodeLocation;

                    //aca actualiza la posicion del operador
                    if (LocationP.Equals(loc))
                    {
                        Console.WriteLine("Destination reached!");
                        break;
                    }
                }
                //ANIMAR LA COSA
            }
            else
            {
                Console.WriteLine("No path found.");
            }


            double distance = CalculateDistance(path);
            double batteryConsumption = CalculateBatteryConsumption(distance);
            Battery.DecreaseBattery(batteryConsumption);

            // Verifica si el tipo de terreno está en el diccionario y ejecuta la función correspondiente
            if (TerrainDamages.TryGetValue(terrainType, out var action)) 
            {
                action.Invoke(this);
            }

            int timeSpentMoveToPerNode = SimulateTime(TimeSimulator.MoveToPerNode) * 10;
            TimeSpent += timeSpentMoveToPerNode;

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
            }

        }
        public void TransferBattery(MechanicalOperator destination, double amountPercentage)
        {
            destination.BusyStatus = true;
            BusyStatus = true;

            if (amountPercentage < 0)
            {
                Console.WriteLine("Amount must be non-negative for Transfer Battery.");
                return;
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
                }
                else
                {
                    Console.WriteLine("Transfer Battery aborted due to battery validation failure.");
                    BusyStatus = false;
                }
            }
            else
            { // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP, true); //Al ser una operacion de rescate, es esencial que la nave que rescata no se dañe. 


                double distance = CalculateDistance(new List<Node> { new Node(LocationP.LocationX, LocationP.LocationY),
                new Node(destination.LocationP.LocationX, destination.LocationP.LocationY) });


                if (ValidateBatteryTransfer(amountPercentage))
                {
                    destination.Battery.ChargeBattery(amountPercentage);
                    Battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                    Battery.DecreaseBattery(CalculateBatteryConsumption(distance));
                    destination.BusyStatus = false;
                    BusyStatus = false;
                    SimulateTime(TimeSimulator.TransferBattery);
                }
            }
        }
        public void TransferLoad(MechanicalOperator destination, double amountKG)
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

                MoveTo(destination.LocationP, true); //no se le da la opcion de seleccion al usuario, la seguridad es importante. 

                double distance = CalculateDistance(new List<Node> { new Node(LocationP.LocationX, LocationP.LocationY),
                new Node(destination.LocationP.LocationX, destination.LocationP.LocationY) });


                if (destination.CurrentLoad + amountKG <= destination.MaxLoad && ValidateBatteryTransfer(CalculateBatteryConsumption(distance)))
                {
                    destination.CurrentLoad += amountKG;
                    CurrentLoad -= amountKG;
                    Battery.DecreaseBattery(CalculateBatteryConsumption(distance));
                    destination.BusyStatus = false;
                    BusyStatus = false;
                    SimulateTime(TimeSimulator.TransferLoad);
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                    destination.BusyStatus = false;
                    BusyStatus = false;
                }
            }
        }
        //AJUSTAR A SEARCH Y ASTAR
        private double CalculateDistance(List<Node> nodes)
        {
            double totalDistance = 0;

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                double distance = 10;
                totalDistance += distance;
            }

            return totalDistance;
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
        public List<Node> GetLocal(Location A, int terrainType, Node[,] grilla)
        {
            List<Node> nodeList = new List<Node>();

            foreach (var square in grilla)
            {
                if (square != null && square.NodeLocation.Equals(A) && square.TerrainType == terrainType)
                {
                    nodeList.Add(square);
                }
            }

            return nodeList;
        }
        private Node FindClosestNode(List<Node> nodes)
        {
            if (nodes.Count == 0)
            {
                Console.WriteLine("The list of nodes is empty. Unable to find the closest node.");
            }

            Node closestNode = nodes[0];
            double minDistance = double.MaxValue;

            foreach (var node in nodes)
            {
                double distance = CalculateDistance(nodes);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestNode = node;
                }
            }

            return closestNode;
        }
        private void HandleOrder(Node[,] grid, int terrainType, double loadAmount)
        {
            List<Node> closestNodes = GetLocal(LocationP, terrainType, grid);
            Node mostClosestNode = FindClosestNode(closestNodes);
            MoveToAndProcess(mostClosestNode, loadAmount);
        }
        private void MoveToAndProcess(Node destination, double loadAmount)
        {
            MoveTo(destination.NodeLocation, true); //ESTE SAFE ES TEMPORAL; HAY QUE PREGUNTAR
            CurrentLoad = loadAmount;
        }
        private bool IsDamaged()
        {
            if (DamageSimulatorP.DamagedEngine || DamageSimulatorP.StuckServo || DamageSimulatorP.PerforatedBattery
                || DamageSimulatorP.DisconnectedBatteryPort || DamageSimulatorP.PaintScratch)
            {
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
                        double distance = CalculateDistance(new List<Node> { new Node(LocationP.LocationX, LocationP.LocationY),
                          new Node(headquartersLocation.LocationX, headquartersLocation.LocationY) });

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

        public void BatteryChange(Node[,] grid)
        {
            if (DamageSimulatorP.PerforatedBattery)
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters, false); //aca se podria preguntar, igual hay que refactorizar. pero asumiendo que tiene bateria limitada, preferible que sea camino optimo
                DamageSimulatorP.RepairBatteryOnly(this);
                SimulateTime(TimeSimulator.BatteryChange);
            }
        }
        public void GeneralOrder(Node[,] grid)
        {
            if (!BusyStatus)
            {

                HandleOrder(grid, 3, MaxLoad);

                HandleOrder(grid, 4, 0);
            }
            else if (IsDamaged())
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters, true);//lo  mismo, preguntar

                DamageSimulatorP.Repair(this);
                SimulateTime(TimeSimulator.DamageRepair);
            }
            DamageSimulatorP.Repair(this);
        }
    }
}
