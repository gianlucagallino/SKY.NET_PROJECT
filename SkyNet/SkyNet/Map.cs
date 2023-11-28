using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

        //Ah, y reducir hardcodeo + abstraer patrones repetitivos a funciones. 

        private Node[,] grid;
        private int mapSize;
        private double headquarterCounter;
        private double recyclingCounter;

        public Node[,] Grid { get { return grid; } set { grid = value; } }
        public double HeadquarterCounter { get; set; }
        public double RecyclingCounter { get; set; }
        public int MapSize { get; set; }

        public int SizeOffset { get; set; }



        private Map()
        {
            MapSize = 10; //ver grabacion, arreglar. 
            SizeOffset = MapSize.ToString().Length;
            Grid = new Node[MapSize, MapSize];
            HeadquarterCounter = 0;
            RecyclingCounter = 0;
            FillGrid();
        
        }
        private void FillGrid()
        {
            
            for ( int j=0; j < MapSize; j++)
            {
                for (int k = 0; k < MapSize; k++)
                {
                    if (Grid[j, k] == null)
                    {
                        Grid[j, k] = new Node(j, k);
                    }
                }
                
            }
        }


        private static Map _instance;
        public static Map GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Map();
            }
            return _instance;
        }

        public void PrintMap ()
        {

            PrintColumnIndicators();
            PrintLineIndicators();
            for (int i = 0; i < MapSize; i++)
            {
                
                for (int j = 0; j < MapSize; j++)
                {
                    // Initialize each node in the grid
                    int consoleX = Math.Min(Grid[i, j].NodeLocation.LocationX+3, Console.BufferWidth - 1);
                    int consoleY = Math.Min(Grid[i, j].NodeLocation.LocationY+3, Console.BufferHeight - 1);

                    Console.SetCursorPosition(consoleX, consoleY);
                    Console.BackgroundColor = ReadPositionColor(Grid[i, j]);
                    Console.Write(" ");
                }
            }
            Console.BackgroundColor = ConsoleColor.Black; // vuelve al negro, para que no se quede "pegado" el ultimo color.

            Console.WriteLine("ignoren el offset del mapa, la falta de menu y el area de 10x10, estaba a medio test cuando empezo la clase. Preferible que vean el codigo!");
        }

        private void PrintColumnIndicators()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("  ");

            for (int i = 0; i < MapSize; i++)
            {
                if (i % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                else { Console.BackgroundColor = ConsoleColor.Red; }
                if (i < 10)
                {
                    Console.Write(i + " ");
                }
                else Console.Write(i);
            }
            Console.Write("\n");
        }
        private void PrintLineIndicators()
        {
            Console.ForegroundColor = ConsoleColor.White;
            
            for (int i = 0; i < MapSize; i++)
            {
                if (i % 2 == 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }
                else { Console.BackgroundColor = ConsoleColor.Red; }
                if (i < 10)
                {
                    Console.WriteLine(i + " ");
                }
                else Console.WriteLine(i);


            }
        }


        //Esta funcion deberia ser optimizada. es un desastre.
        private ConsoleColor ReadPositionColor(Node input)
        {
            int type = input.TerrainType;
            if (type == 0)
            {
                return ConsoleColor.DarkBlue;
            }
            else if (type == 1)
            {
                return ConsoleColor.DarkGreen;
            }
            else if (type == 2)
            {
                return ConsoleColor.Cyan;
            }
            else if (type == 3)
            {
                return ConsoleColor.Yellow;
            }
            else if (type == 4)
            {
                return ConsoleColor.Gray;
            }
            else if (type == 5)
            {
                return ConsoleColor.Green;
            }
            else return ConsoleColor.Black;
        }
        
        
        /* code assorted
         * 
         * 
         * public static void WriteAt(string s, int x, int y, int origCol=0, int origRow=0)
{

    try
    {
        Console.SetCursorPosition(origCol + x, origRow + y);
        Console.Write(s);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.Clear();
        Console.WriteLine(e.Message);
    }
}


pero antes de llamarlo le cambio el color a la letra y al fondo de la consola, si pones los dos del mismo color pintas un espacio de ese color, si pones colores diferentes podes ver la letra q pones


Console.BackgroundColor = ConsoleColor.Blue;
Console.ForegroundColor = ConsoleColor.Blue;


ConsoleHelper.WriteAt("@", (i - coord[0]) * 2 + 2, j - coord[1] + 2);
tambien, en el WriteAt, el origCol y origRow no recomiendo usarlos porq es a partir de donde queres dibujar, si lo dejas en 0, dibuja desde el primer espacio hacia abajo y es más facil guiarse
         * 
         * 
         * 
         * 
         * namespace SkyNet.Entidades.Mundiales
{

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
