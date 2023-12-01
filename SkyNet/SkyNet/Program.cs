using SkyNet.Entidades.Mapa;
using SkyNet.Entidades.Operadores;
using SkyNet.Menu;

namespace SkyNet
{

    internal class Program
    {
        private static bool isRunning = true;
        private static List<MechanicalOperator> operators = new List<MechanicalOperator>();

        static void Main(string[] args)
        {
            //Introduction.GetInstance().Play();
            Map.GetInstance().PrintMap();
            MenuClass menu = new MenuClass();
            menu.RunMenu();
        }
    }
}


