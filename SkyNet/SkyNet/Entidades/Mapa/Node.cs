using SkyNet.Entidades.Operadores;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkyNet.Entidades.Mapa
{
    /*
       Node es una clase que representa un nodo en el mapa del juego. Cada nodo tiene información sobre su tipo de terreno, 
       peligrosidad, ubicación, costos estimados para algoritmos de búsqueda y una lista de operadores mecánicos presentes en ese nodo.
       Además, cuenta con métodos para la generación aleatoria del tipo de terreno.

    //REFERENCIAS DE NOTACION
        //F = Total estimated cost (G + H)
        //G = Cost from the start node to the current node
        //H = Heuristic estimate from the current node to the goal node
        //Parent = Reference to the previous node in the path

        /* Referencias de TerrainType
         * 0- Terreno Neutro (baldio, planicie, bosque, sector urbano)
         * 1- Vertedero
         * 2-Lago
         * 3-Vertedero electronico
         * 4-Sitio de reciclaje (Implementar maximo 5)
         * 5-Cuartel general(maximo 3)
      */
         
    [Serializable]
    public class Node
    {

        private static Random rng = new Random();


        

        public int TerrainType { get; set; }
        public bool IsDangerous { get; set; }
        [JsonPropertyName("NodeLocation")]
        public Location NodeLocation { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public Node Parent { get; set; }
        public List<MechanicalOperator> OperatorsInNode { get; set; }

        [JsonConstructor]
        public Node()
        {
                
        }
        public Node(int horizontal, int vertical)
        {
            NodeLocation = new Location(horizontal, vertical);
            OperatorsInNode = new List<MechanicalOperator>();
            TerrainType = SetNonLimitedTerrainType();
            SetDangerousFlag();
        }

        public void SetDangerousFlag()
        {
            IsDangerous = (TerrainType == 1 || TerrainType == 3);
        }

        //This function sets terrain generation odds. It predominantly generates normal terrain (type 0)
        public int SetNonLimitedTerrainType()
        {
            int n = rng.Next(0, 100);

            if (n < 60) return 0;    // 60% chance of getting 0
            if (n < 73) return 1;    // 13% chance of getting 1 
            if (n < 86) return 2;    // 13% chance of getting 2 

            return 3;                // 14% chance of getting 3
        }

    }
}


