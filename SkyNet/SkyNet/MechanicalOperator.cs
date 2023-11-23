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
            id= string.Empty;
            battery = new Battery();
            //battery.CurrentCharge = battery.MAHCapacity;
            status = "ACTIVE";
            maxLoad = 1000;
            currentLoad = 0;
            optimalSpeed = 100;
            LocationP = new Location();
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
            double x = loc.CurrentLocationX;
            double y = loc.CurrentLocationY;
            int movX = 0;
            int movY = 0;
            //Se asigna que tipo de movimiento debe ser realizado para llegar a la cuadrilla que corresponde. 
            if (LocationP.CurrentLocationX < x)
            {
                movX = 1;
            }
            else if (LocationP.CurrentLocationX > x)
            {
                movX = -1;
            }

            if (LocationP.CurrentLocationY < y)
            {
                movY = 1;
            }
            else if (LocationP.CurrentLocationY > y)
            {
                movY = -1;
            }

            //se desplaza la posicion actual a la posicion buscada 

            while (LocationP.CurrentLocationY != y)
            {
                LocationP.CurrentLocationY += movY;
                /*
                InteractuarConPosicion() 

                Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                 que dependiendo del tipo de terreno tiene diferentes efectos
                */
            }
            while (LocationP.CurrentLocationX != x)
            {
                LocationP.CurrentLocationX += movX;
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
                //compara tipos de bateria PREGUNTAR SI ES LA MEJOR MANERA HACIENDOLO DESDE TIPO O CON DATO CRUDO
                if (destination.battery.Type == battery.Type)
                {
                    destination.battery.ChargeBattery(amountPercentage);
                    battery.DecreaseBattery(amountPercentage);
                }
            }
            else
            { // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
                MoveTo(destination.LocationP);

                // Calcula la distancia entre los operadores y disminuye la batería del operador actual.
                double distance = CalculateDistance(destination.LocationP);
                double batteryConsumptionPercentage = 0.05 * (distance / 10);// TODO valores a revisar
                
                if (destination.battery.Type == battery.Type)
                {
                    destination.battery.ChargeBattery(amountPercentage);
                    battery.DecreaseBattery(battery.CurrentChargePercentage * batteryConsumptionPercentage);
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
                double batteryConsumptionPercentage = 0.05 * (distance / 10);//TODO valores a revisar
                

                // Luego, realiza la transferencia de carga.
                if (destination.currentLoad + amountKG <= destination.MaxLoad)
                {
                    destination.currentLoad += amountKG;
                    currentLoad -= amountKG;
                    battery.DecreaseBattery(battery.CurrentChargePercentage * batteryConsumptionPercentage);
                }
                else
                {
                    Console.WriteLine("TransferLoad failed. Destination operator cannot hold that much load.");
                }
            }
        }

        private double CalculateDistance(Location destinationLocation)
        {

            double difCoordX = Math.Abs(LocationP.CurrentLocationX - destinationLocation.CurrentLocationX);
            double difCoordY = Math.Abs(LocationP.CurrentLocationY - destinationLocation.CurrentLocationY);
            double distance = difCoordX + difCoordY;
            
            return  distance ;
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
        */
        public void ReturnToHQandRemoveLoad()
        {
            LocationP.CurrentLocationX = HeadQuarters.GetInstance().LocationHeadQuarters.CurrentLocationX;
            LocationP.CurrentLocationY = HeadQuarters.GetInstance().LocationHeadQuarters.CurrentLocationY;
            CurrentLoad=0;
        }

        public void ReturnToHQandChargeBattery()
        {
            LocationP.CurrentLocationX= HeadQuarters.GetInstance().LocationHeadQuarters.CurrentLocationX;
            LocationP.CurrentLocationY = HeadQuarters.GetInstance().LocationHeadQuarters.CurrentLocationY;
            battery.CompleteBatteryLevel();
        }
    }
}
