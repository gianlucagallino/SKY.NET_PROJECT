using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SkyNet
{
    internal class Node
    {

        //los ints son necesarios, ya que ciertas funciones en A* toman solamente ints, y no se puede transformar double a int. 
        private int terrainType;
        private bool isDangerous;
        private bool isObstacle;
        private Location nodeLocation;
        private int f;  // Total estimated cost (G + H)
        private int g;  // Cost from the start node to the current node
        private int h;  // Heuristic estimate from the current node to the goal node
        public Node parent;  // Reference to the previous node in the path

        public int TerrainType { set; get; }
        public bool IsDangerous { set; get; }
        public bool IsObstacle { set; get; }
        public Location NodeLocation { set; get; }
        public int F { set; get; }
        public int G { set; get; }
        public int H { set; get; }
        public Node Parent { set; get; }

        


        public Node(int horizontal, int vertical, int type)
        {
            NodeLocation.LocationX = horizontal;
            NodeLocation.LocationY = vertical;
            terrainType = type;
            if (type == 1 || type == 3 )
            {
                IsDangerous = true;
                IsObstacle = true;
            }
            else if (type == 2)
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
         
        //Es necesario agregar una sobrecarga de "interaaccion", con cada celda, correspondiente a su tipo. Posible uso de sobrecarga de metodo. 

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
