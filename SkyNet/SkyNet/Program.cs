using SkyNet.Entidades.Mapa;
using SkyNet.Menu;

namespace SkyNet
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Introduction.GetInstance().Play();
            Map.GetInstance().PrintMap();
            MenuClass menu = new MenuClass();
            menu.RunMenu();
        }
    }
}


