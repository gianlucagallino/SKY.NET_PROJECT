namespace SkyNet.Entidades
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

        public Location(int hor, int vert)
        {
            LocationId = 0;
            LocationName = string.Empty;
            //LocationMatrix = new int[10, 10];
            LocationX = hor;
            LocationY = vert;
        }
    }
}