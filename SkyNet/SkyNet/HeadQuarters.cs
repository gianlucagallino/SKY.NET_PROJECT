using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class HeadQuarters
    {
        //HeadQuarters utiliza el patron singleton. 

        private List<MechanicalOperator> operators;
        private Location locationHeadQuarters;

        public List<MechanicalOperator> Operators { get; set; }
        public Location LocationHeadQuarters { get; set; }


        //este constructor es temporal, debemos mejorarlo al implementar el mapa. 
        private HeadQuarters()
        {
            Operators = new List<MechanicalOperator>();
            LocationHeadQuarters = new Location();
        }

        private static HeadQuarters _instance;

        public static HeadQuarters GetInstance()
        {
            if (_instance == null) { _instance = new HeadQuarters(); }
            return _instance;

        }
        public void ShowOperatorStatus()
        {
            foreach (MechanicalOperator op in operators)
            {
                Console.WriteLine(op.Status);
            }
        }
        public void ShowOperatorStatusAtLocation(Location loc)
        {
            foreach (MechanicalOperator op in operators)
            {
                if (op.LocationP == loc)
                {
                    Console.WriteLine(op.Status);
                }
            }
        }
        public void TotalRecall()
        {
            foreach (MechanicalOperator op in operators)
            {
                op.MoveTo(locationHeadQuarters);
            }
        }
        //esto depende para donde lo encaremos. 
        /* public MechanicalOperator SelectOperator(string id)
         {
             return ;
         }*/

        public void AddReserveOperator(MechanicalOperator oper)
        {
            operators.Add(oper);
        }
        public void RemoveReserveOperator(MechanicalOperator oper)
        {
            operators.Remove(oper);
        }


    }
}
