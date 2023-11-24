using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Program
    {
        static void Main(string[] args)

        {
            // Habria que agarrar el menu del proyecto anterior, y pasarlo a command. 
            // El codigo anterior deberia estar disponible en el segundo repo que hizo cata. 

            Introduction.GetInstance().Play();
            //Menu();

        }
    }
}
