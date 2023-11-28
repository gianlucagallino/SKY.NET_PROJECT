using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SkyNet
{
    internal class Program
    {
        static void Main(string[] args)

        {
            
         
            // Habria que agarrar el menu del proyecto anterior, y pasarlo a command. 
            // El codigo anterior deberia estar disponible en el segundo repo que hizo cata. 


            Introduction.GetInstance().Play();

            Map.GetInstance().PrintMap();

             

            //Menu();

        }
    }
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
}

