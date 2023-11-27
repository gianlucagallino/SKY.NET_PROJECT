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

        //Ah, y reducir hardcodeo + abstraer patrones repetitivos a funciones. 

        private Node[,] grid;
        private double headquarterCounter;
        private double recyclingCounter;

        public Node[,] Grid { get; set; }
        public double HeadquarterCounter { get; set; }
        public double RecyclingCounter { get; set; }


        private Map()
        {
            Grid = new Node[100, 100];
            HeadquarterCounter = 0;
            RecyclingCounter = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    Grid[i, j] = new Node(i, j);
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

        public void PrintMap(Node[,] Grid)
        {

            //PrintColumnIndicators(100);
            for (int i = 0; i < 100; i++)
            {
                //PrintLineIndicator();
                for (int j = 0; j < 100; j++)
                {
                    // Initialize each node in the grid
                    Console.SetCursorPosition(Grid[i, j].NodeLocation.LocationX, Grid[i, j].NodeLocation.LocationY);
                    Console.BackgroundColor = ReadPositionColor(Grid[i, j]);
                    Console.WriteLine(" ");
                }
            }
        }

        //Esta funcion deberia ser optimizada. es un desastre 
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