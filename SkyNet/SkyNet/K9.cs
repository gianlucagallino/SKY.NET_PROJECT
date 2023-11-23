using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    class K9 : MechanicalOperator
    {
      
        private string sensorType;
        private string movility;

        public string SensorType { get; set; }
        public string Movility { get; set; }
      
        public K9(/*string sensorType, string Movility, */string id, double maxLoad, Battery battery, Location location, string status) 
            : base(maxLoad, battery, location, status, id)
        {
            //this.sensorType = string.Empty;
            //this.movility = string.Empty;
            maxLoad = 250;
            optimalSpeed = 100;
            battery.MAHCapacity = 6500;
            battery.MaxCharge = 100;
            battery.CurrentCharge = 100;
            battery.Type = 1;
        }
        /*public void Patrol()
        {
        }*/
    }
}

