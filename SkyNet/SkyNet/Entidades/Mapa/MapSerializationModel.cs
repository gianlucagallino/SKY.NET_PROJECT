
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/*
    La clase MapSerializationModel es un modelo de serializaci�n dise�ado para almacenar informaci�n relevante sobre el estado del juego, 
    especialmente para serializar y deserializar el mapa. Contiene propiedades para almacenar el tama�o del mapa, contadores de diferentes
    tipos de operadores, un offset de tama�o, una lista de cuarteles generales, un contador de reciclaje y una lista de nodos 
    que representan el mapa.
    Se realiza en una clase separada a la de Map, ya que puedes gestionar de manera m�s flexible la serializaci�n y
    deserializaci�n del estado del juego sin depender directamente de la implementaci�n est�tica de la clase Map
 */

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
        

        [JsonPropertyName("Grid")]
        public List<Node> Grid { get; set; }


        public MapSerializationModel()
        {

        }

        [JsonConstructor]
        public MapSerializationModel(int mapSize, int m8Counter, int k9Counter, int uAVCounter, int sizeOffset, List<HeadQuarters> hQList,
             int recyclingCounter, List<Node> grid)
        {
            MapSize = mapSize;
            M8Counter = m8Counter;
            K9Counter = k9Counter;
            UAVCounter = uAVCounter;
            SizeOffset = sizeOffset;
            HQList = hQList;
            RecyclingCounter = recyclingCounter;
            Grid = grid;
        }
    }
}
