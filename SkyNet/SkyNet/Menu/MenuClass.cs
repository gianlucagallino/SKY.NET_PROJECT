using SkyNet.Entidades;
using SkyNet.Entidades.Mapa;
using SkyNet.Entidades.Operadores;
using System;

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
            W = 3;
            H = Map.MapSize + 5;
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
            Console.WriteLine(" --------------------------------  ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.Write("     Pick an option: ");
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
                    Console.WriteLine("Invalid input. (Press any key) ");
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
            if (Map.GetInstance().HQList[indexer - 1].Operators.Count == 0)
            {

                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("No operators belong to this HQ.");
            }
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer - 1].Operators)
            {
                H++;
                Console.SetCursorPosition(W, H);
                Console.Write($"Id: {oper.Id}, Status: {oper.Status}, X: {oper.LocationP.LocationX}, Y: {oper.LocationP.LocationY}, " + oper.ToString());

            }
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private void ShowOperatorStatusAtLocation()
        {
            int Xinput;
            int Yinput;

            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.Write("Enter coordinates: ");
            H++;
            //INPUT
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter an X location between 0 and {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out Xinput) || Xinput < 0 || Xinput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter an X location between 0 and {Map.MapSize - 1}: ");
            }
            H++;
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter a Y location between 0 and {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out Yinput) || Yinput < 0 || Yinput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter a Y location between 0 and {Map.MapSize - 1}: ");

            }
            H++;
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.WriteLine($"Operator Status at those coordinates:");
            H++;
            int indexer = Convert.ToInt32(selectedHQ);
            ClearMenuRemains();
            if (Map.Grid[Xinput, Yinput].OperatorsInNode.Count == 0)
            {

                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("No operators here!");
            }
            else
            {
                GetConsoleSizeAfterMap();
                foreach (MechanicalOperator oper in Map.Grid[Xinput, Yinput].OperatorsInNode)
                {


                    Console.SetCursorPosition(W, H);
                    Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status},  X: {oper.LocationP.LocationX}, Y: {oper.LocationP.LocationY}, " + oper.ToString());
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
            H++;
            Thread.Sleep(2000);
            bool safety = AskForSafety();
            int indexer = Convert.ToInt32(selectedHQ);
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            foreach (MechanicalOperator oper in Map.GetInstance().HQList[indexer - 1].Operators)
            {
                Console.WriteLine($"{oper.Id}: ");
                H++;
                Console.SetCursorPosition(W, H);
                oper.MoveTo(Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters, safety, indexer - 1, oper.Id);
                H++;
                Console.SetCursorPosition(W, H);
            }
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            UpdateMap();
        }

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
            //Trae una lista del cuartel seleccionado
            List<MechanicalOperator> operators = Map.GetInstance().HQList[indexer - 1].Operators;
            foreach (MechanicalOperator op in operators)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"{op.Id} -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY} ");
                H++;
            }
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
                Console.WriteLine("4. General Order (Repair damages at HQ)");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("5. Battery Change");
                H++;
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Select option: ");
                H++;
                Console.SetCursorPosition(W, H);
                Console.Write("Enter an integer from 1 to 5:");
                while (!int.TryParse(Console.ReadLine(), out subOption) || subOption < 1 || subOption > 5)
                {

                    Console.SetCursorPosition(W, H);
                    Console.Write("Invalid input. Please enter an integer from 1 to 5:");
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

        private void ChangeBatteryMenu(MechanicalOperator selectedOperator)
        {
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();

            if (selectedOperator.DamageSimulatorP.PerforatedBattery)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Performing Battery Change...");
                selectedOperator.BatteryChange(Map.Grid, safety, whatHeadquarter, operatorId);
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
                Console.WriteLine("Battery is not perforated. Battery cannot be changed");
            }
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            UpdateMap();
        }

        private void GeneralOrderMenu(MechanicalOperator selectedOperator)
        {
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();

            if (selectedOperator.BusyStatus == false)
            {
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine("Executing General Order...");
                Thread.Sleep(2000);
                selectedOperator.GeneralOrder(Map.Grid, operatorId, whatHeadquarter, safety);
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
                Console.WriteLine("Operator is busy. Execution failed.");
            }
            H++;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            UpdateMap();
        }

        private void MoveToMenu(MechanicalOperator selectedOperator)
        {
            int Xinput;
            int Yinput;
            bool safety;
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter an X location from 0 to {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out Xinput) || Xinput < 0 || Xinput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter an integer between 0 and {Map.MapSize - 1}: ");
            }
            H++;
       
            Console.SetCursorPosition(W, H);
            Console.Write($"Enter a Y location from 0 to {Map.MapSize - 1}: ");
            while (!int.TryParse(Console.ReadLine(), out Yinput) || Yinput < 0 || Yinput >= Map.MapSize)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid input. Enter a number from 0 to {Map.MapSize - 1}: ");
            }
            Location location = new Location(Xinput, Yinput);
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            safety = AskForSafety();
            H++;
            Console.SetCursorPosition(W, H);
            int indexer = Convert.ToInt32(selectedHQ);
            selectedOperator.MoveTo(location, safety, indexer - 1, selectedOperator.Id);
            H++;
            UpdateMap();
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
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();


            //LISTA DE TODOS LOS OPERADORES DE TODOS LOS CUARTELES
            foreach (HeadQuarters hq in Map.GetInstance().HQList)
            {
                List<MechanicalOperator> operators = hq.Operators;

                foreach (MechanicalOperator op in operators)
                {
                    Console.WriteLine($"{op.Id} -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY} - {op.Battery.CurrentChargePercentage}");
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
                Console.WriteLine("Enter battery percentage to transfer: ");
                double percentage = Convert.ToDouble(Console.ReadLine()); //verificar negatividad, si es suficiente, etc. 

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
            string operatorId = selectedOperator.Id;
            int whatHeadquarter = Convert.ToInt32(selectedHQ) - 1;
            bool safety = AskForSafety();


            //LISTA DE TODOS LOS OPERADORES DE TODOS LOS CUARTELES
            foreach (HeadQuarters hq in Map.GetInstance().HQList)
            {
                List<MechanicalOperator> operators = hq.Operators;

                foreach (MechanicalOperator op in operators)
                {
                    Console.WriteLine(op.Id + $" -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY} - " + op.Battery.CurrentChargePercentage);
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
                double amountKG = Convert.ToDouble(Console.ReadLine()); //aca hay que ver si excede la carga que tiene, o si es negativo, o si el otro puede aceptar esa carga

                selectedOperator.TransferLoad(destinationOperator, amountKG, safety, whatHeadquarter, operatorId);
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
            ClearMenuRemains();
            GetConsoleSizeAfterMap();

            int indexer = Convert.ToInt32(selectedHQ);
            int Xposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationX; 
            int Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;
            int operatorType;
            Console.SetCursorPosition(W, H);
            Console.Write("Enter type number (1: M8, 2: K9, 3: UAV): ");
            while (!int.TryParse(Console.ReadLine(), out operatorType) || (operatorType < 1 || operatorType > 3))
            {
                Console.SetCursorPosition(W, H);
                Console.Write("Invalid input. Enter type number (1: M8, 2: K9, 3: UAV): ");
            }

            if (operatorType == 1)
            {
                M8 m8 = new M8(Xposition, Yposition);
                Map.GetInstance().HQList[indexer - 1].Operators.Add(m8);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(m8);
                Map.M8Counter++;
                ClearMenuRemains();
                GetConsoleSizeAfterMap();
                Console.SetCursorPosition(W, H);
                Console.WriteLine($"Added operator {m8.Id}.");
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
                Console.WriteLine($"Added operator {k9.Id}.");
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
                Console.WriteLine($"Added operator {uav.Id}.");
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
            UpdateMap();

        }

        private void RemoveOperator()
        {
            ClearMenuRemains();
            GetConsoleSizeAfterMap();
            Console.SetCursorPosition(W, H);
            int indexer = Convert.ToInt32(selectedHQ);
            H++;
            //A list of operators is displayed, corresponding to that hq.
            List<MechanicalOperator> operators = Map.GetInstance().HQList[indexer - 1].Operators;
            foreach (MechanicalOperator op in operators)
            {
                Console.SetCursorPosition(W, H);
                Console.WriteLine(op.Id + $" -  X: {op.LocationP.LocationX}, Y: {op.LocationP.LocationY}");
                H++;
            }
            Console.SetCursorPosition(W, H);
            Console.Write("Enter Operator Id to remove: ");
            string operatorId = Console.ReadLine();

            int Xposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationX; 
            int Yposition = Map.GetInstance().HQList[indexer - 1].LocationHeadQuarters.LocationY;
            //verificar que exista
            MechanicalOperator removeOp = Map.GetInstance().HQList[indexer - 1].Operators.FirstOrDefault(op => op.Id == operatorId);
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
            Console.ReadKey();
            UpdateMap();
        }





        private string GetValidHQSelection()
        {
            int maxHQ = Map.GetInstance().HeadquarterCounter;
            string input = "Error.";
            bool isRunning = true;
            int selected;


            Console.SetCursorPosition(W, H);
            Console.Write($"     Pick an HQ (1-{maxHQ}): ");
            while (!int.TryParse(Console.ReadLine(), out selected) || selected < 1 || selected > maxHQ)
            {
                Console.SetCursorPosition(W, H);
                Console.Write($"Invalid selection. Please enter a number between 1 and {maxHQ}: ");
            }

            return selected.ToString();
        }

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
            for (int i = 0; i < 30; i++)
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
    }
}