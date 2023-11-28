using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class CemeteryClass
    {

        //This is the cemetery class, meant purely for storing old code and cleaning up the project. Feel free to add fun comments. 


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

        //Estos comentarios corresponden previo a la refactorizacion del codigo
        //Falta agregar el decrease battery
        // List<Node> closestDumpster = GetLocal(LocationP, 3, grid);
        //Node mostClosestDumpster= FindClosestNode(closestRecycling);
        //MoveTo(mostClosestDumpster.NodeLocation);


        //List<Node> closestRecycling = GetLocal(LocationP, 4, grid);
        // Node mostClosestRecycling= FindClosestNode(closestRecycling);
        //MoveTo(mostClosestRecycling.NodeLocation);



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
    }
}
