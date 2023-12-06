﻿using SkyNet.Data;
using SkyNet.Entidades.Mapa;
using System;
using System.Text.Json.Serialization;

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
        //AGREGO PARA BASE DE DATOS
        public double KilometersTraveled { get; private set; }
        public double EnergyConsumed { get; private set; } 
        public double TotalCarriedLoad { get; private set; }
        public int ExecutedInstructions { get; private set; }
        public int DamagesReceived { get; private set; }

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


            if (path.Count == 0)
            {
                Console.WriteLine("No path found for this unit.");
            } 

            if (path != null)
            {
                foreach (Node node in path)
                {
                    Location TempLocation = node.NodeLocation;

                    //aca actualiza la posicion del operador
                    if (TempLocation.LocationX==loc.LocationX && TempLocation.LocationY==loc.LocationY)
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

            double distance = CalculateDistance(path);
            double batteryConsumption = CalculateBatteryConsumption(distance);
            Battery.DecreaseBattery(batteryConsumption);
            KilometersTraveled += CalculateDistance(path);
            EnergyConsumed += CalculateBatteryConsumption(distance);
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
                TotalCarriedLoad += amountKG;
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
                    EnergyConsumed += CalculateBatteryConsumption(amountPercentage);
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
                    EnergyConsumed += CalculateBatteryConsumption(distance);
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
                    EnergyConsumed += CalculateBatteryConsumption(distance);
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
        private double CalculateDistance(List<Node> nodes)
        {
            double totalDistance = 0;
            double distance = 10;

            for (int i = 0; i < nodes.Count - 1; i++)
            {
       
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
        public List<Node> GetLocal(Location A, int terrainType) //Renombrar
        {
            List<Node> nodeList = new List<Node>();
            /*
            foreach (Node square in Map.Grid)
            {
                if (/*square != null && square.NodeLocation.Equals(A) && square.TerrainType == terrainType)
                {
                    nodeList.Add(square);
                }
            }
            */
            for (int i = 0; i < Map.MapSize; i++)
            {
                for (int x = 0; x < Map.MapSize; x++)
                {
                    if (/*Map.Grid[i,x] != null /*&& square.NodeLocation.Equals(A) *//*&&*/ Map.Grid[x, i].TerrainType == terrainType)
                    {
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
        private void HandleOrder(Node[,] grid, int terrainType, double loadAmount, bool safety, int whatHq, string opId)
        {
            List<Node> closestNodes = GetLocal(LocationP, terrainType);
            Node mostClosestNode = FindClosestNode(closestNodes);
            MoveToAndProcess(mostClosestNode, loadAmount, safety, whatHq, opId);
        }

        private Node FindClosestNode(List<Node> nodes)
        {
            if (nodes.Count == 0)
            {
                Console.WriteLine("The list of nodes is empty. Unable to find the closest node.");
            }

            Node closestNode = nodes[0];//tira OUT OF RANGE para el general order
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
        


        private bool IsDamaged()
        {
            if (DamageSimulatorP.DamagedEngine || DamageSimulatorP.StuckServo || DamageSimulatorP.PerforatedBattery
                || DamageSimulatorP.DisconnectedBatteryPort || DamageSimulatorP.PaintScratch)
            {
                return true;
                DamagesReceived++;
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
        public void GeneralOrder(Node[,] grid, string opId, int whatHq, bool safety)
        {
            

            if (!BusyStatus)
            {

                HandleOrder(grid, 3, MaxLoad, safety, whatHq, opId);

                HandleOrder(grid, 4, 0, safety, whatHq, opId);
                ExecutedInstructions++;
            }
            else if (IsDamaged())
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters, safety, whatHq, opId);

                DamageSimulatorP.Repair(this);
                SimulateTime(TimeSimulator.DamageRepair);
                ExecutedInstructions++;
            }
            DamageSimulatorP.Repair(this);
        }

    }
}
