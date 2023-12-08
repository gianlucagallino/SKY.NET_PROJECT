using System.Text.Json.Serialization;

namespace SkyNet.Entidades
{
    public class Location
    {
        private int locationX;
        private int locationY;

        public int LocationX { get; set; }
        public int LocationY { get; set; }

        [JsonConstructor]
        public Location()
        {
            LocationX = 0;
            LocationY = 0;
        }
        public Location(int hor, int vert)
        {
            LocationX = hor;
            LocationY = vert;
        }

        public override string ToString()
        {
            return LocationX.ToString() + LocationY.ToString();
        }
    }
}