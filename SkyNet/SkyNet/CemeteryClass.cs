namespace SkyNet
{
    internal class CemeteryClass
    {

        //This is the cemetery class, meant purely for storing old code and cleaning up the project. Feel free to add fun comments. 

        //Este era el a* anterior. indentado hasta china, pero esta un poco mejor ahora. 

        /*
                  internal class Search
                  {

                      // Heuristic function to calculate Manhattan distance between two nodes
                      static int heuristic(Node a, Node b)
                      {
                          //HEURISTICA DE MANHATTAN: Distancia entre posiciones de X y de Y entre el nodo actual y fin
                          return Math.Abs(a.NodeLocation.LocationX - b.NodeLocation.LocationX) + Math.Abs(a.NodeLocation.LocationY - b.NodeLocation.LocationY);
                      }

                      // Function to check if a node is valid for traversal

                      //esta la usan las walking units, para optimo tomando en cuenta riesgos
                      static bool isValidandDangerless(Node n, Node[,] grid)
                      {
                          // Check if the node is within the boundaries and not dangerous
                          if (n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) && n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1))
                          {
                              if (grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsDangerous == false)
                              {
                                  return true;
                              }
                          }
                          return false;
                      }

                      static bool isValidandWalkable(Node n, Node[,] grid)
                      {
                          // Check if the node is within the boundaries and not an obstacle
                          if (n.NodeLocation.LocationX >= 0 && n.NodeLocation.LocationX < grid.GetLength(0) && n.NodeLocation.LocationY >= 0 && n.NodeLocation.LocationY < grid.GetLength(1))
                          {
                              if (grid[n.NodeLocation.LocationX, n.NodeLocation.LocationY].IsObstacle == false)
                              {
                                  return true;
                              }
                          }
                          return false;
                      }

                      private void ExploreNeighbourNodes(Node currentNode, Node goal, List<Node> openSet, HashSet<Node> closedSet, Node[,] grid)
                      {
                          for (int i = -1; i <= 1; i++)
                          {
                              for (int j = -1; j <= 1; j++)
                              {
                                  if (i == 0 && j == 0) continue; // Skip the current node

                                  if (currentNode.NodeLocation.LocationX + i >= 0 && currentNode.NodeLocation.LocationX + i < grid.GetLength(0) && currentNode.NodeLocation.LocationY + j >= 0 && currentNode.NodeLocation.LocationY + j < grid.GetLength(1))
                                  {
                                      Node neighbour = grid[currentNode.NodeLocation.LocationX + i, currentNode.NodeLocation.LocationY + j];
                                      if (isValidandWalkable(neighbour, grid) && !closedSet.Contains(neighbour))
                                      {

                                          // Update the costs and parent if the neighbor is not in the closed set

                                          neighbour.G = currentNode.G + 1;
                                          neighbour.H = heuristic(neighbour, goal);
                                          neighbour.F = neighbour.G + neighbour.H;
                                          neighbour.Parent = currentNode;

                                          // Add the neighbor to the open set if it's not already there

                                          if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                                      }
                                  }
                                  else continue; //skips the neighbour node if its out of bounds
                              }
                          }
                      }


                      // A* algorithm implementation

                      //ESTA VA A SER PARA LAS UNIDADES QUE CAMINEN. Toma en cuenta riesgos. hay que hacer uno que no tome en cuenta riesgos, lo mismo para las flying units. 
                      private void AStar(Node start, Node goal, Node[,] grid)
                      {
                          List<Node> openSet = new List<Node>();  // Nodes to be evaluated
                          HashSet<Node> closedSet = new HashSet<Node>();  // Nodes already evaluated
                          openSet.Add(start);  // Start with the initial node

                          while (openSet.Count > 0)
                          {
                              Node currentNode = openSet[0];

                              // Find the node in openSet with the lowest F cost
                              for (int i = 1; i < openSet.Count; i++)
                              {
                                  if (openSet[i].F < currentNode.F) currentNode = openSet[i];

                                  openSet.Remove(currentNode);
                                  closedSet.Add(currentNode);

                                  // Check if the goal is reached
                                  if (currentNode.NodeLocation.LocationX == goal.NodeLocation.LocationX && currentNode.NodeLocation.LocationX == goal.NodeLocation.LocationX)
                                  {
                                      // If we reach the goal, we can use the Parent property to get the shortest path.
                                      // Alternatively, we can also use a different approach to get the path.
                                      Console.WriteLine("Path found.");
                                      return;
                                  }

                                  ExploreNeighbourNodes(currentNode, goal, openSet, closedSet, grid);
                              }
                          }
                      }

                  }
                  */




        /* internal class Program
     {
             private static bool isRunning = true;
             private static string? menuPick;
             private static List<MechanicalOperator> operators = new List<MechanicalOperator>();
             private static List<string> menuOptions = new List<string>();
             private static bool menuOptionsFlag = false;

             static void Main(string[] args)
             {


                 Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>
                 {
                     {"1", new RelayCommand(ShowOperatorStatus)},
                     {"2", new RelayCommand(ShowOperatorStatusAtLocation)},
                     {"3", new RelayCommand(TotalRecall)},
                     {"4", new RelayCommand(SelectOperator)},
                     {"5", new RelayCommand(AddReserveOperator)},
                     {"6", new RelayCommand(RemoveReserveOperator)},
                 };

                 while (isRunning)
                 {
                     PrintMenu();
                 menuPick = Console.ReadLine();

                 if (commands.TryGetValue(menuPick, out ICommand command))
                 {
                     command.Execute(null);
                 }
                 else
                 {
                     Console.Clear();
                     Console.WriteLine(" Pay attention to your inputs, please. (Press any key) ");
                     Console.ReadLine(); //No guarda el input, es una pausa.
                 }
             }
         }

         private static void PrintMenu()
         {
             Console.Clear();
             Console.WriteLine("\n           Welcome to the Skynet Headquarters            ");
             Console.WriteLine("                     Management Menu                      ");
             Console.WriteLine(" ---------------------------------------------------------");

             if (!menuOptionsFlag)
             {
                 menuOptions.Add("                  1. List all operators                   ");
                 menuOptions.Add("                 2. List operators at X                  ");
                 menuOptions.Add("                      3. Total recall                     ");
                 menuOptions.Add("                  4. Operator Operations                  ");
                 menuOptions.Add("                 5. Add Reserve Operators                 ");
                 menuOptions.Add("                6. Remove Reserve Operators               ");
                 menuOptionsFlag = true;
             }

             foreach (string option in menuOptions)
             {
                 Console.WriteLine(option);
             }
             Console.WriteLine(" ---------------------------------------------------------");
             Console.WriteLine("                  Please enter your Pick:                 ");
         }

         private static void ShowOperatorStatus(object parameter)
         {
             Console.Clear();
             Console.WriteLine("Operator Status:");
             foreach (var oper in operators)
             {
                 Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}");
             }
             Console.ReadLine();
         }
         private static void ShowOperatorStatusAtLocation(object parameter)
         {
             Console.Clear();
             Console.WriteLine("Enter location: ");
             string locationName = Console.ReadLine();

             Console.WriteLine($"Operator Status at {locationName}:");
             foreach (var oper in operators.Where(op => op.LocationP.LocationId.ToString() == locationName))
             {
                 Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}");
             }
             Console.ReadLine();
         }

         private static void TotalRecall(object parameter)
         {
             Console.Clear();
             Console.WriteLine("Performing total recall...");
             foreach (var oper in operators)
             {
                 //oper.LocationP = "headerquarters";
             }
             Console.WriteLine("All operators recalled to Headquarters.");
             Console.ReadLine();
         }
         private static void SelectOperator(object parameter)
         {
             Console.Clear();
             Console.WriteLine("Enter operator name: ");
             string operatorName = Console.ReadLine();

             var selectedOperator = operators.FirstOrDefault(op => op.Id == operatorName);

             if (selectedOperator != null)
             {
                 Console.WriteLine($"Selected operator: {selectedOperator.Id}, Status: {selectedOperator.Status}");
             }
             else
             {
                 Console.WriteLine($"Operator {operatorName} not found.");
             }
             Console.ReadLine();
         }
         private static void AddReserveOperator(object parameter)
         {
           /*  Console.Clear();
             Console.WriteLine("Enter reserve operator details: ");
             string operatorDetails = Console.ReadLine();

             var newOperator = new ConcreteMechanicalOperator
             {
                 Id = operatorDetails,
                 Status = "Reserve",
                 LocationP = new Location { // Inicializa las propiedades de Location si es necesario o ver cuales eran  }
             };

             operators.Add(newOperator);

             Console.WriteLine($"Adding reserve operator: {newOperator.Id}");
             Console.ReadLine();*/
        /*}

        private static void RemoveReserveOperator(object parameter)
        {
            Console.Clear();
            Console.WriteLine("Enter reserve operator name: ");
            string operatorName = Console.ReadLine();

            var reserveOperator = operators.FirstOrDefault(op => op.Id == operatorName && op.Status == "Reserve");

            if (reserveOperator != null)
            {
                operators.Remove(reserveOperator);
                Console.WriteLine($"Removing reserve operator: {reserveOperator.Id}");
            }
            else
            {
                Console.WriteLine($"Reserve operator {operatorName} not found.");
            }
            Console.ReadLine();
        }

        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;

            public RelayCommand(Action<object> execute)
            {
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            public event EventHandler CanExecuteChanged;
        }

    }*/

        //MechanicalOperator

        /*Estos eran los metodos anteriores:
                Los transfer me dejan en duda del tipo que deberian ser, considerando que para los k9 se le deberia meter un k9, un m8 debe ingresar un m8 y asi. Lo dejo comentado por ahora. 
                Este seria un buen uso de genericos?
                ademas, no haria las verificaciones aca, pero si en el metodo que llama los transfer. 

                public void TransferBattery(TIPO A ARREGLAR destination, float amount)
                {
                    currentBattery-=amount;
                    destination.setCurrentBattery = destination.getCurrentBattery + amount;
                }

                public void TransferLoad(TIPO A ARREGLAR destination, float amount)
                {
                    currentLoad-=amount;
                    destination.setCurrentLoad = destination.getCurrentLoad + amount;

                }

                public void ReturnToHQandRemoveLoad()
                {
                    LocationP.LocationX = HeadQuarters.GetInstance().LocationHeadQuarters.LocationX;
                    LocationP.LocationY = HeadQuarters.GetInstance().LocationHeadQuarters.LocationY;
                    CurrentLoad = 0;
                }

                public void ReturnToHQandChargeBattery()
                {
                    LocationP.LocationX = HeadQuarters.GetInstance().LocationHeadQuarters.LocationX;
                    LocationP.LocationY = HeadQuarters.GetInstance().LocationHeadQuarters.LocationY;
                    battery.CompleteBatteryLevel();
                }*/

        //Estos comentarios corresponden previo a la refactorizacion del codigo de GeneralOrder
        //Falta agregar el decrease battery
        // List<Node> closestDumpster = GetLocal(LocationP, 3, grid);
        //Node mostClosestDumpster= FindClosestNode(closestRecycling);
        //MoveTo(mostClosestDumpster.NodeLocation);


        //List<Node> closestRecycling = GetLocal(LocationP, 4, grid);
        // Node mostClosestRecycling= FindClosestNode(closestRecycling);
        //MoveTo(mostClosestRecycling.NodeLocation);

        //Este era el metodo MoveTo
        /*  public void MoveTo(Location loc)
        {
            damageSimulator.SimulateRandomDamage(this);

            double x = loc.LocationX;
            double y = loc.LocationY;
            int movX = 0;
            int movY = 0;
            busyStatus = true;
            //Se asigna que tipo de movimiento debe ser realizado para llegar a la cuadrilla que corresponde. 
            if (LocationP.LocationX < x)
            {
                movX = 1;
            }
            else if (LocationP.LocationX > x)
            {
                movX = -1;
            }

            if (LocationP.LocationY < y)
            {
                movY = 1;
            }
            else if (LocationP.LocationY > y)
            {
                movY = -1;
            }

            //se desplaza la posicion actual a la posicion buscada 

            while (LocationP.LocationY != y)
            {
                LocationP.LocationY += movY;
                /*
                InteractuarConPosicion() 

                Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                 que dependiendo del tipo de terreno tiene diferentes efectos
                */
        /*}
                while (LocationP.LocationX != x)
                {
                    LocationP.LocationX += movX;
                    /*
                    InteractuarConPosicion() 

                    Este debe ser un metodo que interactue con la casilla actual en el tp2, 
                     que dependiendo del tipo de terreno tiene diferentes efectos
                    */
        /*  }
      }

      private double CalculateBatteryConsumption(double distance)
{
  return 0.05 * (distance / 10); // Ajusta según tus necesidades
}
public void TransferBattery(MechanicalOperator destination, double amountPercentage)
{
  damageSimulator.SimulateRandomDamage(this);
  destination.busyStatus = true;
  busyStatus = true;
  //calcula que la carga no sea negativa
  if (amountPercentage < 0)
  {
      Console.WriteLine("Amount must be non-negative for Transfer Battery.");
      return;
  }
  if (AreOperatorsInSameLocation(destination))
  {
      if (ValidateBatteryTransfer(amountPercentage))
      {
          destination.battery.ChargeBattery(amountPercentage);
          battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
          destination.busyStatus = false;
          busyStatus = false;
      }
      else
      {
          Console.WriteLine("Transfer Battery aborted due to battery validation failure.");
          busyStatus = false;
      }
  }
  else
  { // Si no están en la misma ubicación, mueve el operador actual hacia la ubicación del destino.
      MoveTo(destination.LocationP);

      // Calcula la distancia entre los operadores y disminuye la batería del operador actual.
      double distance = CalculateDistance(destination.LocationP);
      // TODO valores a revisar creo q vuelve a ser el optimal speed

      if (ValidateBatteryTransfer(amountPercentage))
      {
          destination.battery.ChargeBattery(amountPercentage);
          battery.DecreaseBattery(CalculatePercentage(destination, amountPercentage));
          battery.DecreaseBattery(CalculateBatteryConsumption(distance));
          destination.busyStatus = false;
          busyStatus = false;
      }
  }
}*/

        //map


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

        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Blue;

        }*/


        //Enum
        /*
                        using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;
                using System.Threading.Tasks;

                namespace SkyNet
            {
                public enum EnumColors

                {/* Yo no se de enums, y por ahora no hace falta. pero idealmente al optimizar, lo usariamos. 
                BoringSector = ConsoleColor.DarkBlue,
                Dumpster = ConsoleColor.DarkGreen,
                Lake = ConsoleColor.Cyan,
                ElectroDumpster = ConsoleColor.Yellow,
                HQ = ConsoleColor.Gray,
                Recycler = ConsoleColor.Green,
                *//*
                }
        }*/


        /*
    public class Location
    {
        private int locationId;
        private string locationName;
        //private int[,] locationMatrix;
        private int locationX;
        private int locationY;
        private int horizontal;
        private int vertical;

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        //public int[,] LocationMatrix { get; set; }

        public int LocationX { get; set; }
        public int LocationY { get; set; }

        public Location()
        {
            LocationId = 0;
            LocationName = string.Empty;
            //LocationMatrix = new int[10, 10];
            LocationX = 0;
            LocationY = 0;
        }

        public Location(int horizontal, int vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }
    }*/


        //Viejo sistema de sincronizacion de operadores con hq y nodo. 

        /*
            private void GenerateRandomAmountOfOperators()
            {

                int OpAmount = rng.Next(1, 15); // 15 operator cap, para no explotar el mapa. 
                for (int i = 0; i < OpAmount; i++)
                {
                    bool inLoop = true;
                    while (inLoop)
                    {
                        int generatedType = rng.Next(1, 4);
                        int Xposition = rng.Next(0, Map.MapSize); //We use static variables in this method, instead of GetInstance(), as by
                        int Yposition = rng.Next(0, Map.MapSize); // this point the instance is still being built, so it evaluates to null, and breaks stuff.
                        if (generatedType == 1 && !CheckWater(Xposition, Yposition))
                        {
                            Operators.Add(new M8(Xposition, Yposition));
                            Map.Grid[Xposition, Yposition].OperatorsInNode.Add(new M8(Xposition, Yposition));
                            inLoop = false;
                        }
                        else if (generatedType == 2 && !CheckWater(Xposition, Yposition))
                        {
                            Operators.Add(new K9(Xposition, Yposition));
                            Map.Grid[Xposition, Yposition].OperatorsInNode.Add(new K9(Xposition, Yposition));
                            inLoop = false;
                        }
                        else if (generatedType == 3) //CheckWater is not necessary, UAV's can fly. 
                        {
                            Operators.Add(new UAV(Xposition, Yposition));
                            Map.Grid[Xposition, Yposition].OperatorsInNode.Add(new UAV(Xposition, Yposition));
                            inLoop = false;
                        }
                    }
                }
            }

            */
        //Old methods of SaveOrLoadGame Class
        //De esta manera nueva de hacer LoadGame no retorna nada en medio del bucle.
        /* public static void LoadGame()
         {
             bool isValidSelection = true;
             List<Map> loadedGames = new List<Map>();

             while (isValidSelection)
             {
                 try
                 {
                     string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json");
                     LoadGamesFromFiles(files, loadedGames);

                     if (loadedGames.Count == 0)
                     {
                         Console.WriteLine("No saved games available.");
                         continue;
                     }

                     DisplaySavedGames(loadedGames);

                     Console.WriteLine("Enter the number of the game you want to load");
                     if (TryGetSelectedGameIndex(Console.ReadLine(), loadedGames.Count, out int selectedGameIndex) && selectedGameIndex > 0)
                     {

                         Map selectedGame = loadedGames[selectedGameIndex - 1];

                     }
                     else
                     {
                         Console.WriteLine("Invalid input. Please enter a valid number.");
                         continue;
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error loading the game: {ex.Message}");
                     Console.WriteLine("An error occurred while loading the game. Please try again.");
                 }
                 finally
                 {
                     isValidSelection = false; // Esto evita que el bucle continúe indefinidamente
                 }
             }
         }

         /*static Map LoadGame()
         {
             bool isValidSelection = true;
             List<Map> loadedGames = new List<Map>();

             while (isValidSelection)
             {
                 try
                 {
                     string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json");
                     LoadGamesFromFiles(files, loadedGames);

                     if (loadedGames.Count == 0)
                     {
                         Console.WriteLine("No saved games avilable.");
                         continue; //en teoria con esto vuelve al princiio del bucle para q el usuario pueda seleccionar otro numero
                     }
                     DisplaySavedGames(loadedGames);

                     Console.WriteLine("Enter the number of the game you wanto to load");
                     if (TryGetSelectedGameIndex(Console.ReadLine(), loadedGames.Count, out int selectedGameIndex) && selectedGameIndex > 0)
                     {
                         return loadedGames[selectedGameIndex - 1];
                     }
                     else
                     {
                         Console.WriteLine("Invalid input. Please enter a valid number.");
                         continue;
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error loading the game: {ex.Message}");
                     Console.WriteLine("An error occurred while loading the game. Please try again.");
                 }

             }
             return null;
         }*/

        /*static void LoadGamesFromFiles(string[] files, List<Map> loadedGames)
        {
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    string content = File.ReadAllText(file);
                  //  Map loadedGame = Map.DeserializeFromJson(content);
               //     loadedGames.Add(loadedGame);
                    Console.WriteLine($"Game: {loadedGames.Count}, loaded successfully");
                }
                else
                {
                    Console.WriteLine("No saved game found");
                }
            }
        }

        static bool TryGetSelectedGameIndex(string input, int maxIndex, out int selectedGameIndex)
        {
            return int.TryParse(input, out selectedGameIndex) && selectedGameIndex > 0 && selectedGameIndex <= maxIndex;
        }


        static void DisplaySavedGames(List<Map> games)
        {
            Console.WriteLine("Saved Games:");
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Game {i + 1}");
            }

          public void ShowSavedGames()
        {
            if (Directory.Exists(SaveFolderPath))
            {
                string[] files = Directory.GetFiles(SaveFolderPath, "*.json");
                Console.WriteLine("Saved game files:");

                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.WriteLine("The saved games folder does not exist.");
            }
        }
        }
        public Map LoadGame(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The selected file does not exist.");
                return null;
            }

            try
            {
                string gameJson = File.ReadAllText(filePath);
                Map loadedMap = JsonSerializer.Deserialize<Map>(gameJson);
                Console.WriteLine("Game loaded successfully");
                return loadedMap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading the game: {ex.Message}");
                return null;
            }
        }

         */
    }
}
