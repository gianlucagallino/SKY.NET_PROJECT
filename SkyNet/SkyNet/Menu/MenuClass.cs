using SkyNet.Entidades;
using SkyNet.Entidades.Mapa;
using SkyNet.Entidades.Operadores;

namespace SkyNet.Menu
{
    internal class MenuClass
    {
        private bool menuOptionsFlag;
        private string selectedHQ;
        public int W { set; get; }
        public int H { set; get; }

        public MenuClass()
        {
            menuOptionsFlag = false;
        }

        public void GetConsoleSizeAfterMap()
        {
            W = Map.MapSize * 2 + 5;
            H = 5;
        }
        public void RunMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                PrintMenu();
                string menuPick = Console.ReadLine();
                ExecuteCommand(menuPick);
            }
        }

        private void PrintMenu()
        {
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" _______________________________");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("|        Management Menu        | ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" ------------------------------- ");

            int conta = 1;
            for (int i = 0; i < Map.GetInstance().HeadquarterCounter; i++)
            {
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine($"{conta}. HeadQuarter {conta}");
                conta++;

            }
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" -------------------------------");


            // Read and set the selected HQ
            selectedHQ = GetValidHQSelection();

            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" _______________________________  ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("|        Operator Menu          | ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" -------------------------------  ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("1. Show Operator Status          ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("2. Show Operators at Location     ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("3. Total Recall                   ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("4. Select Operator                 ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("5. Add New Operator                ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("6. Destroy Operator                ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" -------------------------------");
            Console.SetCursorPosition(W, H);
            H++;
            Console.Write("     Pick an option: ");
        }


        private string GetValidHQSelection()
        {
            int maxHQ = Map.GetInstance().HeadquarterCounter;
            string input = "";
            bool isRunning = true;

            while (isRunning)
            {
                Console.SetCursorPosition(W, H);
                H++;
                Console.Write($"     Pick an HQ (1-{maxHQ}): ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int selected) && selected >= 1 && selected <= maxHQ)
                {
                    isRunning = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Invalid selection. Please enter a number between 1 and {maxHQ}.");
                }
            }

            return input;
        }

        private void ExecuteCommand(string menuPick)
        {
            // Lógica para ejecutar el comando seleccionado
            switch (menuPick)
            {
                case "1":
                    ShowOperatorStatus();
                    break;
                case "2":
                    ShowOperatorStatusAtLocation();
                    break;
                case "3":
                    TotalRecall();
                    break;
                case "4":
                    SelectOperator();
                    break;
                case "5":
                    AddOperator();
                    break;
                case "6":
                    RemoveOperator();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine(" Pay attention to your inputs, please. (Press any key) ");
                    Console.ReadKey();
                    break;
            }
        }
        private void ShowOperatorStatus()
        {
            Console.Clear();
            Console.WriteLine("Operator Status:");
            int indexer = Convert.ToInt32(selectedHQ);
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer].Operators)
            {

                Console.WriteLine($"Operator Id: {oper.Id}, Status: {oper.Status}");
            }
        }

        private void ShowOperatorStatusAtLocation()
        {
            Console.Clear();
            Console.WriteLine("Enter location coordinates: ");
            //INPUT
            int Xinput = Convert.ToInt32(Console.ReadLine()); //AGREGAR VERIF
            int Yinput = Convert.ToInt32(Console.ReadLine()); //AGREGAR VERIF
            Console.WriteLine($"Operator Status at those coordinates:");
            int indexer = Convert.ToInt32(selectedHQ);
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer].Operators.Where(op => op.LocationP.LocationX == Xinput && op.LocationP.LocationY == Yinput))
            {
                Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}");
            }
            Console.ReadKey();
        }

        private void TotalRecall()
        {
            Console.Clear();
            Console.WriteLine("Performing total recall...");
            int indexer = Convert.ToInt32(selectedHQ);
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer].Operators)
            {
                oper.MoveTo(Map.GetInstance().HQList[indexer].LocationHeadQuarters);
            }
            Console.WriteLine("All operators recalled to Headquarters.");
            Console.ReadKey();
        }

        private void SelectOperator()
        { 

            Console.Clear();
            Console.WriteLine("Enter operator Id ");
            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer].LocationHeadQuarters.LocationX;
            int Yposition = Map.GetInstance().HQList[indexer].LocationHeadQuarters.LocationY;
            int operatorId = Convert.ToInt32(Console.ReadLine());
            var selectedOperator = Map.GetInstance().HQList[indexer].Operators.FirstOrDefault(op => op.Id.Equals(operatorId));

            if (selectedOperator != null)
            {
                Console.WriteLine($"Selected Operator {selectedOperator.Id}, Status: {selectedOperator.Status}" +
                    $"\n Choose an option:" +
                    $"\n 1. Move To" +
                    $"\n 2. Transfer Battery" +
                    $"\n 3. Transfer Load" +
                    $"\n 4. General Order: if operator is damaged, repair it at the headquerter" +
                    $"\n 5. Battery Change");

                string subOption = Console.ReadLine();
                HandleSubOption(subOption, selectedOperator);
            }
            else
            {
                Console.WriteLine($"Operator {operatorId} not found.");
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();


        }
        private void HandleSubOption(string subOption, MechanicalOperator selectedOperator)
        {
            Dictionary<string, Action> subOptions = new Dictionary<string, Action>
            {
                {"1",()=>MoveToMenu(selectedOperator) },
                {"2",()=> TransferBatteryMenu(selectedOperator) },
                {"3",()=>TransferLoadMenu(selectedOperator) },
                {"4",()=>GeneralOrderMenu(selectedOperator) },
                {"5",()=>ChangeBatteryMenu(selectedOperator) }
            };

            if (subOptions.ContainsKey(subOption))
            {
                subOptions[subOption].Invoke();
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }

        private Node[,] GetGrid()
        {
            return Map.Grid;//al ser miembro estatico no me funcionaba con el GetInstance().
            //Revisar que funcione!!!
        }
        private void ChangeBatteryMenu(MechanicalOperator selectedOperator)
        {
            Console.Clear();

            if (selectedOperator.DamageSimulatorP.PerforatedBattery)
            {
                Console.WriteLine("Performing Battery Change...");
                Node[,] grid = GetGrid();
                selectedOperator.BatteryChange(grid);
                Console.WriteLine("Battery Change completed successfully");
            }
            else
            {
                Console.WriteLine("Battery is not perforated.Battery cannot be changed");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        private void GeneralOrderMenu(MechanicalOperator selectedOperator)
        {
            if (selectedOperator.BusyStatus == false)
            {
                Console.Clear();
                Console.WriteLine("Executing General Order...");

                Node[,] grid = GetGrid();

                selectedOperator.GeneralOrder(grid);
                Console.WriteLine("General Order executed successfully.");
            }
            else
            {
                Console.WriteLine("Operator is busy. General Order cannot be executed.");
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private void MoveToMenu(MechanicalOperator selectedOperator)
        {
            Console.Clear();
            Console.WriteLine("Enter destination coordinates X: ");
            //aca habria q ingresar las coordenadas para q vayan al mapa
            int Xposition = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter destination coordinates Y: ");
            int Yposition = Convert.ToInt32(Console.ReadLine());
            Location location = new Location(Xposition, Yposition);
            selectedOperator.MoveTo(location);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        private void TransferBatteryMenu(MechanicalOperator selectedOperator)
        {
            Console.Clear();
            Console.WriteLine("Enter destination operator Id: ");
            string destOperatorId = Console.ReadLine();
            int indexer = Convert.ToInt32(selectedHQ);
            var destinationOperator = Map.GetInstance().HQList[indexer].Operators.FirstOrDefault(op => op.Id.Equals(destOperatorId));

            if (destinationOperator != null)
            {
                Console.WriteLine("Enter percentage of battery to transfer: ");
                double percentage = Convert.ToDouble(Console.ReadLine());

                selectedOperator.TransferBattery(destinationOperator, percentage);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Operator {destOperatorId} not found.");
            }
        }

        private void TransferLoadMenu(MechanicalOperator selectedOperator)
        {
            Console.Clear();
            Console.WriteLine("Enter destination operator Id: ");
            string destOperatorId = Console.ReadLine();
            int indexer = Convert.ToInt32(selectedHQ);
            var destinationOperator = Map.GetInstance().HQList[indexer].Operators.FirstOrDefault(op => op.Id.Equals(destOperatorId));

            if (destinationOperator != null)
            {
                Console.WriteLine("Enter amount of load to transfer: ");
                double amountKG = Convert.ToDouble(Console.ReadLine());

                selectedOperator.TransferLoad(destinationOperator, amountKG);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Operator {destOperatorId} not found.");
            }
        }
        private void AddOperator()
        {
            //NECESITA VERIFICACIONES
            Console.Clear();
            Console.WriteLine("Enter operator type (1 = K9, 2 = UAV, 3 = M8): ");
            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer].LocationHeadQuarters.LocationX;
            int Yposition = Map.GetInstance().HQList[indexer].LocationHeadQuarters.LocationY;
            int operatorType = Convert.ToInt32(Console.ReadLine()); //Agregar verif

            if (operatorType == 1)
            {
                M8 m8 = new M8(Xposition, Yposition);
                Map.GetInstance().HQList[indexer].Operators.Add(m8);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(m8);
                Map.M8Counter++;
            }
            else if (operatorType == 2)
            {
                K9 k9 = new K9(Xposition, Yposition);
                Map.GetInstance().HQList[indexer].Operators.Add(k9);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(k9);
                Map.K9Counter++;
            }
            else
            {
                UAV uav = new UAV(Xposition, Yposition);
                Map.GetInstance().HQList[indexer].Operators.Add(uav);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(uav);
                Map.UAVCounter++;
            }
            Console.WriteLine("Added!");
            Console.ReadKey();
        }

        private void RemoveOperator()
        {
            Console.Clear();
            Console.WriteLine("Enter Operator ID to remove: ");
            string operatorId = Console.ReadLine();

            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer].LocationHeadQuarters.LocationX;
            int Yposition = Map.GetInstance().HQList[indexer].LocationHeadQuarters.LocationY;
            //verificar que exista
            var removeOp = Map.GetInstance().HQList[indexer].Operators.FirstOrDefault(op => op.Id == operatorId);
            if (removeOp != null)
            {
                Map.GetInstance().HQList[indexer].Operators.Remove(removeOp);
                Console.WriteLine($"Removing operator: {removeOp.Id}");
            }
            else
            {
                Console.WriteLine($"Operator {operatorId} not found.");
            }
            Console.ReadKey();
        }
    }
}