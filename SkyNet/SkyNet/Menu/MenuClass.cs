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
            ClearMenuRemains();
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
            Console.WriteLine(" ------------------------------");


            // Read and set the selected HQ
            selectedHQ = GetValidHQSelection();
            ClearMenuRemains();
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
            Console.WriteLine("1. Show Operator Status           ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("2. Show Operators at Location     ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("3. Total Recall                   ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("4. Select Operator                ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("5. Add New Operator               ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("6. Destroy Operator               ");
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
                    Console.SetCursorPosition(W, H);
                    H++;
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
                    ClearMenuRemains();
                    GetConsoleSizeAfterMap();
                    Console.SetCursorPosition(W, H);
                    Console.WriteLine(" Pay attention to your inputs, please. (Press any key) ");
                    Console.ReadKey();
                    break;
            }
        }
        private void ShowOperatorStatus() 
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Operator Status:");
            int indexer = Convert.ToInt32(selectedHQ);
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer-1].Operators) 
            {
                Console.WriteLine($"Operator Id: {oper.Id}, Status: {oper.Status}, "+oper.ToString());
            }
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private void ShowOperatorStatusAtLocation()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.Write("Enter X location coordinates: "); //LA X; DP LA Y
            H++;
            //INPUT
            ClearMenuRemains();
            int Xinput = Convert.ToInt32(Console.ReadLine()); //AGREGAR VERIF
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.Write("Enter Y location coordinates: "); //LA X; DP LA Y
            H++;
            int Yinput = Convert.ToInt32(Console.ReadLine()); //AGREGAR VERIF
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine($"Operator Status at those coordinates:"); //No se ni si anda, pero printea void (wow)
            int indexer = Convert.ToInt32(selectedHQ);
            foreach (MechanicalOperator oper in Map.Grid[Xinput, Yinput].OperatorsInNode)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}, "+oper.ToString());
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Press any key to continue");
            }
            Console.ReadKey();
        }

        private void TotalRecall()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Performing total recall...");
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("If you want optimal search, press 1\n");
                H++;
                Console.SetCursorPosition(W, H);
            Console.WriteLine("If you want safe search, press 2");
            H++;
            Console.SetCursorPosition(W, H);
            int search = Convert.ToInt32(Console.ReadLine()); //verificaciones
            bool safety = false;
            if (search == 2)
            {
                safety = true;
            }

            int indexer = Convert.ToInt32(selectedHQ);
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer-1].Operators)
            {
                oper.MoveTo(Map.GetInstance().HQList[indexer-1].LocationHeadQuarters, safety);
            }
            Console.WriteLine("All operators recalled to Headquarters.");
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);
            Map.GetInstance().PrintMap(); //actualiza el mapa
        }

        private void SelectOperator() 
        {

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            //Deberiamos traer una lista de todos operadores con sus id en el cuartel q seleccionas
            List<MechanicalOperator> operators = Map.GetInstance().HQList[0].Operators;
            foreach (MechanicalOperator op in operators)
            {
                Console.WriteLine(op.Id);
            }
            Console.Write("Enter operator Id: ");
            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer-1].LocationHeadQuarters.LocationX;
            int Yposition = Map.GetInstance().HQList[indexer-1].LocationHeadQuarters.LocationY;
            string operatorId = Console.ReadLine().ToUpper(); //Formato incorrecto? verificacion falta 
            var selectedOperator = Map.GetInstance().HQList[indexer-1].Operators.FirstOrDefault(op => op.Id.Equals(operatorId)); //al tocar 4, obj ref not set to an instance of an object. 

            if (selectedOperator != null)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Selected Operator {selectedOperator.Id}, Status: {selectedOperator.Status}" +
                    $"\n Choose an option:" +
                    $"\n 1. Move To" +
                    $"\n 2. Transfer Battery" +
                    $"\n 3. Transfer Load" +
                    $"\n 4. General Order: if operator is damaged, repair it at the headquerter" +
                    $"\n 5. Battery Change");
                Console.WriteLine("Select option: ");

                string subOption = Console.ReadLine();
                HandleSubOption(subOption, selectedOperator);
            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Operator {operatorId} not found.");
            }
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
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
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
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
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Performing Battery Change...");
                Node[,] grid = GetGrid();
                selectedOperator.BatteryChange(grid);
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Battery Change completed successfully");
            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Battery is not perforated.Battery cannot be changed");
            }
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Map.GetInstance().PrintMap(); //actualiza el mapa
        }
        private void GeneralOrderMenu(MechanicalOperator selectedOperator)
        {
            if (selectedOperator.BusyStatus == false) {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Executing General Order...");

            Node[,] grid = GetGrid();

                selectedOperator.GeneralOrder(grid);
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("General Order executed successfully.");
            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Operator is busy. General Order cannot be executed.");
            }
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Map.GetInstance().PrintMap(); //actualiza el mapa
        }

        private void MoveToMenu(MechanicalOperator selectedOperator)
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Enter destination coordinates X: ");
            //aca habria q ingresar las coordenadas para q vayan al mapa
            int Xposition = Convert.ToInt32(Console.ReadLine());
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Enter destination coordinates Y: ");
            int Yposition = Convert.ToInt32(Console.ReadLine());
            Location location = new Location(Xposition, Yposition);
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("If you want optimal search, press 1\n" +
                                   "If you want safe search, press 2");
                int search = Convert.ToInt32(Console.ReadLine());
                bool safety = false;
                if (search == 2)
                {
                    safety = true;
                }

                int indexer = Convert.ToInt32(selectedHQ);
                selectedOperator.MoveTo(location, safety);
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
        private void TransferBatteryMenu(MechanicalOperator selectedOperator)
        {
            Location currentLocation = selectedOperator.LocationP;
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);


            //LISTA DE TODOS LOS OPERADORES DE TODOS LOS CUARTELES
            foreach (HeadQuarters hq in Map.GetInstance().HQList)
            {
                List<MechanicalOperator> operators = hq.Operators;

                foreach (MechanicalOperator op in operators)
                {
                    Console.WriteLine(op.Id+" "+op.Battery.CurrentChargePercentage);
                }
            }

            Console.WriteLine("Enter destination operator Id: ");
            string destOperatorId = Console.ReadLine();
            var destinationOperator = Map.GetInstance().HQList
                         .SelectMany(hq => hq.Operators)
                         .FirstOrDefault(op => op.Id.Equals(destOperatorId, StringComparison.OrdinalIgnoreCase));

            if (destinationOperator != null)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Enter percentage of battery to transfer: ");
                double percentage = Convert.ToDouble(Console.ReadLine());

                selectedOperator.TransferBattery(destinationOperator, percentage);
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Bateria operador {destinationOperator.Battery.CurrentChargePercentage}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            

            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Operator {destOperatorId} not found.");
            }
        }

        private void TransferLoadMenu(MechanicalOperator selectedOperator)
        {
            Location currentLocation = selectedOperator.LocationP;
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);


            //LISTA DE TODOS LOS OPERADORES DE TODOS LOS CUARTELES
            foreach (HeadQuarters hq in Map.GetInstance().HQList)
            {
                List<MechanicalOperator> operators = hq.Operators;

                foreach (MechanicalOperator op in operators)
                {
                    Console.WriteLine(op.Id + " " + op.Battery.CurrentChargePercentage);
                }
            }

            Console.WriteLine("Enter destination operator Id: ");
            string destOperatorId = Console.ReadLine();
            var destinationOperator = Map.GetInstance().HQList
                         .SelectMany(hq => hq.Operators)
                         .FirstOrDefault(op => op.Id.Equals(destOperatorId, StringComparison.OrdinalIgnoreCase));

            if (destinationOperator != null)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Enter amount of load to transfer: ");
                double amountKG = Convert.ToDouble(Console.ReadLine());

                selectedOperator.TransferLoad(destinationOperator, amountKG);
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Operator {destOperatorId} not found.");
            }
        }
        private void AddOperator()
        {
            //NECESITA VERIFICACIONES
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Enter operator type (1 = K9, 2 = UAV, 3 = M8): ");
            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer-1].LocationHeadQuarters.LocationX; //Index out of range ????? al crear hq
            int Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;
            int operatorType = Convert.ToInt32(Console.ReadLine()); //Agregar verif

            if (operatorType == 1)
            {
                M8 m8 = new M8(Xposition, Yposition);
                Map.GetInstance().HQList[indexer - 1].Operators.Add(m8);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(m8);
                Map.M8Counter++;
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Added!");
                Console.ReadKey();
            }
            else if (operatorType == 2)
            {
                K9 k9 = new K9(Xposition, Yposition);
                Map.GetInstance().HQList[indexer - 1].Operators.Add(k9);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(k9);
                Map.K9Counter++;
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Added!");
                Console.ReadKey();
            }
            else if (operatorType == 3)
            {
                UAV uav = new UAV(Xposition, Yposition);
                Map.GetInstance().HQList[indexer - 1].Operators.Add(uav);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(uav);
                Map.UAVCounter++;
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Added!");
                Console.ReadKey();
            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Failed.");
                Console.ReadKey();
            }
            Map.GetInstance().PrintMap(); //actualiza el mapa

        }

        private void RemoveOperator()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.Write("Enter Operator ID to remove: ");
            string operatorId = Console.ReadLine();

            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationX; //Out of range 
            int Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;
            //verificar que exista
            MechanicalOperator removeOp = Map.GetInstance().HQList[indexer - 1].Operators.FirstOrDefault(op => op.Id == operatorId);
            if (removeOp != null)
            {
                Location toDelete = removeOp.LocationP;
                Map.Grid[toDelete.LocationX, toDelete.LocationY].OperatorsInNode.Remove(removeOp);
                Map.GetInstance().HQList[indexer-1].Operators.Remove(removeOp);
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Removing operator: {removeOp.Id}");
            }
            else
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Operator {operatorId} not found.");
            }
            Console.ReadKey();
            Map.GetInstance().PrintMap(); //actualiza el mapa
        }

        private void ClearMenuRemains()
        {
            GetConsoleSizeAfterMap();
             for (int i = 0; i <15; i++)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine("                                                         ");
                H++;
            }
        }
    }
}