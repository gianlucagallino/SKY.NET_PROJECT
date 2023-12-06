using SkyNet.Entidades.Operadores;

namespace SkyNet.Entidades.Mapa
{
    public class HeadQuarters
    {
        // Random number generator for operator generation
        private Random rng;

        // List to store MechanicalOperators
        public List<MechanicalOperator> Operators { get; set; }

        // Location of the headquarters on the map
        public Location LocationHeadQuarters { get; set; }


        // Constructor initializes the headquarters and generates operators
        public HeadQuarters(int x, int y)
        {
            Operators = new List<MechanicalOperator>();
            LocationHeadQuarters = new Location(x, y);
            rng = new Random();
            GenerateRandomAmountOfOperators();
        }

        // Generates a random number of operators and adds them to the list
        private void GenerateRandomAmountOfOperators()
        {
            int OpAmount = rng.Next(1, 15);

            for (int i = 0; i < OpAmount; i++)
            {
                bool inLoop = true;

                while (inLoop)
                {
                    int generatedType = rng.Next(1, 4);
                    int Xposition = rng.Next(0, Map.MapSize);
                    int Yposition = rng.Next(0, Map.MapSize);

                    // Checks the type of operator and terrain type before adding to the list
                    if (generatedType == 1 && !CheckWater(Xposition, Yposition))
                    {
                        M8 m8 = new M8(Xposition, Yposition);
                        Operators.Add(m8);
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(m8);
                        inLoop = false;
                        Map.M8Counter++;
                       // Console.WriteLine(m8.ToString()); para testear
                    }
                    else if (generatedType == 2 && !CheckWater(Xposition, Yposition))
                    {
                        K9 k9 = new K9(Xposition, Yposition);
                        Operators.Add(k9);
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(k9);
                        inLoop = false;
                        Map.K9Counter++;
                        //Console.WriteLine(k9.ToString());
                    }
                    else if (generatedType == 3)
                    {
                        UAV uav = new UAV(Xposition, Yposition);
                        Operators.Add(uav);
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(uav);
                        inLoop = false;
                        Map.UAVCounter++;
                       // Console.WriteLine(uav.ToString()); para testear
                    }
                }
            }
        }

        // Checks if the given location is water (TerrainType == 2)
        private bool CheckWater(int x, int y)
        {
            if (Map.Grid[x, y].TerrainType == 2) return true;
            return false;
        }

        /* esto deberia ir en codigo? menu? no se. 

        // Displays the status of all operators
        public void ShowOperatorStatus()
        {
            foreach (MechanicalOperator op in Operators)
            {
                Console.WriteLine(op.Status);
            }
        }

        // Displays the status of operators at a specific location
        public void ShowOperatorStatusAtLocation(Location loc)
        {
            foreach (MechanicalOperator op in Operators)
            {
                if (op.LocationP == loc)
                {
                    Console.WriteLine(op.Status);
                }
            }
        }

        // Commands all operators to move back to headquarters
        public void TotalRecall()
        {
            foreach (MechanicalOperator op in Operators)
            {
                op.MoveTo(LocationHeadQuarters);
            }
        }

        // Adds a reserve operator to the list
        public void AddReserveOperator(MechanicalOperator oper)
        {
            Operators.Add(oper);
        }

        // Removes a reserve operator from the list
        public void RemoveReserveOperator(MechanicalOperator oper)
        {
            Operators.Remove(oper);
        }*/
    }
}