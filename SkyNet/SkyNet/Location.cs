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
        private int currentLocationX;
        private int currentLocationY;

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        //public int[,] LocationMatrix { get; set; }

        public int CurrentLocationX { get; set; }
        public int CurrentLocationY { get; set; }

        public Location()
        {
            LocationId = 0;
            LocationName = string.Empty;
            //LocationMatrix = new int[10, 10];
            CurrentLocationX = 0;
            CurrentLocationY = 0;
        }



    }
}
