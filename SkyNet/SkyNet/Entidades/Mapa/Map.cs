using SkyNet.Entidades.Operadores;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        [JsonPropertyName("Grid")]
        public List<Node> Nodes { get; set; }
        [JsonPropertyName("Node")]
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
        public Map(int mapSize, int m8Counter, int k9Counter, int uAVCounter, int sizeOffset, List<HeadQuarters> hQList, int recyclingCounter, Node[,] nodes)
        {
            MapSize = mapSize;
            M8Counter = m8Counter;
            K9Counter = k9Counter;
            UAVCounter = uAVCounter;
            SizeOffset = sizeOffset;
            HQList = hQList;
            RecyclingCounter = recyclingCounter;

            Grid = (nodes != null) ? nodes : new Node[mapSize, mapSize];

            if (nodes == null)
            {
                FillGrid();
                AddLimitedTerrainTypes();
            }
            else
            {
                UpdateNodeParentReferences();
            }
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

        // Singleton instance
        private static Map _instance;

        //Singleton Get method
        public static Map GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Map();
            }
            return _instance;
        }


        //Map size input method
        private static int AskForMapSize()
        {
            // Calculate the center of the console window for display
            int XCenter = Console.WindowWidth / 4;
            int YCenter = Console.WindowHeight / 3;

            int tempNum = 0;
            bool isValidInput = false;

            // Keep asking for input until a valid map size is entered
            while (!isValidInput)
            {
                Console.SetCursorPosition(XCenter, YCenter);
                Console.Write("Please, enter your desired map size between 30-100 (Recommended: 30): ");

                // Check if the input is a valid integer
                if (int.TryParse(Console.ReadLine(), out tempNum))
                {
                    // Check if the entered map size is within the valid range
                    if (tempNum >= 10 && tempNum <= 100)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        // Invalid input range, display error message
                        DisplayErrorMessage(XCenter, YCenter, "Invalid input. Map size must be between 30 and 100 (inclusive). Try again.");
                    }
                }
                else
                {
                    // Invalid integer input, display error message
                    DisplayErrorMessage(XCenter, YCenter, "Invalid input. Please enter a valid integer. Try again.");
                }
            }

            // Clear the console and return the valid map size
            Console.Clear();
            return tempNum;
        }

        // Function to fill the grid with nodes and add limited terrain types
        private void FillGrid()
        {
            for (int row = 0; row < MapSize; row++)
            {
                for (int col = 0; col < MapSize; col++)
                {
                    // Check if the cell is empty (null) and create a new node if so
                    if (Grid[row, col] == null)
                    {
                        Grid[row, col] = new Node(row, col);
                    }
                }
            }
            // Add limited terrain types after filling the grid with nodes
            AddLimitedTerrainTypes();
        }

        // Function to add limited terrain types to the grid
        private void AddLimitedTerrainTypes()
        {
            Random rng = new Random();

            // Set random counts for limited terrain types
            HeadquarterCounter = rng.Next(1, 4);
            RecyclingCounter = rng.Next(1, 6);

            // List to store nodes with limited terrains to avoid overwriting
            List<Node> pickedNodes = new List<Node>();

            LoopTerrainSelection(pickedNodes, HeadquarterCounter, RecyclingCounter);
        }

        public void LoopTerrainSelection(List<Node> list, double HQC, double RC)
        {
            Random rng = new Random();

            // Loop for selecting HQ (HeadQuarters) nodes
            for (int i = 0; i < HQC; i++)
            {
                SelectTerrainNode(list, rng, 5);
                HQList.Add(new HeadQuarters(list.Last().NodeLocation.LocationX, list.Last().NodeLocation.LocationY));
            }

            // Loop for selecting RC (Resource Centers) nodes
            for (int i = 0; i < RC; i++)
            {
                SelectTerrainNode(list, rng, 4);
            }
        }

        // Method to select a terrain node and update the list
        private void SelectTerrainNode(List<Node> list, Random rng, int terrainType)
        {
            bool inLoop = true;

            while (inLoop)
            {
                int randomX = rng.Next(0, MapSize);
                int randomY = rng.Next(0, MapSize);

                // Check if the selected node is not already in the list
                if (!list.Contains(Grid[randomX, randomY]))
                {
                    Grid[randomX, randomY].TerrainType = terrainType;
                    list.Add(Grid[randomX, randomY]);  // Add the selected node to the list
                    inLoop = false;
                }
            }
        }

        // Function to print the map to the console
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
                    int consoleX = Math.Min(Grid[i, j].NodeLocation.LocationX + 2, Console.BufferWidth - 1);
                    int consoleY = Math.Min(Grid[i, j].NodeLocation.LocationY + 1, Console.BufferHeight - 1);

                    Console.SetCursorPosition(consoleX + modifier, consoleY);
                    ColorMapper colors = new ColorMapper();
                    Console.BackgroundColor = colors.GetColorForTerrainType(Grid[i, j].TerrainType);

                    string unitInNode = EvaluateUnitInNode(Grid[i, j]);
                    Console.Write(unitInNode);
                }
                modifier++;
            }

            Console.BackgroundColor = ConsoleColor.Black; // Reset to black to avoid problems
        }

        // Helper method to evaluate the unit in a node and return a string representation
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
                switch (input.OperatorsInNode[0].GetType().Name)
                {
                    case "M8":
                        printType = " M";
                        break;
                    case "K9":
                        printType = " K";
                        break;
                    default:
                        printType = " U";
                        break;
                }
            }

            return printType;
        }

        // Helper method to print column indicators
        private void PrintColumnIndicators()
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("  ");

            for (int i = 0; i < MapSize; i++)
            {
                Console.BackgroundColor = (i % 2 == 0) ? ConsoleColor.DarkRed : ConsoleColor.Red;
                Console.Write((i < 10) ? $"{i} " : $"{i}");
                Console.BackgroundColor = ConsoleColor.Black; // Reset background color for visual clarity
            }

            Console.Write("\n");
        }

        // Helper method to print line indicators
        private void PrintLineIndicators()
        {
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < MapSize; i++)
            {
                Console.BackgroundColor = (i % 2 == 0) ? ConsoleColor.DarkRed : ConsoleColor.Red;
                Console.WriteLine((i < 10) ? $"{i} " : $"{i}");
                Console.BackgroundColor = ConsoleColor.Black; // Reset background color for visual clarity
            }
        }

        //esto permitiria serializar el mapa 
        public string SerializeToJson()
        {
            MapSerializationModel serializationModel = new MapSerializationModel()
            {
                MapSize = MapSize,
                M8Counter = M8Counter,
                K9Counter = K9Counter,
                UAVCounter = UAVCounter,
                SizeOffset = SizeOffset,
                HQList = HQList,
                RecyclingCounter = RecyclingCounter,
                Grid = SerializeNodesToList(Grid)
            };

            return JsonSerializer.Serialize(serializationModel, new JsonSerializerOptions { WriteIndented = true });
        }

        private List<Node> SerializeNodesToList(Node[,] grid)
        {
            List<Node> nodesList = new List<Node>();

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Node currentNode = grid[i, j];
                    nodesList.Add(new Node
                    {
                        TerrainType = currentNode.TerrainType,
                        IsDangerous = currentNode.IsDangerous,
                        NodeLocation = currentNode.NodeLocation,
                        F = currentNode.F,
                        G = currentNode.G,
                        H = currentNode.H,
                        Parent = (currentNode.Parent != null) ? new Node() { NodeLocation = currentNode.Parent.NodeLocation } : null,
                        OperatorsInNode = currentNode.OperatorsInNode
                    });
                }
            }

            return nodesList;
        }

        public static Node[,] SerializeNodes(List<Node> nodes, int mapSize)
        {
            Node[,] serializedNodes = new Node[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    int index = i * mapSize + j;
                    serializedNodes[i, j] = nodes.Count > index ? nodes[index] : new Node();
                }
            }

            return serializedNodes;
        }

        public static Map BuildMapFromJson(string json)
        {
            Console.WriteLine("Starting BuildMapFromJson method...");

            try
            {
                MapSerializationModel serializationModel = JsonSerializer.Deserialize<MapSerializationModel>(json);

                Console.WriteLine("Serialization model deserialized successfully.");

                Map map = new Map(
                    serializationModel.MapSize,
                    serializationModel.M8Counter,
                    serializationModel.K9Counter,
                    serializationModel.UAVCounter,
                    serializationModel.SizeOffset,
                    serializationModel.HQList,
                    (int)serializationModel.RecyclingCounter,
                    SerializeNodes(serializationModel.Grid, serializationModel.MapSize)
                );

                Console.WriteLine("Map object created successfully.");

                Map.MapSize = serializationModel.MapSize;
                Map.M8Counter = serializationModel.M8Counter;
                Map.K9Counter = serializationModel.K9Counter;
                Map.UAVCounter = serializationModel.UAVCounter;
                map.SizeOffset = serializationModel.SizeOffset;
                map.HQList = serializationModel.HQList.Select(hq =>
                    new HeadQuarters(
                        hq.Operators,
                        hq.LocationHeadQuarters
                    )).ToList() ?? new List<HeadQuarters>();
                map.RecyclingCounter = (int)serializationModel.RecyclingCounter;

                Console.WriteLine("Map properties set successfully.");

                return map;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BuildMapFromJson method: {ex.Message}");
                return null;
            }
            finally
            {
                Console.WriteLine("BuildMapFromJson method finished.");
            }
        }


        public List<MechanicalOperator> GetAllOperators()
        {
            List<MechanicalOperator> allOperators = new List<MechanicalOperator>();

            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    allOperators.AddRange(Grid[i, j].OperatorsInNode);
                }
            }

            return allOperators;
        }


        // Helper method to display error messages
        private static void DisplayErrorMessage(int xPosition, int yPosition, string message)
        {
            Console.Clear();
            Console.SetCursorPosition(xPosition, yPosition);
            Console.WriteLine(message);
            Thread.Sleep(1500);
            Console.Clear();
        }
        private static int GetIndex(int x, int y)
        {
            return x * MapSize + y;
        }
        
        private static Node[,] DeserializeNodes(List<Node> nodes, int mapSize)
        {
            Node[,] grid = new Node[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    int index = i * mapSize + j;
                    grid[i, j] = nodes.Count > index ? nodes[index] : new Node();  // Maneja posibles elementos faltantes
                }
            }

            return grid;
        }
        private void UpdateNodeParentReferences()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    Node currentNode = Grid[i, j];

                    if (currentNode.Parent != null)
                    {
                        int parentX = currentNode.Parent.NodeLocation.LocationX;
                        int parentY = currentNode.Parent.NodeLocation.LocationY;
                        currentNode.Parent = Grid[parentX, parentY];
                    }
                }
            }
        }
    }
}





