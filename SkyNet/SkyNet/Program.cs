namespace SkyNet
{

    internal class Program
    {
        private static bool isRunning = true;
        private static List<MechanicalOperator> operators = new List<MechanicalOperator>();
        //private static Menu menu;

        static void Main(string[] args)
        {
            Introduction.GetInstance().Play();
            Map.GetInstance().PrintMap();
            Menu menu = new Menu();
            menu.RunMenu();
        }
    }
}


