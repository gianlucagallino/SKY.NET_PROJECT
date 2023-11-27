using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Introduction
    {

        //esto es un desastre, hay que hacerlo mejor. Necesita mas funciones, una por ejemplo de aumentar Y position. Esto es una proof of concept.
        private int w;
        private int h;
        public int W { get; set; }
        public int H { get; set; }
        private Introduction()
        {


        }

        private static Introduction _instance;



        public static Introduction GetInstance()
        {
            if (_instance == null) //si no existe, se implementa
            {
                _instance = new Introduction();
            }
            return _instance;
        }

        private void GetConsoleSizeCenter()
        {
            W = Console.WindowWidth / 4;
            H = Console.WindowHeight / 3;
        }

        public void Play()
        {
            ShowResolutionWarning();
            BlinkTitle(6, 90);

        }

        private void ShowResolutionWarning()
        {
            GetConsoleSizeCenter();
            Console.CursorVisible = false;
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine(" ________________________________________________________________ ");
            Console.SetCursorPosition(W, H);

            H++;
            Console.WriteLine("|                                                                |");
            Console.SetCursorPosition(W, H);

            H++;
            Console.WriteLine("|   This experience is made with a fullscreen console in mind.   |");
            Console.SetCursorPosition(W, H);

            H++;
            Console.WriteLine("|           Please, turn fullscreen on before proceeding.        |");
            Console.SetCursorPosition(W, H);

            H++;
            Console.WriteLine("|________________________________________________________________|");
            Console.SetCursorPosition(W, H);

            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("                    PRESS ENTER TO CONTINUE                      ");
            Console.ReadKey();
        }

        private void BlinkTitle(double amount, int time)
        {

            for (int i = 0; i < amount; i++)
            {
                Console.CursorVisible = false;
                GetConsoleSizeCenter();
                Thread.Sleep(time);
                Console.Clear();
                Thread.Sleep(time);
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t    )                                              )                                          ____            ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t ( /(                              )            ( /(                    (              (     |   /            ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t )\\())           (   (       )  ( /(      (     )\\())   )      (   (    )\\         )   )\\ )  |  /         ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t((_)\\   `  )    ))\\  )(   ( /(  )\\()) (   )(   ((_)\\   /((    ))\\  )(  ((_) (   ( /(  (()/(  | /         ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t  ((_)  /(/(   /((_)(()\\  )(_))(_))/  )\\ (()\\    ((_) (_))\\  /((_)(()\\  _   )\\  )(_))  ((_)) |/         ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t / _ \\ ((_)_\\ (_))   ((_)((_)_ | |_  ((_) ((_)  / _ \\ _)((_)(_))   ((_)| | ((_)((_)_   _| | (              ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t| (_) || '_ \\)/ -_) | '_|/ _` ||  _|/ _ \\| '_| | (_) |\\ V / / -_) | '_|| |/ _ \\/ _` |/ _` | )\\           ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t \\___/ | .__/ \\___| |_|  \\__,_| \\__|\\___/|_|    \\___/  \\_/  \\___| |_|  |_|\\___/\\__,_|\\__,_|((_)    ");
                Console.SetCursorPosition(W, H);
                H++;
                Console.WriteLine("\t       |_|                                                                                                    ");
                Console.SetCursorPosition(W, H);
                H++;
            }

            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("\t                                    PRESS ANY KEY TO CONTINUE                                              ");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
