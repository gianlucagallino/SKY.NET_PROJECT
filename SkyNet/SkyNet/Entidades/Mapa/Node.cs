using SkyNet.Entidades.Operadores;

namespace SkyNet.Entidades.Mapa
{
    public class Node
    {

        private static Random rng = new Random();


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

        public int TerrainType { get; set; }
        public bool IsDangerous { get; set; }
        public bool IsObstacle { get; set; }
        public Location NodeLocation { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public Node Parent { get; set; }
        public List<MechanicalOperator> OperatorsInNode { get; set; }

        public Node(int horizontal, int vertical)
        {
            NodeLocation = new Location(horizontal, vertical);
            OperatorsInNode = new List<MechanicalOperator>();
            TerrainType = SetNonLimitedTerrainType();
            TerrainTypeMethod();
        }

        public void TerrainTypeMethod()
        {
            if (TerrainType == 1 || TerrainType == 3)
            {
                IsDangerous = true;
                IsObstacle = true;
            }
            else if (TerrainType == 2)
            {
                IsDangerous = false;
                IsObstacle = false;
            }
            else
            {
                IsDangerous = false;
                IsObstacle = true;
            }
        }

        public int SetNonLimitedTerrainType()
        {
            int n = rng.Next(0, 100); // Adjust the range based on bias
            
            if (n < 75)  // 73% chance of getting 0
            {
                return 0;
            }
            else if (n < 87)  // 12% chance of getting 1
            {
                return 1;
            }
            else  // 13% chance of getting 2
            {
                return 2;
            }
        }

        public int SetHeadquarterTerrainType()
        {
            int n = 5;
            return n;
        }

        public int SetRecyclingTerrainType()
        {
            int n = 4;
            return n;
        }

    }
}


