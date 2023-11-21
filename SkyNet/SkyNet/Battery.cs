using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    public class Battery
    {
        private int mAhCapacity;
        private string type;

        public int MAHCapacity { get; set; }
        public string Type { get; set; }

        public Battery()
        {
            this.mAhCapacity = 0;
            this.type = string.Empty;
        }
    }
}
