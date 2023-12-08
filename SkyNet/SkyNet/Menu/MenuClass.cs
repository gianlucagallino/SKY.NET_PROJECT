using SkyNet.Data;
using SkyNet.Entidades;
using SkyNet.Entidades.Mapa;
using SkyNet.Entidades.Operadores;

namespace SkyNet.Menu
{

    //This class is responsable for menu graphics, input handling, and wrapping operator actions. 
    internal class MenuClass
    {
        private bool menuOptionsFlag;
        private string selectedHQ;
        SaveOrLoadGame saver;
        public int W { set; get; }
        public int H { set; get; }

        public MenuClass()
        {
            menuOptionsFlag = false;
            saver = new SaveOrLoadGame();
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
            Console.WriteLine("|          General Menu         | ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" ------------------------------- ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("1. Management Menu                ");

            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("100. Exit                           ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("200. Show saved Games               ");
            Console.SetCursorPosition(W, H);
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" -------------------------------");
            string pickedOption = GetValidOptionSelection();


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
            Console.WriteLine(" -------------------------------");


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

        // Logic to execute the selected command
        private void ExecuteCommand(string menuPick)
        {

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
                    HandleInvalidInput();
                    break;
            }
        }

        // Method to handle invalid input
        private void HandleInvalidInput()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Invalid input. (Press any key) ");
            Console.ReadKey();
        }

        //Game exit related Functions
        private void Exit()
        {
            Console.WriteLine("Are you sure you want to exit?" +
                              "\n Press 1 if you want to exit and save the game" +
                              "\n Press 2 if you want to exit without saving ");

            int response;
            while (!int.TryParse(Console.ReadLine(), out response) || (response != 1 && response != 2))
            {
                Console.WriteLine("Invalid input. Try again.");
            }

            if (response == 1)
            {
                SaveAndExit();
            }
            else if (response == 2)
            {
                ExitWithoutSaving();
            }
        }

        // Save the game and exit
        private void SaveAndExit()
        {
            int nextGame = HelperDB.ObtenerInstancia().GetNextGame();

            // Insert game and operators into the database
            HelperDB.ObtenerInstancia().InsertPartida(nextGame);
            foreach (var oper in Map.GetInstance().GetAllOperators())
            {
                HelperDB.ObtenerInstancia().InsertOperator(oper, nextGame);
            }

            // Save the game state
            saver.SaveGame();

            // Exit the application
            Environment.Exit(0);
        }

        // Exit without saving
        private void ExitWithoutSaving()
        {
            Console.WriteLine("Thank you for playing!");
            Environment.Exit(0);
        }

        //Show operator status menu options
        private void ShowOperatorStatus()
        {
            // Clear remnants and get console size
            ClearMenuRemains();
            GetConsoleSizeAfterMap();

            Console.SetCursorPosition(W, H);
            Console.WriteLine("Operator Status:");

            int indexer = Convert.ToInt32(selectedHQ);
            var operators = Map.GetInstance().HQList[indexer - 1].Operators;

            if (operators.Count == 0)
            {
                // No operators belong to this HQ
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("No operators belong to this HQ.");
            }
            else
            {
                DisplayOperatorStatus(operators);
            }

            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        // Method to display operator status
        private void DisplayOperatorStatus(List<MechanicalOperator> operators)
        {
            foreach (MechanicalOperator oper in operators)
            {
                H++;
                Console.SetCursorPosition(W, H);
                Console.Write($"Id: {oper.Id}, Status: {oper.Status}, X: {oper.LocationP.LocationX}, Y: {oper.LocationP.LocationY}, " + oper.ToString());
            }
        }

        public void ShowOperatorStatusAtLocation()
        {
            int xInput;
            int yInput;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();

            Console.SetCursorPosition(W, H);
            Console.Write("Enter coordinates: ");
            H++;

            // Input for X coord
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter an X location between 0 and {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out xInput) || xInput < 0 || xInput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter an X location between 0 and {Map.MapSize - 1}: ");
            }
            H++;

            // Input for Y coord
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter a Y location between 0 and {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out yInput) || yInput < 0 || yInput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter a Y location between 0 and {Map.MapSize - 1}: ");
            }
            H++;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();

            Console.SetCursorPosition(W, H);
            Console.WriteLine($"Operator Status at coordinates ({xInput}, {yInput}):");
            H++;

            ClearMenuRemains();

