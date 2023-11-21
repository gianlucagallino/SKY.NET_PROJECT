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
        protected double currentBattery;
        protected string status;
        protected float maxLoad;
        protected float currentLoad;
        protected float optimalSpeed;
        protected double currentLocationX;
        protected double currentLocationY;
        //lo cambiaria por
        protected Location location;

        public string Id { get; set; }
        public Battery Battery { get; set; }
        public double CurrentBattery { get; set; }
        public string Status { get; set; }
        public float MaxLoad { get; set; }
        public float CurrentLoad { get; set; }
        public float OptimalSpeed { get; set; }
        public double CurrentLocationX { get; set; }
        public double CurrentLocationY { get; set; }
        public Location LocationP { get; set; }


        //Hay que revisar este constructor. El profe menciono que debian tener valores no vacios
        public MechanicalOperator()
        {
            id= string.Empty;
            battery = new Battery();
            currentBattery = battery.MAHCapacity;
            status = "ACTIVE";
            maxLoad = 1000;
            currentLoad = 0;
            optimalSpeed = 100;
            currentLocationX = HeadQuarters.GetInstance().LocationX;
            currentLocationY = HeadQuarters.GetInstance().LocationY;
            LocationP = new Location();
        }
        public double CalculateMovementSpeed()
        {
            double batteryPercentageSpent = 100 - ((currentBattery / battery.MAHCapacity) * 100);
            double slownessMultiplier = batteryPercentageSpent % 10; //this line calculates how many times to apply the speed debuff
            double finalSpeed = optimalSpeed - ((optimalSpeed / 10) * slownessMultiplier);
            return finalSpeed;
        }
        public void MoveTo(double x, double y)//esto deberia ser int, si me autorizan cambio todo esto
        {
            long movX = 0;
            long movY = 0;
            //Se asigna que tipo de movimiento debe ser realizado para llegar a la cuadrilla que corresponde. 
            if (CurrentLocationX < x)
            {
                movX = 1;
            }
            else if (CurrentLocationX > x)
            {
                movX = -1;
            }

            if (CurrentLocationY < y)
            {
                movY = 1;
            }
            else if (CurrentLocationY > y)
            {
                movY = -1;
            }

            //se desplaza la posicion actual a la posicion buscada 

            while (CurrentLocationY != y)
            {
                CurrentLocationY += movY;
                /*
                InteractuarConPosicion() 

                Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                 que dependiendo del tipo de terreno tiene diferentes efectos
                */
            }
            while (CurrentLocationX != x)
            {
                CurrentLocationX += movX;
                /*
                InteractuarConPosicion() 

                Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                 que dependiendo del tipo de terreno tiene diferentes efectos
                */
            }
        }

        //Los transfer me dejan en duda del tipo que deberian ser, considerando que para los k9 se le deberia meter un k9, un m8 debe ingresar un m8 y asi. Lo dejo comentado por ahora. 
        //Este seria un buen uso de genericos?
        //ademas, no haria las verificaciones aca, pero si en el metodo que llama los transfer. 
        /*
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
            CurrentLocationX = HeadQuarters.GetInstance().LocationX;
            CurrentLocationY = HeadQuarters.GetInstance().LocationY;
            CurrentLoad=0;
        }

        public void ReturnToHQandChargeBattery()
        {
            CurrentLocationX=HeadQuarters.GetInstance().LocationX;
            CurrentLocationY = HeadQuarters.GetInstance().LocationY;
            CurrentLoad=battery.MAHCapacity;
        }
    }
}
