using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    public class Location
    {
        private int locationId;
        private string locationName;
        //private int[,] locationMatrix;
        private int locationX;
        private int locationY;

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        //public int[,] LocationMatrix { get; set; }

        public int LocationX { get; set; }
        public int LocationY { get; set; }

        public Location()
        {
            LocationId = 0;
            LocationName = string.Empty;
            //LocationMatrix = new int[10, 10];
            LocationX = 0;
            LocationY = 0;
        }



    }
}
