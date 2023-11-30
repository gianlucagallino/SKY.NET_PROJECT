using SkyNet.Entidades.Mapa;
using SkyNet.Entidades.Operadores;

namespace SkyNet.Menu
{
    internal class Menu
    {
        private bool menuOptionsFlag;
        private string selectedHQ;

        public Menu()
        {
            menuOptionsFlag = false;
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
            Console.Clear();
            Console.WriteLine(" _______________________________");
            Console.WriteLine("|        Management Menu        | ");
            Console.WriteLine(" ------------------------------- ");

            int conta = 1;
            for (int i = 0; i < Map.GetInstance().HeadquarterCounter; i++)
            {
                Console.WriteLine($"{conta}. HeadQuarter {conta}");
            }

            Console.WriteLine(" ------------------------------");


            // Read and set the selected HQ
            selectedHQ = GetValidHQSelection();

            Console.Clear();
            Console.WriteLine(" _______________________________");
            Console.WriteLine("|        Operator Menu          | ");
            Console.WriteLine(" ------------------------------- ");
            Console.WriteLine("1. Show Operator Status");
            Console.WriteLine("2. Show Operator Status at Location");
            Console.WriteLine("3. Total Recall");
            Console.WriteLine("4. Select Operator");
            Console.WriteLine("5. Add New Operator");
            Console.WriteLine("6. Destroy Operator");
            Console.WriteLine(" ------------------------------");
            Console.Write("     Pick an option: ");
        }


        private string GetValidHQSelection()
        {
            double maxHQ = Map.GetInstance().HeadquarterCounter;

            while (true) //esto pasarlo a un booleano
            {
                Console.Write($"     Pick an HQ (1-{maxHQ}): ");
                string input = Console.ReadLine(); //hay que hacer verificacion

                if (int.TryParse(input, out int selected) && selected >= 1 && selected <= maxHQ)
                {
                    return input;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Invalid selection. Please enter a number between 1 and {maxHQ}.");
                }
            }
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
                    Console.ReadLine();
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
            Console.ReadLine();
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
            Console.ReadLine();
        }

        private void SelectOperator() // Este no tiene nada hecho, hay que armar las 3 subopciones. Idealmente, en todo metodo tambien debemos tener un mensaje de si falla encontrar el operador o algo similar
        {/*
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
        }*/
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
            }
            else if (operatorType == 2)
            {
                K9 k9 = new K9(Xposition, Yposition);
                Map.GetInstance().HQList[indexer].Operators.Add(k9);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(k9);
            }
            else
            {
                UAV uav = new UAV(Xposition, Yposition);
                Map.GetInstance().HQList[indexer].Operators.Add(uav);
                Map.Grid[Xposition, Yposition].OperatorsInNode.Add(uav);
            }
            Console.WriteLine("Added!");
        }

        private void RemoveOperator()
        {
            Console.Clear();
            Console.WriteLine("Enter Operator ID to remove: ");
            string operatorId = Console.ReadLine(); 
            //verificar que exista
            var removeOp=operators.First(op => op.Id==operadorId )
            if (removeOp != null)
            {
                operators.Remove(reserveOp);
                Console.WriteLine($"Removing reserve operator: {reserveOp.Id}");
            }
            else
            {
                Console.WriteLine($"Reserve operator {operatorName} not found.");
            }
            Console.ReadLine();
        }
    }
}