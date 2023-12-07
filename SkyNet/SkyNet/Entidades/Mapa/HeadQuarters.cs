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

        // Individual creator functions
        private MechanicalOperator CreateM8(int xPosition, int yPosition)
        {
            M8 m8 = new M8(xPosition, yPosition);
            Map.Grid[xPosition, yPosition].OperatorsInNode.Add(m8);
            Map.M8Counter++;
            // Console.WriteLine(m8.ToString()); // Uncomment for testing
            return m8;
        }

        private MechanicalOperator CreateK9(int xPosition, int yPosition)
        {
            K9 k9 = new K9(xPosition, yPosition);
            Map.Grid[xPosition, yPosition].OperatorsInNode.Add(k9);
            Map.K9Counter++;
            // Console.WriteLine(k9.ToString()); // Uncomment for testing
            return k9;
        }

        private MechanicalOperator CreateUAV(int xPosition, int yPosition)
        {
            UAV uav = new UAV(xPosition, yPosition);
            Map.Grid[xPosition, yPosition].OperatorsInNode.Add(uav);
            Map.UAVCounter++;
            // Console.WriteLine(uav.ToString()); // Uncomment for testing
            return uav;
        }


        //Valid position check
        private bool IsValidPosition(int xPosition, int yPosition)
        {
            return xPosition >= 0 && xPosition < Map.MapSize && yPosition >= 0 && yPosition < Map.MapSize;
        }

        // Generates a type for a generated operator
        private MechanicalOperator GenerateRandomOperator(int xPosition, int yPosition, int generatedType)
        {
            switch (generatedType)
            {
                case 1:
                    if (!CheckWater(xPosition, yPosition))
                        return CreateM8(xPosition, yPosition);
                    break;

                case 2:
                    if (!CheckWater(xPosition, yPosition))
                        return CreateK9(xPosition, yPosition);
                    break;

                case 3:
                    return CreateUAV(xPosition, yPosition);
            }

            return null; // Invalid operator type or water position
        }

        // Generates a random number of operators and adds them to the list
        private void GenerateRandomAmountOfOperators()
        {
            int opAmount = rng.Next(1, 15);

            for (int i = 0; i < opAmount; i++)
            {
                bool inLoop = true;

                while (inLoop)
                {
                    int generatedType = rng.Next(1, 4);
                    int xPosition = rng.Next(0, Map.MapSize);
                    int yPosition = rng.Next(0, Map.MapSize);

                    if (IsValidPosition(xPosition, yPosition))
                    {
                        MechanicalOperator newOperator = GenerateRandomOperator(xPosition, yPosition, generatedType);

                        if (newOperator != null)
                        {
                            Operators.Add(newOperator);
                            inLoop = false;
                        }
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
    }
}