            // Display operator status
            if (Map.Grid[xInput, yInput].OperatorsInNode.Count == 0)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine("No operators here!");
            }
            else
            {
                foreach (MechanicalOperator oper in Map.Grid[xInput, yInput].OperatorsInNode)
                {
                    Console.SetCursorPosition(W, H);
                    Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}, " +
                                      $"X: {oper.LocationP.LocationX}, Y: {oper.LocationP.LocationY}, " +
                                      oper.ToString());
                    H++;
                }
            }

            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }



        private void TotalRecall()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.WriteLine("Performing total recall...");
            Thread.Sleep(2000);

            // Ask for safety confirmation
            bool safety = AskForSafety();

            //Retrieve the selected HQ index
            int selectedHQIndex = Convert.ToInt32(selectedHQ);

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            //Iterate through operators in the selected HQ
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[selectedHQIndex - 1].Operators)
            {

                Console.WriteLine($"{oper.Id}: ");

                H++;
                Console.SetCursorPosition(W, H);

                // Move operator to HQ location
                oper.MoveTo(Map.GetInstance().HQList[selectedHQIndex - 1].LocationHeadQuarters, safety, selectedHQIndex - 1, oper.Id);
                H++;

                Console.SetCursorPosition(W, H);
            }

            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            UpdateMap();
        }

        //Method that selects an operator to apply one of the sub-actions to
        private void SelectOperator()
        {
            int subOption;
            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition;
            int Yposition;
            string operatorId;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            H++;

            DisplayOperatorsInHq(indexer);

            Console.SetCursorPosition(W, H);
            Console.Write("Enter operator Id: ");

            Xposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationX;
            Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;

            operatorId = Console.ReadLine().ToUpper();
            var selectedOperator = Map.GetInstance().HQList[indexer - 1].Operators.FirstOrDefault(op => op.Id.Equals(operatorId));

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            if (selectedOperator != null)
            {

                Console.WriteLine($"Selected Operator {selectedOperator.Id}, Status: {selectedOperator.Status}");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Choose an option: ");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("1. Move To");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("2. Transfer Battery");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("3. Transfer Load");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("4. General Order (Go to nearest recycler & pick up all possible weight)");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("5. General Order (Go to nearest HQ and heal)");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("6. Battery Change");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Select option: ");

                H++;
                Console.SetCursorPosition(W, H);
                Console.Write("Enter an integer from 1 to 6:");

                while (!int.TryParse(Console.ReadLine(), out subOption) || subOption < 1 || subOption > 6)
                {
                    ClearMenuRemains();
                    Console.SetCursorPosition(W, H);
                    Console.Write("Invalid input. Please enter an integer from 1 to 6:");
                }

                HandleSubOption(subOption.ToString(), selectedOperator);
            }
            else
            {
                Console.WriteLine($"Operator {operatorId} not found.");
                H++;
            }

            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        // Display list of operators for the selected headquarters
        private void DisplayOperatorsInHq(int indexer)
        {
            List<MechanicalOperator> operators = Map.GetInstance().HQList[indexer - 1].Operators;
            foreach (MechanicalOperator op in operators)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"{op.Id} -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY} ");
                H++;
            }
        }

        // Handles the sub-options for the chosen operator
        private void HandleSubOption(string subOption, MechanicalOperator selectedOperator)
        {
            Dictionary<string, Action> subOptions = new Dictionary<string, Action>
            {
                {"1",()=>MoveToMenu(selectedOperator) },
                {"2",()=> TransferBatteryMenu(selectedOperator) },
                {"3",()=>TransferLoadMenu(selectedOperator) },
                {"4",()=>GeneralOrderWeightMenu(selectedOperator) },
                {"5",()=>GeneralOrderHealMenu(selectedOperator) },
                {"6",()=>ChangeBatteryMenu(selectedOperator) }
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

        //Method to determine if the battery will be change or not
        private void ChangeBatteryMenu(MechanicalOperator selectedOperator)
        {
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();

            // Check if the battery is perforated
            if (selectedOperator.DamageSimulatorP.PerforatedBattery)
            {
                PerformBatteryChange(selectedOperator, whatHeadquarter, safety);
            }
            else
            {
                DisplayBatteryNotPerforatedMessage();
            }

            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            UpdateMap();
        }

        private void PerformBatteryChange(MechanicalOperator selectedOperator, int whatHeadquarter, bool safety)
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.WriteLine("Performing Battery Change...");

            // Perform Battery Change
            selectedOperator.BatteryChange(Map.Grid, safety, whatHeadquarter, selectedOperator.Id);

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.WriteLine("Battery Change completed successfully");
        }

        private void DisplayBatteryNotPerforatedMessage()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.WriteLine("Battery is not perforated. Battery cannot be changed");
        }

        //Method to determine if the general order will be executed
        private void GeneralOrderWeightMenu(MechanicalOperator selectedOperator)
        {
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();

            // Check if the operator is not busy
            if (!selectedOperator.BusyStatus)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);

                Console.WriteLine("Executing General Order...");
                Thread.Sleep(2000);

                // Execute General Order for weight
                selectedOperator.GeneralOrderWeight(Map.Grid, operatorId, whatHeadquarter, safety);

                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
            }
            else
            {
                DisplayOperatorBusyMessage();
            }

            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            UpdateMap();
        }

        private void DisplayOperatorBusyMessage()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.WriteLine("Operator is busy. Execution failed.");
        }

        //Method to determine if the heal general order will be executed
        private void GeneralOrderHealMenu(MechanicalOperator selectedOperator)
        {
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();

            // Check if the operator is not busy
            if (!selectedOperator.BusyStatus)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);

                Console.WriteLine("Executing General Order...");
                Thread.Sleep(2000);

                // Execute General Order for healing
                selectedOperator.GeneralOrderHeal(Map.Grid, operatorId, whatHeadquarter, safety);

                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
            }
            else
            {
                DisplayOperatorBusyMessage();
            }

            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            UpdateMap();
        }

        //Wrapper for the MoveTo method
        private void MoveToMenu(MechanicalOperator selectedOperator)
        {
            int Xinput;
            int Yinput;
            bool safety;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            // Get X location input from the user
            Console.Write($"Enter an X location from 0 to {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out Xinput) || Xinput < 0 || Xinput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter an integer between 0 and {Map.MapSize - 1}: ");
            }
            H++;

            // Get Y location input from the user
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter a Y location from 0 to {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out Yinput) || Yinput < 0 || Yinput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter a number from 0 to {Map.MapSize - 1}: ");
            }
            H++;

            Location location = new Location(Xinput, Yinput);

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            // Get safety input from the user
            safety = AskForSafety();
            H++;

            // Move the operator to the specified location
            MoveOperatorToLocation(selectedOperator, location, safety);

            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        //Performs the MoveTo action
        private void MoveOperatorToLocation(MechanicalOperator selectedOperator, Location location, bool safety)
        {
            // Get headquarters indexer
            int indexer = Convert.ToInt32(selectedHQ);

            // Move the operator to the specified location
            selectedOperator.MoveTo(location, safety, indexer - 1, selectedOperator.Id);

            UpdateMap();
        }

        //Method to determine path of action for battery transfering
        private void TransferBatteryMenu(MechanicalOperator selectedOperator)
        {
            Location currentLocation = selectedOperator.LocationP;
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            // Display a list of all operators in all headquarters
            DisplayOperatorsInAllHeadquarters();

            Console.WriteLine("Enter destination operator Id: ");
            string destOperatorId = Console.ReadLine();

            // Find destination operator
            var destinationOperator = FindOperatorById(destOperatorId);

            if (destinationOperator != null)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);

                Console.WriteLine("Enter battery percentage to transfer: ");
                double percentage = GetBatteryTransferPercentage();

                // Transfer battery
                selectedOperator.TransferBattery(destinationOperator, percentage, safety, whatHeadquarter, operatorId);

                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);

                Console.WriteLine($"Operator charge: {destinationOperator.Battery.CurrentChargePercentage}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                DisplayOperatorNotFoundMessage(destOperatorId);
            }
        }

        // Display a list of all operators in all headquarters
        private void DisplayOperatorsInAllHeadquarters()
        {
            foreach (HeadQuarters hq in Map.GetInstance().HQList)
            {
                List<MechanicalOperator> operators = hq.Operators;

                foreach (MechanicalOperator op in operators)
                {
                    Console.WriteLine($"{op.Id} -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY} - {op.Battery.CurrentChargePercentage}");
                }
            }
        }

        // Find destination operator. It will return null if not found, which is handled already
        private MechanicalOperator FindOperatorById(string destOperatorId)
        {
            return Map.GetInstance().HQList
                          .SelectMany(hq => hq.Operators)
                          .FirstOrDefault(op => op.Id.Equals(destOperatorId, StringComparison.OrdinalIgnoreCase));
        }

        // Get battery transfer percentage from the user
        private double GetBatteryTransferPercentage()
        {
            double percentage;
            while (!double.TryParse(Console.ReadLine(), out percentage) || percentage < 0)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Invalid input. Please enter a non-negative number for the battery percentage: ");
            }
            return percentage;
        }


        // Display message if the destination operator is not found
        private void DisplayOperatorNotFoundMessage(string destOperatorId)
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.WriteLine($"Operator {destOperatorId} not found.");
        }


        //Wrapper for the load transfer
        private void TransferLoadMenu(MechanicalOperator selectedOperator)
        {
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();
            Location currentLocation = selectedOperator.LocationP;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            // Display a list of all operators in all headquarters
            DisplayOperatorsInAllHeadquarters();

            Console.WriteLine("Enter destination operator Id: ");
            string destOperatorId = Console.ReadLine();

            // Find destination operator
            var destinationOperator = FindOperatorById(destOperatorId);

            if (destinationOperator != null)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);

                Console.WriteLine("Enter amount of load to transfer: ");
                double amountKG = GetLoadTransferAmount();

                // Transfer load
                selectedOperator.TransferLoad(destinationOperator, amountKG, safety, whatHeadquarter, operatorId);

                ClearMenuRemains();
                GetConsoleSizeAfterMap();

                Console.SetCursorPosition(W, H);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                DisplayOperatorNotFoundMessage(destOperatorId);
            }
        }

        // Get load transfer amount from the user
        private double GetLoadTransferAmount()
        {
            double amountKG;
            while (!double.TryParse(Console.ReadLine(), out amountKG) || amountKG < 0)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Invalid input. Please enter a non-negative number for the load amount: ");
            }
            return amountKG;
        }

        //Wrapper for operator generation
        private void AddOperator()
        {
            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationX;
            int Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;
            int operatorType;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();

            // Prompt user to enter operator type
            Console.SetCursorPosition(W, H);
            Console.Write("Enter type number (1: M8, 2: K9, 3: UAV): ");
            while (!int.TryParse(Console.ReadLine(), out operatorType) || (operatorType < 1 || operatorType > 3))
            {
                Console.SetCursorPosition(W, H);
                Console.Write("Invalid input. Enter type number (1: M8, 2: K9, 3: UAV): ");
            }

            // Add operator based on the selected type
            MechanicalOperator newOperator = CreateOperator(operatorType, Xposition, Yposition);
            Map.GetInstance().HQList[indexer - 1].Operators.Add(newOperator);
            Map.Grid[Xposition, Yposition].OperatorsInNode.Add(newOperator);

            DisplayCreationMessage(newOperator);

            UpdateMap();
        }

        // Create and return a new operator based on the selected type and  update counters
        private MechanicalOperator CreateOperator(int operatorType, int Xposition, int Yposition)
        {
            switch (operatorType)
            {
                case 1:
                    M8 m8 = new M8(Xposition, Yposition);
                    Map.M8Counter++;
                    return m8;
                case 2:
                    K9 k9 = new K9(Xposition, Yposition);
                    Map.K9Counter++;
                    return k9;
                case 3:
                    UAV uav = new UAV(Xposition, Yposition);
                    Map.UAVCounter++;
                    return uav;
                default:
                    return null;
            }
        }

        // Display success/failure message
        private void DisplayCreationMessage(MechanicalOperator newOperator)
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            if (newOperator != null)
            {
                Console.WriteLine($"Added operator {newOperator.Id}.");
            }
            else
            {
                Console.WriteLine("Failed.");
            }

            Console.ReadKey();
        }

        private void RemoveOperator()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            H++;


            int indexer = Convert.ToInt32(selectedHQ);
      
            // Display a list of operators corresponding to the selected headquarters
            DisplayOperatorsInHeadquarters(indexer);

            Console.SetCursorPosition(W, H);
            Console.Write("Enter Operator Id to remove: ");
            string operatorId = Console.ReadLine();

            // Get headquarters location
            int Xposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationX;
            int Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;

            // Find and remove the operator
            RemoveOperatorFromHeadquarters(indexer, operatorId);

            Console.ReadKey();
            UpdateMap();
        }

        // Display a list of operators corresponding to the selected headquarters
        private void DisplayOperatorsInHeadquarters(int indexer)
        { 
            List<MechanicalOperator> operators = Map.GetInstance().HQList[indexer - 1].Operators;
            foreach (MechanicalOperator op in operators)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine(op.Id + $" -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY}");
                H++;
            }
        }

        private void RemoveOperatorFromHeadquarters(int indexer, string operatorId)
        {
            // Find the operator to remove
            MechanicalOperator removeOp = Map.GetInstance().HQList[indexer - 1].Operators.FirstOrDefault(op => op.Id == operatorId);

            // Check if the operator exists and remove it
            if (removeOp != null)
            {
                Location toDelete = removeOp.LocationP;
                Map.Grid[toDelete.LocationX, toDelete.LocationY].OperatorsInNode.Remove(removeOp);
                Map.GetInstance().HQList[indexer - 1].Operators.Remove(removeOp);

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
        }

        //Handles HQ inputs safely
        private string GetValidHQSelection()
        {
            int maxHQ = Map.GetInstance().HeadquarterCounter;
            int selected = 0;

            Console.SetCursorPosition(W, H);
            Console.Write($"     Pick an HQ (1-{maxHQ}): ");

            bool continueLoop = true;

            while (continueLoop)
            {
                if (!int.TryParse(Console.ReadLine(), out selected))
                {
                    Console.SetCursorPosition(W, H);
                    Console.Write($"Invalid input. Please enter a valid number: ");
                }
                else if (selected >= 1 && selected <= maxHQ)
                {
                    continueLoop = false;
                }
                else
                {
                    Console.SetCursorPosition(W, H);
                    Console.Write($"Invalid selection. Please enter a number between 1 and {maxHQ}: ");
                }
            }

            return selected.ToString();
        }

        //Handles option inputs safely
        private string GetValidOptionSelection()
        {
            int selected = 0;

            Console.SetCursorPosition(W, H);
            Console.Write($"     Pick an Option ");

            bool continueLoop = true;

            while (continueLoop)
            {
                if (!int.TryParse(Console.ReadLine(), out selected))
                {
                    Console.SetCursorPosition(W, H);
                    Console.Write($"Invalid input. Please enter a valid number: ");
                }
                else if (selected == 1)
                {
                    continueLoop = false;
                }
                else if (selected == 100)
                {
                    Exit();
                }
                else if (selected == 200)
                {
                    ShowSavedGames();
                }
                else
                {
                    Console.SetCursorPosition(W, H);
                    Console.Write($"Invalid selection. ");
                }
            }

            return selected.ToString();
        }

        //Asks for movement safety to determine search pattern
        public bool AskForSafety()
        {
            bool safety = false;
            int search;
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);

            Console.Write("Enter 1 for safe or 2 for optimal pathfinding: ");
            while (!int.TryParse(Console.ReadLine(), out search) || (search != 1 && search != 2))
            {
                Console.Write("Invalid input. Please enter 1 for safe or 2 for optimal pathfinding:");
            }
            if (search == 1) safety = true;
            return safety;
        }


        //GUI Related functions
        private void ClearMenuRemains()
        {
            GetConsoleSizeAfterMap();
            for (int i = 0; i < 10; i++)  //CAMBIAR A 30 PRE ENTREGA+
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine("                                                                                                 ");
                H++;
            }
        }

        private void UpdateMap()
        {
            Console.SetCursorPosition(0, 0);
            Map.GetInstance().PrintMap();
        }

        //Gets position to print menu in
        public void GetConsoleSizeAfterMap()
        {
            W = 3;
            H = Map.MapSize + 5;
        }


        //SQL Related functions
        private void ShowSavedGames()
        {
            SaveOrLoadGame saver = new SaveOrLoadGame();
            List<string> savedGames = saver.GetSavedGames();

            if (savedGames.Count > 0)
            {
                Console.WriteLine("Saved Games:");
                for (int i = 0; i < savedGames.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {savedGames[i]}");
                }
                Console.WriteLine("Enter the number of the saved game to load (or any other key to cancel):");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= savedGames.Count)
                {
                    // Usuario seleccionó un juego válido, ahora carguemos el juego
                    string selectedGameName = savedGames[selectedIndex - 1];
                    Map loadedMap = saver.LoadSpecificGame(selectedGameName);

                    if (loadedMap != null)
                    {
                        Console.WriteLine($"Loaded saved game: {selectedGameName}");

                        loadedMap.PrintMap();
                    }
                    else
                    {
                        Console.WriteLine($"Failed to load saved game: {selectedGameName}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection. Cancelling operation.");
                }
            }
            else
            {
                Console.WriteLine("No saved games found.");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

        }

    }
}