﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
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

        /* Referencias de TerrainType (CONSIDERAR MOVER SISTEMA A ENUM)
         * 0- Terreno Neutro (baldio, planicie, bosque, sector urbano)
         * 1- Vertedero
         * 2-Lago
         * 3-Vertedero electronico
         * 4-Sitio de reciclaje (Implementar maximo 5)
         * 5-Cuartel general(maximo 3)
         */


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
            MapSize = 100; //ver grabacion, arreglar. 
            SizeOffset = MapSize.ToString().Length;
            Grid = new Node[MapSize, MapSize];
            HeadquarterCounter = 0;
            RecyclingCounter = 0;
            FillGrid();

        }
        private void FillGrid()
        {

            for (int j = 0; j < MapSize; j++)
            {
                for (int k = 0; k < MapSize; k++)
                {
                    if (Grid[j, k] == null)
                    {
                        Grid[j, k] = new Node(j, k);
                    }
                }

            }
            AddLimitedTerrainTypes();
        }

        private void AddLimitedTerrainTypes()
        {

            Random rng = new Random();
            HeadquarterCounter = rng.Next(1, 4);
            RecyclingCounter = rng.Next(1, 6);
            List<Node> pickedNodes = new List<Node>();  //Saves the limited terrains in a list, so they dont get overwritten by accident. Low chance of that happening, but makes for a more secure system. 
            LoopTerrainSelection(pickedNodes, HeadquarterCounter, RecyclingCounter);
        }

        private void LoopTerrainSelection(List<Node> list, double HQC, double RC)
        {
            Random rng = new Random();
            for (int i = 0; i < HQC; i++)
            {
                bool inLoop = true;
                while (inLoop)
                {
                    int RandomX = rng.Next(0, MapSize);
                    int RandomY = rng.Next(0, MapSize);
                    if (!list.Contains(Grid[RandomX, RandomY]))
                    {
                        Grid[RandomX, RandomY].TerrainType = 5;
                        list.Add(Grid[RandomX, RandomY]);
                        inLoop = false;
                    }
                }
            }
            for (int i = 0; i < RC; i++)
            {

                bool inLoop = true;
                while (inLoop)
                {
                    int RandomX = rng.Next(0, MapSize);
                    int RandomY = rng.Next(0, MapSize);
                    if (!list.Contains(Grid[RandomX, RandomY]))
                    {
                        Grid[RandomX, RandomY].TerrainType = 4;
                        list.Add(Grid[RandomX, RandomY]);
                        inLoop = false;
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

        public void PrintMap()
        {

            PrintColumnIndicators();
            PrintLineIndicators();
            int modifier = 0;
            for (int i = 0; i < MapSize; i++)
            {
                

                for (int j = 0; j < MapSize; j++)
                {
                    // Initialize each node in the grid
                    int consoleX = Math.Min(Grid[i, j].NodeLocation.LocationX + 2, Console.BufferWidth - 1);
                    int consoleY = Math.Min(Grid[i, j].NodeLocation.LocationY + 1, Console.BufferHeight - 1);
                    

                    Console.SetCursorPosition(consoleX+modifier, consoleY);
                    Console.BackgroundColor = ReadPositionColor(Grid[i, j]);
                    Console.Write("  ");
                    
                }
                modifier++;
            }
            Console.BackgroundColor = ConsoleColor.Black; // vuelve al negro, para que no se quede "pegado" el ultimo color.
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
                Console.BackgroundColor= ConsoleColor.Black;//necesario por prolijidad visual
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
                Console.BackgroundColor = ConsoleColor.Black;//necesario por prolijidad visual

            }
        }


        //Esta funcion deberia ser optimizada. es un desastre.
        private ConsoleColor ReadPositionColor(Node input)
        {
            int type = input.TerrainType;
            if (type == 0)
            {
                return ConsoleColor.White;
            }
            else if (type == 1)
            {
                return ConsoleColor.Green;
            }
            else if (type == 2)
            {
                return ConsoleColor.Blue; 
            }
            else if (type == 3)
            {
                return ConsoleColor.Yellow;
            }
            else if (type == 4)
            {
                return ConsoleColor.DarkYellow;
            }
            else if (type == 5)
            {
                return ConsoleColor.Magenta;
            }
            else return ConsoleColor.White;
        }
    }
}





