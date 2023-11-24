using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    class UAV : MechanicalOperator
    {
        public UAV(/*float flightHeight, int bladeAmount, bool integratedCamera,*/ string id, double maxLoad, Battery battery, Location location, string status) : base(maxLoad, battery, location, status, id)
        {
            //flightHeight = 0;
            //bladeAmount = 0;
            //integratedCamera = true;
            maxLoad = 5;
            optimalSpeed = 150;
            battery.MAHCapacity = 4000;
            battery.CurrentChargePercentage = 100;
            battery.Type = 3;
        }

        private float flightHeight;
        private int bladeAmount;
        private bool integratedCamera;

        public float FlightHeight { get; set; }
        public int BladeAmount { get; set; }
        public bool IntegratedCamera { get; set; }
        /* public void Fly()
         {

         }*/
    }
}
