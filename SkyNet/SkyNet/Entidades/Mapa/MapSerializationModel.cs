
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkyNet.Entidades.Mapa

{
    [Serializable]
    public class MapSerializationModel
    {
        [JsonPropertyName("MapSize")]
        public int MapSize { get; set; }
        [JsonPropertyName("M8Counter")]
        public int M8Counter { get; set; }
        [JsonPropertyName("K9Counter")]
        public int K9Counter { get; set; }
        [JsonPropertyName("UAVCounter")]
        public int UAVCounter { get; set; }
        [JsonPropertyName("SizeOffset")]
        public int SizeOffset { get; set; }
        [JsonPropertyName("HQList")]
        public List<HeadQuarters> HQList { get; set; }
        [JsonPropertyName("RecyclingCounter")]
        public int RecyclingCounter { get; set; }
        //public NodeSerializationModel[,] Grid { get; set; } 

        public MapSerializationModel()
        {
            
        }
        [JsonConstructor]
        public MapSerializationModel(int MapSize, int M8Counter, int K9Counter, int UAVCounter, int SizeOffset, List<HeadQuarters> HQList, 
            int RecyclingCounter)
        {
            MapSize = MapSize;
            M8Counter = M8Counter;
            K9Counter = K9Counter;
            UAVCounter = UAVCounter;
            SizeOffset = SizeOffset;
            HQList = HQList;
            RecyclingCounter =RecyclingCounter;
            //Grid = grid;
        }

    }
}
