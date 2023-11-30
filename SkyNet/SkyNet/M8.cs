using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    class M8 : MechanicalOperator
    {

        private float loadCapacity;
        private float appendageType;

        public float LoadCapacity { get; set; }
        public float AppendageType { get; set; }

        public M8(/*float loadCapacity, float appendageType,*/ string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status) : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            //this.loadCapacity = 0;
            //this.appendageType = 0;
            maxLoad = 40;
            maxLoadOriginal = 40;
            optimalSpeed = 250;
            battery.MAHCapacity = 12250;
            battery.CurrentChargePercentage = 100;
            battery.Type = 2;
        }

        public  M8(int xposition, int yposition) : base(xposition, yposition)
        {
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition; 
        }

        /* public void Reconnoiter()
         {

         }*/
    }
}