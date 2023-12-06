using System.Text.Json;
using System.Text.Json.Serialization;


namespace SkyNet.Entidades.Mapa
{
    public class Map
    {

        /* Referencias de TerrainType (CONSIDERAR MOVER SISTEMA A ENUM)
         * 0- Terreno Neutro (baldio, planicie, bosque, sector urbano)
         * 1- Vertedero
         * 2-Lago
         * 3-Vertedero electronico
         * 4-Sitio de reciclaje (Implementar maximo 5)
         * 5-Cuartel general(maximo 3)
         */


        public static Node[,] Grid { get; set; }
        public int HeadquarterCounter { get; set; }
        [JsonPropertyName("HQList")]
        public List<HeadQuarters> HQList { get; set; }
        public int RecyclingCounter { get; set; }
        [JsonPropertyName("MapSize")]
        public static int MapSize { get; set; }
        [JsonPropertyName("M8Counter")]
        public static int M8Counter { get; set; }
        [JsonPropertyName("K9Counter")]
        public static int K9Counter { get; set; }
        [JsonPropertyName("UAVCounter")]
        public static int UAVCounter { get; set; }

        [JsonPropertyName("SizeOffset")]
        public int SizeOffset { get; set; }

        [JsonConstructor]
        public Map(int mapSize, int m8Counter, int k9Counter, int uAVCounter, int sizeOffset, List<HeadQuarters> hQList, int recyclingCounter)
        {

            MapSize = mapSize;
            M8Counter = m8Counter;
            K9Counter = k9Counter;
            UAVCounter = uAVCounter;
            SizeOffset = sizeOffset;
            HQList = hQList;
            RecyclingCounter = recyclingCounter;
            Grid = new Node[MapSize, MapSize];
            FillGrid();
        }

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
                Console.Write("Please, enter your desired map size between 30-100 (Recommended: 30): ");


                if (int.TryParse(Console.ReadLine(), out tempNum)) //"out" makes sure the variable stores the number thats input. Necessary for TryParse.
                {
                    if (tempNum >= 10 && tempNum <= 100)
                    {
                        // Valid input, set the flag to true to exit the loop
                        isValidInput = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.SetCursorPosition(XCenter, YCenter);
                        Console.WriteLine("Invalid input. Map size must be between 30 and 100 (inclusive). Try again.");
                        Thread.Sleep(1500);
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Clear();
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

        // Singleton instance
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
            MapSerializationModel serializationModel = new MapSerializationModel
            {
                MapSize = MapSize,
                M8Counter = M8Counter,
                K9Counter = K9Counter,
                UAVCounter = UAVCounter,
                SizeOffset = SizeOffset,
                HQList = HQList,
                RecyclingCounter = RecyclingCounter,
                //  Grid = SerializeNodes(Grid)  // Llamada a un método para serializar los nodos
            };

            return JsonSerializer.Serialize(serializationModel, new JsonSerializerOptions { WriteIndented = true });
        }

        public static Map BuildMapFromJson(string json)
        {
            MapSerializationModel serializationModel = JsonSerializer.Deserialize<MapSerializationModel>(json);

            Map map = new Map(
             serializationModel.MapSize,
             serializationModel.M8Counter,
             serializationModel.K9Counter,
             serializationModel.UAVCounter,
             serializationModel.SizeOffset,
             serializationModel.HQList,
             (int)serializationModel.RecyclingCounter
         );

            Map.MapSize = serializationModel.MapSize;
            // Asigna las propiedades de instancia
            Map.M8Counter = serializationModel.M8Counter;
            Map.K9Counter = serializationModel.K9Counter;
            Map.UAVCounter = serializationModel.UAVCounter;
            map.SizeOffset = serializationModel.SizeOffset;
            // Asigna las propiedades de instancia
            map.HQList = serializationModel.HQList;
            map.RecyclingCounter = (int)serializationModel.RecyclingCounter;
            

            return map;
        }



    }
}





