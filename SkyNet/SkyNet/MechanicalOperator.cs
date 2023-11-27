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
        protected Battery battery;
        protected string status;
        protected double maxLoad;
        protected double currentLoad;
        protected float optimalSpeed;
        protected Location location;

        public string Id { get; set; }
        public Battery Battery { get; set; }
        public string Status { get; set; }
        public double MaxLoad { get; set; }
        public double CurrentLoad { get; set; }
        public float OptimalSpeed { get; set; }
        public Location LocationP { get; set; }


        //Hay que revisar este constructor. El profe menciono que debian tener valores no vacios
        public MechanicalOperator()
        {
            id = string.Empty;
            battery = new Battery();
            //battery.CurrentCharge = battery.MAHCapacity;
            status = "ACTIVE";
            maxLoad = 1000;
            currentLoad = 0;
            optimalSpeed = 100;
            //LocationP = new Location();
        }

        protected MechanicalOperator(double maxLoad, Battery battery, Location location, string status, string id)
        {
            this.maxLoad = maxLoad;
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
            double x = loc.LocationX;
            double y = loc.LocationY;
            int movX = 0;
            int movY = 0;
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

        public void TransferBattery(MechanicalOperator destination, double amountPercentage)
        {
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
                }
                else { Console.WriteLine("Transfer Battery aborted due to battery validation failure."); }
            }
            else
            { // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP);

                // Calcula la distancia entre los operadores y disminuye la batería del operador actual.
                double distance = CalculateDistance(destination.LocationP);
                double batteryConsumptionPercentage = 0.05 * (distance / 10);// TODO valores a revisar creo q vuelve a ser el optimal speed

                if (ValidateBatteryTransfer(amountPercentage))
                {
                    destination.battery.ChargeBattery(amountPercentage);
                    battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
                    battery.DecreaseBattery(batteryConsumptionPercentage);
                }
            }
        }

        public void TransferLoad(MechanicalOperator destination, double amountKG)
        {
            if (amountKG < 0)
            {
                Console.WriteLine("Amount must be non-negative for TransferLoad.");
                return;
            }
            //compara si estan en la misma ubicacion
            if (AreOperatorsInSameLocation(destination))
            {
                //calcula que la carga actual mas lo que se quiera sumar no supere la carga maxima del operador
                if (destination.currentLoad + amountKG < destination.MaxLoad)
                {
                    destination.currentLoad += amountKG;
                    currentLoad -= amountKG;
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                }
            }
            else
            {
                // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP);

                // Calcula la distancia entre los operadores y disminuye la batería del operador actual.
                double distance = CalculateDistance(destination.LocationP);
                double batteryConsumptionPercentage = 0.05 * (distance / 10);//TODO valores a revisar esto no esta calculado en la velocidad optima?


                // Luego, realiza la transferencia de carga.
                if (destination.currentLoad + amountKG <= destination.MaxLoad && ValidateBatteryTransfer(batteryConsumptionPercentage))
                {
                    destination.currentLoad += amountKG;
                    currentLoad -= amountKG;
                    
                    battery.DecreaseBattery(batteryConsumptionPercentage);
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
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
        /*Estos eran los metodos anteriores:
        Los transfer me dejan en duda del tipo que deberian ser, considerando que para los k9 se le deberia meter un k9, un m8 debe ingresar un m8 y asi. Lo dejo comentado por ahora. 
        Este seria un buen uso de genericos?
        ademas, no haria las verificaciones aca, pero si en el metodo que llama los transfer. 
        
        public void TransferBattery(TIPO A ARREGLAR destination, float amount)
        {
            currentBattery-=amount;
            destination.setCurrentBattery = destination.getCurrentBattery + amount;
        }

        public void TransferLoad(TIPO A ARREGLAR destination, float amount)
        {
            currentLoad-=amount;
            destination.setCurrentLoad = destination.getCurrentLoad + amount;
        
        }
        
        public void ReturnToHQandRemoveLoad()
        {
            LocationP.LocationX = HeadQuarters.GetInstance().LocationHeadQuarters.LocationX;
            LocationP.LocationY = HeadQuarters.GetInstance().LocationHeadQuarters.LocationY;
            CurrentLoad = 0;
        }

        public void ReturnToHQandChargeBattery()
        {
            LocationP.LocationX = HeadQuarters.GetInstance().LocationHeadQuarters.LocationX;
            LocationP.LocationY = HeadQuarters.GetInstance().LocationHeadQuarters.LocationY;
            battery.CompleteBatteryLevel();
        }*/
        public double CalculatePercentage(MechanicalOperator destination, double amountPercentage)
        {
            double increaseAmperes = (destination.battery.MAHCapacity*amountPercentage)/100;
            double decreasePercentage = (100 * increaseAmperes) / battery.MAHCapacity;
            
            return decreasePercentage;
        }
        public bool ValidateBatteryTransfer(double amountPercentage)
        {
            double decreasePercentage = CalculatePercentage(this, amountPercentage);

            if (battery.CurrentChargePercentage >= decreasePercentage)
            {
                return true; 
            }
            else
            {
                Console.WriteLine("Battery validation failed. Not enough battery capacity for the transfer.");
                return false; 
            }
        }
    }
}