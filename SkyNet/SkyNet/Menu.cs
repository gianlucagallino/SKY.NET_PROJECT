namespace SkyNet
{

    internal class Menu
    {
        private bool menuOptionsFlag;

        public Menu()
        {
            menuOptionsFlag = false;
        }

        public void RunMenu()
        {
            bool isRunning = true;
            //tipos de menu dependiendo de la cantidad de hqs
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

            if (!menuOptionsFlag)
            {
                // Agregar opciones de menú aquí
                menuOptionsFlag = true;
            }

            // Imprimir opciones de menú

            Console.WriteLine(" ------------------------------");
            Console.Write("     Pick an HQ: ");
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
                    AddReserveOperator();
                    break;
                case "6":
                    RemoveReserveOperator();
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
            foreach (MechanicalOperator oper in Operators)
            {
                Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}");
            }
            Console.ReadLine();
        }

        private void ShowOperatorStatusAtLocation()
        {
            Console.Clear();
            Console.WriteLine("Enter location coordinates: ");

            Console.WriteLine($"Operator Status at {locationName}:");
            foreach MechanicalOperator oper in Operators.Where(op => op.LocationP.LocationId.ToString() == locationName)) //esto por que es name?? no se ni por que tienen nombre
            {
                Console.WriteLine($"Operator Name: {oper.Id}, Status: {oper.Status}");
            }
            Console.ReadLine();
        }

        private void TotalRecall()
        {
            Console.Clear();
            Console.WriteLine("Performing total recall...");
            foreach (MechanicalOperator oper in Operators)
            {
                oper.LocationP = new Location { LocationId = localizacioncuartelquellama };  // aca me falta
            }
            Console.WriteLine("All operators recalled to Headquarters.");
            Console.ReadLine();
        }

        private void SelectOperator()
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

        private void AddReserveOperator()
        {
            Console.Clear();
            Console.WriteLine("Enter reserve operator details: ");
            string operatorDetails = Console.ReadLine();

            var newOperator = new ConcreteMechanicalOperator
            {
                Id = operatorDetails,
                Status = "Reserve",
                LocationP = new Location { } /* Inicializa las propiedades de Location si es necesario o ver cuales eran */
            };

            operators.Add(newOperator);

            Console.WriteLine($"Adding reserve operator: {newOperator.Id}");
            Console.ReadLine();
        }

        private void RemoveReserveOperator()
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
    }
}

