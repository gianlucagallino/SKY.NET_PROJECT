namespace SkyNet.Entidades
{
    public class Location
    {
        private int locationX;
        private int locationY;

        public int LocationX { get; set; }
        public int LocationY { get; set; }

        public Location(int hor, int vert)
        {
            LocationX = hor;
            LocationY = vert;
        }
    }
}