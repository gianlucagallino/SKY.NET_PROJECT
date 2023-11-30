namespace SkyNet
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

            int OpAmount = rng.Next(1, 15); // 15 operator cap, para no explotar el mapa. 
            for (int i = 0; i < OpAmount; i++)
            {
                bool inLoop = true;
                while (inLoop)
                {
                    int generatedType = rng.Next(1, 4);
                    int Xposition = rng.Next(0, Map.MapSize); //We use static variables in this method, instead of GetInstance(), as by
                    int Yposition = rng.Next(0, Map.MapSize); // this point the instance is still being built, so it evaluates to null, and breaks stuff.
                    if (generatedType == 1 && !CheckWater(Xposition, Yposition))
                    {
                        Operators.Add(new M8(Xposition, Yposition));
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(new M8(Xposition, Yposition));
                        inLoop = false;
                    }
                    else if (generatedType == 2 && !CheckWater(Xposition, Yposition))
                    {
                        Operators.Add(new K9(Xposition, Yposition));
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(new K9(Xposition, Yposition));
                        inLoop = false;
                    }
                    else if (generatedType == 3) //CheckWater is not necessary, UAV's can fly. 
                    {
                        Operators.Add(new UAV(Xposition, Yposition));
                        Map.Grid[Xposition, Yposition].OperatorsInNode.Add(new UAV(Xposition, Yposition));
                        inLoop = false;
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
