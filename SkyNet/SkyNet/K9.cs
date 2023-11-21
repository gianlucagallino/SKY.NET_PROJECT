using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    class K9 : MechanicalOperator
    {
        public K9() : base()
        {
            this.sensorType = string.Empty;
            this.movility = string.Empty;
        }
        private string sensorType;
        private string movility;

        public string SensorType { get; set; }
        public string Movility { get; set; }
        public void Patrol()
        {
        }
    }
}

