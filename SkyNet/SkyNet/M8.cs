 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    class M8 : MechanicalOperator
    {
        public M8() {
            this.loadCapacity = 0;
            this.appendageType = 0;
        }
        private float loadCapacity;
        private float appendageType;

        public float LoadCapacity { get; set; }
        public float AppendageType { get; set; }

     

        public void Reconnoiter()
        {

        }
    }
}
