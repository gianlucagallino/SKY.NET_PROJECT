using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal abstract class MechanicalOperator
    {
        protected string id;
        protected bool busyStatus;
        protected Battery battery;
        protected string status;
        protected double maxLoad;
        protected double maxLoadOriginal;
        protected double currentLoad;
        protected float optimalSpeed;
        protected Location location;
        private DamageSimulator simulateDamage;

        public string Id { get; set; }
        public bool BusyStatus { get; set; }
        public Battery Battery { get; set; }
        public string Status { get; set; }
        public double MaxLoad { get; set; }
        public double MaxLoadOriginal { get; set; }
        public double CurrentLoad { get; set; }
        public float OptimalSpeed { get; set; }
        public Location LocationP { get; set; }
        public DamageSimulator SimulateDemage { get; set; }


        //Hay que revisar este constructor. El profe menciono que debian tener valores no vacios
        public MechanicalOperator()
        {
            id = string.Empty;
            busyStatus = false;
            battery = new Battery();
            //battery.CurrentCharge = battery.MAHCapacity;
            status = "ACTIVE";
            maxLoad = 1000;
            maxLoadOriginal = 0;
            currentLoad = 0;
            optimalSpeed = 100;
           simulateDamage = new DamageSimulator();

           //LocationP = new Location();
            simulateDamage = new DamageSimulator();
        }

        protected MechanicalOperator(double maxLoad, double minLoad, Battery battery, Location location, string status, string id)
        {
            this.maxLoad = maxLoad;
            this.maxLoadOriginal = minLoad;
            this.battery = battery;
            this.location = location;
            this.status = status;
            this.id = id;
        }

        public double CalculateMovementSpeed()
        {
            double batteryPercentageSpent = 100 - ((Battery.CurrentChargePercentage / Battery.MAHCapacity) * 100);
            double slownessMultiplier = batteryPercentageSpent % 10; //this line calculates how many times to apply the speed debuff
            double finalSpeed = OptimalSpeed - ((OptimalSpeed / 10) * slownessMultiplier);
            return finalSpeed;
        }

        public void MoveTo(Location loc)
        {
            simulateDamage.SimulateRandomDamage(this);

            double x = loc.LocationX;
            double y = loc.LocationY;
            int movX = 0;
            int movY = 0;
            busyStatus = true;
            //Se asigna que tipo de movimiento debe ser realizado para llegar a la cuadrilla que corresponde. 
            if (LocationP.LocationX < x)
            {
                movX = 1;
            }
            else if (LocationP.LocationX > x)
            {
                movX = -1;
            }

            if (LocationP.LocationY < y)
            {
                movY = 1;
            }
            else if (LocationP.LocationY > y)
            {
                movY = -1;
            }

            //se desplaza la posicion actual a la posicion buscada 

            while (LocationP.LocationY != y)
            {
                LocationP.LocationY += movY;
                /*
                InteractuarConPosicion() 

                Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                 que dependiendo del tipo de terreno tiene diferentes efectos
                */
            }
            while (LocationP.LocationX != x)
            {
                LocationP.LocationX += movX;
                /*
                InteractuarConPosicion() 

                Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                 que dependiendo del tipo de terreno tiene diferentes efectos
                */
            }
        }

        private double CalculateBatteryConsumption(double distance)
        {
            return 0.05 * (distance / 10); // Ajusta según tus necesidades
        }
        public void TransferBattery(MechanicalOperator destination, double amountPercentage)
        {
            simulateDamage.SimulateRandomDamage(this);
            destination.busyStatus = true;
            busyStatus = true;
            //calcula que la carga no sea negativa
            if (amountPercentage < 0)
            {
                Console.WriteLine("Amount must be non-negative for Transfer Battery.");
                return;
            }
            if (AreOperatorsInSameLocation(destination))
            {
                if (ValidateBatteryTransfer(amountPercentage))
                {
                    destination.battery.ChargeBattery(amountPercentage);
                    battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                    destination.busyStatus = false;
                    busyStatus = false;
                }
                else 
                { Console.WriteLine("Transfer Battery aborted due to battery validation failure.");
                    busyStatus = false;
                }
            }
            else
            { // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP);

                // Calcula la distancia entre los operadores y disminuye la batería del operador actual.
                double distance = CalculateDistance(destination.LocationP);
                // TODO valores a revisar creo q vuelve a ser el optimal speed

                if (ValidateBatteryTransfer(amountPercentage))
                {
                    destination.battery.ChargeBattery(amountPercentage);
                    battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                    battery.DecreaseBattery(CalculateBatteryConsumption(distance));
                    destination.busyStatus = false;
                    busyStatus = false;
                }
            }
        }

        public void TransferLoad(MechanicalOperator destination, double amountKG)
        {
            simulateDamage.SimulateRandomDamage(this);
            destination.busyStatus = true;
            busyStatus = true;
            if (amountKG < 0)
            {
                Console.WriteLine("Amount must be non-negative for TransferLoad.");
                return;
                destination.busyStatus = false;
                busyStatus = false;
            }
            //compara si estan en la misma ubicacion
            if (AreOperatorsInSameLocation(destination))
            {
                //calcula que la carga actual mas lo que se quiera sumar no supere la carga maxima del operador
                if (destination.currentLoad + amountKG < destination.MaxLoad)
                {
                    destination.currentLoad += amountKG;
                    currentLoad -= amountKG;
                    destination.busyStatus = false;
                    busyStatus = false;
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                    destination.busyStatus = false;
                    busyStatus = false;
                }
            }
            else
            {
                // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP);

                // Calcula la distancia entre los operadores y disminuye la batería del operador actual.
                double distance = CalculateDistance(destination.LocationP);
                //TODO valores a revisar esto no esta calculado en la velocidad optima?


                // Luego, realiza la transferencia de carga.
                if (destination.currentLoad + amountKG <= destination.MaxLoad && ValidateBatteryTransfer(CalculateBatteryConsumption(distance)))
                {
                    destination.currentLoad += amountKG;
                    currentLoad -= amountKG;

                    battery.DecreaseBattery(CalculateBatteryConsumption(distance));
                    destination.busyStatus = false;
                    busyStatus = false;
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                    destination.busyStatus = false;
                    busyStatus = false;
                }
            }
        }

        private double CalculateDistance(Location destinationLocation)
        {

            double difCoordX = Math.Abs(LocationP.LocationX - destinationLocation.LocationX);
            double difCoordY = Math.Abs(LocationP.LocationY - destinationLocation.LocationY);
            double distance = difCoordX + difCoordY;

            return distance;
        }


        private bool AreOperatorsInSameLocation(MechanicalOperator destination)
        {
            return location == destination.location;
        }
        
        public double CalculatePercentage(MechanicalOperator destination, double amountPercentage)
        {
            double increaseAmperes = (destination.battery.MAHCapacity*amountPercentage)/100;
            double decreasePercentage = (100 * increaseAmperes) / battery.MAHCapacity;
            
            return decreasePercentage;
        }
        public bool ValidateBatteryTransfer(double amountPercentage)
        {
            double decreasePercentage = CalculatePercentage(this, amountPercentage);

            if (battery.CurrentChargePercentage >= decreasePercentage && !simulateDamage.DisconnectedBatteryPort)
            {
                return true; 
            }
            else
            {
                Console.WriteLine("Battery validation failed. Not enough battery capacity for the transfer.");
                return false; 
            }
        }
        //Metodos de nuevas funcionalidades TP Parte 2

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
                return null;
            }

            Node closestNode = nodes[0];
            double minDistance = double.MaxValue;

            foreach (var node in nodes)
            {
                double distance = CalculateDistance(node.NodeLocation);
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
            MoveTo(destination.NodeLocation);
            currentLoad = loadAmount;
        }

        /* esto hay que cambiarlo.
        private bool IsDamaged()
        {
            if (DamageSimulator.DamagedEngine||SimulateDemage.StuckServo||SimulateDemage.PerforatedBattery
                ||SimulateDemage.DisconnectedBatteryPort||SimulateDemage.PaintScratch)
            {
                return true;
            }
            else return false;

        }*/

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
                        double distance = CalculateDistance(headquartersLocation);

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
            if (simulateDamage.PerforatedBattery)
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters);
                simulateDamage.RepairBatteryOnly(this);
            }
        }

        public void GeneralOrder(Node[,] grid)
        {
            if (!busyStatus)
            {
                HandleOrder(grid, 3, MaxLoad);
                HandleOrder(grid, 4, 0);
            }
            /*else if(IsDamaged())
            {
                Location nearestHeadquarters = FindHeadquartersLocation(grid);
                MoveTo(nearestHeadquarters);
                simulateDamage.Repair(this);
            }*/
            
        }
    }
}