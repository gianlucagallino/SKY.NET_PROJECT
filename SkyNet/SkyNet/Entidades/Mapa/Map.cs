using System.Text.Json;
using System.Transactions;


namespace SkyNet.Entidades.Mapa
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


        public static Node[,] Grid { get; set; }
        public double HeadquarterCounter { get; set; }
        public List<HeadQuarters> HQList { get; set; }
        public double RecyclingCounter { get; set; }
        public static int MapSize { get; set; }
        public static int M8Counter { get; set; }
        public static int K9Counter { get; set; }
        public static int UAVCounter { get; set; }
        public int SizeOffset { get; set; }



        private Map()
        {
            M8Counter = 0;
            K9Counter = 0;
            UAVCounter = 0;
            MapSize = AskForMapSize();
            SizeOffset = MapSize.ToString().Length;
            Grid = new Node[MapSize, MapSize];
            HeadquarterCounter = 0;
            RecyclingCounter = 0;
            HQList = new List<HeadQuarters>();
            FillGrid();
            
        }

        private int AskForMapSize()
        {
            int XCenter = Console.WindowWidth / 4;
            int YCenter = Console.WindowHeight / 3;
            int tempNum = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.SetCursorPosition(XCenter, YCenter);
                Console.Write("Please, enter your desired map size between 30-100 (Recommended: 50): ");


                if (int.TryParse(Console.ReadLine(), out tempNum)) //"out" makes sure the variable stores the number thats input. Neccesary for Tryparse.
                {
                    if (tempNum >= 30 && tempNum <= 100)
                    {
                        // Valid input, set the flag to true to exit the loop
                        isValidInput = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(XCenter, YCenter);
                        Console.WriteLine("Invalid input. Map size must be between 30 and 100 (inclusive). Try again.");
                        Thread.Sleep(1500);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.SetCursorPosition(XCenter, YCenter);
                    Console.WriteLine("Invalid input. Please enter a valid integer. Try again.");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }
            Console.Clear();
            return tempNum;
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
                        HQList.Add(new HeadQuarters(RandomX, RandomY));
                        list.Add(Grid[RandomX, RandomY]);  // Add the selected node to the list
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
                        list.Add(Grid[RandomX, RandomY]);  // Add the selected node to the list
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
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < MapSize; i++)
            {


                for (int j = 0; j < MapSize; j++)
                {
                    // Initialize each node in the grid
                    int consoleX = Math.Min(Grid[i, j].NodeLocation.LocationX + 2, Console.BufferWidth - 1);
                    int consoleY = Math.Min(Grid[i, j].NodeLocation.LocationY + 1, Console.BufferHeight - 1);


                    Console.SetCursorPosition(consoleX + modifier, consoleY);
                    Console.BackgroundColor = ReadPositionColor(Grid[i, j]);
                    string unitInNode = EvaluateUnitInNode(Grid[i, j]);
                    Console.Write(unitInNode);

                }
                modifier++;
            }
            Console.BackgroundColor = ConsoleColor.Black; // vuelve al negro, para que no se quede "pegado" el ultimo color.
        }

        private string EvaluateUnitInNode(Node input)
        {
            string printType;
            int amount = input.OperatorsInNode.Count();

            if (amount == 0)
            {
                printType = "  ";
            }
            else if (input.OperatorsInNode.Count > 1)
            {
                printType = " @";
            }
            else
            {
                if (input.OperatorsInNode[0].GetType().Name == "M8")
                {
                    printType = " M";
                }
                else if (input.OperatorsInNode[0].GetType().Name == "K9")
                {
                    printType = " K";
                }
                else
                {
                    printType = " U";
                }
            }

            return printType;
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
                Console.BackgroundColor = ConsoleColor.Black;//necesario por prolijidad visual
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


        //Aca podria ir un switch, la verdad. pero fuck switch statements, all my homies hate switch statements
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

        //esto permitiria serializar el mapa 
        public string SerializeToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
        public static Map DeserializeFromJson(string jsonString)
        {
            return JsonSerializer.Deserialize<Map>(jsonString);
        }
    }
}





