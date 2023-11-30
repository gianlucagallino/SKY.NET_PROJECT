using SkyNet.Entidades.Operadores;

namespace SkyNet.Entidades.Mapa
{
    internal class HeadQuarters
    {
        private Random rng;
        public List<MechanicalOperator> Operators { get; set; }
        public Location LocationHeadQuarters { get; set; }

        public HeadQuarters(int x, int y)
        {
            Operators = new List<MechanicalOperator>();
            LocationHeadQuarters = new Location(x, y);
            rng = new Random();
            GenerateRandomAmountOfOperators();
        }
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

                    if (generatedType == 1 && !CheckWater(Xposition, Yposition))
                    {
                        M8 m8 = new M8(Xposition, Yposition);
                        Operators.Add(m8);
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(m8);
                        inLoop = false;
                        Map.M8Counter++;
                    }
                    else if (generatedType == 2 && !CheckWater(Xposition, Yposition))
                    {
                        K9 k9 = new K9(Xposition, Yposition);
                        Operators.Add(k9);
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(k9);
                        inLoop = false;
                        Map.K9Counter++;
                    }
                    else if (generatedType == 3)
                    {
                        UAV uav = new UAV(Xposition, Yposition);
                        Operators.Add(uav);
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(uav);
                        inLoop = false;
                        Map.UAVCounter++;
                    }
                }
            }
        }

        private bool CheckWater(int x, int y)
        {
            if (Map.Grid[x, y].TerrainType == 2) return true;
            return false;
        }
        public void ShowOperatorStatus()
        {
            foreach (MechanicalOperator op in Operators)
            {
                Console.WriteLine(op.Status);
            }
        }
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
        public void TotalRecall()
        {
            foreach (MechanicalOperator op in Operators)
            {
                op.MoveTo(LocationHeadQuarters);
            }
        }
        //esto depende para donde lo encaremos. 
        /* public MechanicalOperator SelectOperator(string id)
         {
             return ;
         }*/

        public void AddReserveOperator(MechanicalOperator oper)
        {
            Operators.Add(oper);
        }
        public void RemoveReserveOperator(MechanicalOperator oper)
        {
            Operators.Remove(oper);
        }


    }
}
