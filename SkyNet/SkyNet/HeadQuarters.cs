using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class HeadQuarters
    {
        public List<MechanicalOperator> Operators { get; set; }
        public Location LocationHeadQuarters { get; set; }

        public HeadQuarters(int x, int y)
        {
            Operators = new List<MechanicalOperator>();
            LocationHeadQuarters = new Location(x,y);
            GenerateRandomAmountOfOperators();
        }


        private void GenerateRandomAmountOfOperators()
        {

            Random x = new Random();
            int OpAmount = x.Next(0, 11);
            for (int i = 0; i < OpAmount; i++)
            {
                Random rng = new Random();
                int generatedType = rng.Next(1, 4);
                int Xposition =rng.Next(0,  100);//esto es para testear
                int Yposition = rng.Next(0, 100);
                if (generatedType == 1)
                {
                    Operators.Add(new M8(Xposition, Yposition));
                    Map.GetInstance().Grid[Xposition, Yposition].OperatorsInNode.Add(new M8(Xposition, Yposition));
                }
                else if (generatedType == 2)
                {
                    Operators.Add(new K9(Xposition, Yposition));
                    Map.GetInstance().Grid[Xposition, Yposition].OperatorsInNode.Add(new K9(Xposition, Yposition));
                }
                else
                {
                    Operators.Add(new UAV(Xposition, Yposition));
                    Map.GetInstance().Grid[Xposition, Yposition].OperatorsInNode.Add(new UAV(Xposition, Yposition));
                }
            }
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
