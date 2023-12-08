namespace SkyNet.Menu
{
    internal class Introduction
    {

        //This class contains GUI code related to introduction.

        public int W { get; set; }
        public int H { get; set; }
        private Introduction()
        { }

        private static Introduction _instance;
        public static Introduction GetInstance()
        {
            if (_instance == null) //si no existe, se implementa
            {
                _instance = new Introduction();
            }
            return _instance;
        }

        public void GetConsoleSizeCenter()
        {
            W = Console.WindowWidth / 4;
            H = Console.WindowHeight / 3;
        }

        public void Play()
        {
            ShowResolutionWarning();
            BlinkTitle(6, 90);
            TerrainTutorial();
            OperatorTutorial();
            HeadquarterTutorial();
            SaveGameTutorial();
            Console.Clear();
        }

        private void ShowResolutionWarning()
        {
            Console.ForegroundColor = ConsoleColor.White;
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*         Please, turn fullscreen on, or things WILL break.       *");
            Console.SetCursorPosition(W, H);
            Console.ForegroundColor = ConsoleColor.White;
            H++;
            Console.WriteLine("|________________________________________________________________|");
            Console.SetCursorPosition(W, H);
            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("                    PRESS ENTER TO CONTINUE                      ");
            Console.ReadKey();
        }

        private void BlinkTitle(double blinkCount, int blinkDelay)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < blinkCount; i++)
            {
                Console.CursorVisible = false;
                GetConsoleSizeCenter();
                Thread.Sleep(blinkDelay);
                Console.Clear();
                Thread.Sleep(blinkDelay);
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
            }

            H += 3;
            Console.SetCursorPosition(W, H);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t                                    PRESS ANY KEY TO CONTINUE                                              ");
            Console.ReadKey();
            Console.Clear();
        }

        private void TerrainTutorial()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            GetConsoleSizeCenter();
            Console.CursorVisible = false;
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t ________________________________________________________________ ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                      Terrain Tutorial:                         |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| Map and Grid:                                                  |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| The map has a square resolution chosen by the player.          |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| It also has six terrain types:                                 |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t|                  White squares are neutral.                    |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t|     Green squares are Dumpsters which can damage operators.    |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t|     Blue squares are Lakes only traversable by flying units.   |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t|  Yellow squares are E-Dumpsters which reduce battery capacity. |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\t|   Pink squares are HeadQuarters, home base of some operators.  |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\t|    Brown Squares are Recyclers, where operators can recharge.  |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t|________________________________________________________________|");
            Console.SetCursorPosition(W, H);
            Console.ForegroundColor = ConsoleColor.White;
            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("\t                    Press any key to continue.                   ");
            Console.ReadKey();
            Console.Clear();
        }

        private void OperatorTutorial()
        {
            Console.ForegroundColor = ConsoleColor.White;
            GetConsoleSizeCenter();
            Console.CursorVisible = false;
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t ________________________________________________________________ ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                      Operator Tutorial:                        |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                                                                |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| Operators will show up in their corresponding tiles.           |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| There are 3 types of operator:                                 |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t|    K: K9 units. Ground based, faster than M8s, but weaker.     |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|        M: M8 units. Ground based, slower, but stronger.        |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|     U: UAV units. Flying drones, can travel over water.        |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|           @: Multiple units on the same coordinate.            |");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|________________________________________________________________|");
            Console.SetCursorPosition(W, H);
            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("\t                   Press any key to continue.                ");
            Console.ReadKey();
            Console.Clear();
        }


        private void HeadquarterTutorial()
        {
            Console.ForegroundColor = ConsoleColor.White;
            GetConsoleSizeCenter();
            Console.CursorVisible = false;
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t ________________________________________________________________ ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                    Headquarter Tutorial:                        |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                                                                |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| Each headquarter owns certain operators.                       |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| Headquarters can only give out orders to its own operators.    |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| To figure out ownership, go to your desired HQ and press on    |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| show operator status.                                          |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|________________________________________________________________|");
            Console.SetCursorPosition(W, H);
            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("\t                   Press any key to continue.                ");
            Console.ReadKey();
            Console.Clear();
        }

        private void SaveGameTutorial()
        {
            Console.ForegroundColor = ConsoleColor.White;
            GetConsoleSizeCenter();
            Console.CursorVisible = false;
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t ________________________________________________________________ ");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                     Game save Tutorial:                        |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|                                                                |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| To save your game to a file, press on the corresponding        |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| option in the menu. To load a file, choose your desired file.  |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t| You CANT continue a saved game, but you can see how it ended.  |");
            Console.SetCursorPosition(W, H);
            H++;
            Console.WriteLine("\t|________________________________________________________________|");
            Console.SetCursorPosition(W, H);
            H += 2;
            Console.SetCursorPosition(W, H);
            Console.WriteLine("\t                   Press any key to continue.                ");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
