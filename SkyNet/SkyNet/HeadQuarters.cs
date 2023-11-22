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

        private List <MechanicalOperator> operators;
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

        }
        public void ShowOperatorStatusAtLocation(string loc)
        {

        }
        public void TotalRecall()
        {

        }

       /* public MechanicalOperator SelectOperator(string id)
        {
            return ;
        }*/

        public void AddReserveOperator(string opId)
        {

        }
        public void RemoveReserveOperator (string opId)
        {

        }


    }
}
