using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SkyNet
{
    public class Node
    {
        

        //mejorar descripcion, reducir shitcode
        private static Random rng = new Random();


        //REFERENCIAS DE NOTACION
        //F = Total estimated cost (G + H)
        //G = Cost from the start node to the current node
        //H = Heuristic estimate from the current node to the goal node
        //Parent = Reference to the previous node in the path


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
            int n = rng.Next(0, 4);
            return n;
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

        //Es necesario agregar una sobrecarga de "interaaccion", con cada celda, correspondiente a su tipo. 
        //Posible uso de sobrecarga de metodo. 

        /* Referencias de TerrainType (CONSIDERAR MOVER SISTEMA A ENUM)
         * 0- Terreno Neutro (baldio, planicie, bosque, sector urbano)
         * 1- Vertedero
         * 2-Lago
         * 3-Vertedero electronico
         * 4-Sitio de reciclaje (Implementar maximo 5)
         * 5-Cuartel general(maximo 3)
         */
    }
}


