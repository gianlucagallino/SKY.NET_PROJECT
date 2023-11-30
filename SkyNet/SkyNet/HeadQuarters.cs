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
            Random rng = new Random();
            double OpAmount = rng.Next(0, 11);
            for (int i = 0; i<OpAmount; i++)
            {
                double generatedType = rng.Next(1, 4);
                int Xposition= rng.Next(0,  Map.GetInstance().MapSize-1);
                int Yposition = rng.Next(0, Map.GetInstance().MapSize - 1);
                if (generatedType == 1)
                {
                    Operators.Add(new M8(Xposition, Yposition));
                    // aca habria que agregar el operador al nodo Map.GetInstance().Grid[Xposition, Yposition].OperatorsInNode.Add()
                }
                else if (generatedType == 2)
                {
                    Operators.Add(new K9(Xposition, Yposition));
                    // aca habria que agregar el operador al nodo Map.GetInstance().Grid[Xposition, Yposition].OperatorsInNode.Add()
                }
                else
                {
                    Operators.Add(new UAV(Xposition, Yposition));
                    // aca habria que agregar el operador al nodo Map.GetInstance().Grid[Xposition, Yposition].OperatorsInNode.Add()
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
