using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Map
    {

        //Esto, idealmente, deberia ser un singleton. No realmente, por el tema de tener varias partidas. Capaz se puede reutilizar el mismo mapa. 
        //Igualmente, opino que no deberia ser infinitamente instanciable, asi que hay que pensar algo. 

        private Node[,] grid;
        private double headquarterCounter;
        private double recyclingCounter;

        public Node[,] Grid { get; set; }
        public double HeadquarterCounter { get; set; }
        public double RecyclingCounter { get; set; }

        Map()
        {
            Grid = new Node[100, 100];
            HeadquarterCounter = 0;
            RecyclingCounter = 0;
        }
        public void CreateMapDistribution(Node[,] wholegrid)
        {
            

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    // Initialize each node in the grid
                    int type = SetRandomTerrainType();
                    wholegrid[i, j] = new Node(i, j, type);
                }
            }
        }
        public int SetRandomTerrainType()
        {
            bool repeatingFlag = false;
            Random rng = new Random();
            int n = rng.Next(0, 5);
            repeatingFlag = true;
            while (repeatingFlag == true)
            {
                if (n == 4)
                {
                    if (RecyclingCounter >= 5)
                    {
                        n = rng.Next(0, 5);
                    }
                    else
                    {
                        RecyclingCounter++;
                        repeatingFlag = false;
                    }

                }
                else if (n == 5)
                {
                    if (HeadquarterCounter >= 5)
                    {
                        n = rng.Next(0, 5);
                    }
                    else
                    {
                        HeadquarterCounter++;
                        repeatingFlag = false;
                    }
                }
                else repeatingFlag = false;
            }

            return n;
        }

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
