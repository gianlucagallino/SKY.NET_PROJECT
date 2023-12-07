namespace SkyNet.Entidades.Mapa
{
    public class MapSerializationModel
    {
        public int MapSize { get; set; }
        public int M8Counter { get; set; }
        public int K9Counter { get; set; }
        public int UAVCounter { get; set; }
        public int SizeOffset { get; set; }
        public List<HeadQuarters> HQList { get; set; }
        public double RecyclingCounter { get; set; }
        //public Node[,] Grid { get; set; }
    }
}
