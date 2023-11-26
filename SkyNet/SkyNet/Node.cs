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
        private static Random rng = new Random();

        private int terrainType;
        private bool isDangerous;
        private bool isObstacle;
        private Location nodeLocation;
        private int f;  // Total estimated cost (G + H)
        private int g;  // Cost from the start node to the current node
        private int h;  // Heuristic estimate from the current node to the goal node
        public Node parent;  // Reference to the previous node in the path
        private int type;

        public int TerrainType { get; set; }
        public bool IsDangerous { get; set; }
        public bool IsObstacle { get; set; }
        public Location NodeLocation { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public Node Parent { get; set; }

        public Node(int horizontal, int vertical)
        {
            NodeLocation = new Location(horizontal, vertical);
            TerrainType = SetRandomTerrainType();
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

        public Node(int horizontal, int vertical, int type) : this(horizontal, vertical)
        {
            this.type = type;
        }

        public int SetRandomTerrainType()
        {
            bool repeatingFlag = false;
            int n = rng.Next(0, 6); // Updated to include 6 as well

            repeatingFlag = true;
            while (repeatingFlag)
            {
                if (n == 4)
                {
                    if (Map.GetInstance().RecyclingCounter >= 5) // Assuming RecyclingCounter is a property of the Map class
                    {
                        n = rng.Next(0, 6);
                    }
                    else
                    {
                        Map.GetInstance().RecyclingCounter++;
                        repeatingFlag = false;
                    }
                }
                else if (n == 5)
                {
                    if (Map.GetInstance().HeadquarterCounter >= 3) // Assuming HeadquarterCounter is a property of the Map class
                    {
                        n = rng.Next(0, 6);
                    }
                    else
                    {
                        Map.GetInstance().HeadquarterCounter++;
                        repeatingFlag = false;
                    }
                }
                else
                {
                    repeatingFlag = false;
                }
            }

            return n;
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

