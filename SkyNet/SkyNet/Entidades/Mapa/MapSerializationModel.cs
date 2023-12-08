
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
        public MapSerializationModel(int mapSize, int m8Counter, int k9Counter, int uAVCounter, int sizeOffset, List<HeadQuarters> hQList, 
            int recyclingCounter)
        {
            MapSize = mapSize;
            M8Counter = m8Counter;
            K9Counter = k9Counter;
            UAVCounter = uAVCounter;
            SizeOffset = sizeOffset;
            HQList = hQList;
            RecyclingCounter =recyclingCounter;
            //Grid = grid;
        }

    }
}
