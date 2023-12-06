using SkyNet.Entidades.Mapa;
using System;
using System.IO;
using System.Text.Json;

namespace SkyNet.Menu
{
    public class SaveOrLoadGame
    {
        private const string SaveFolderPath = "SavedGames"; // Cambiar a la ruta real

        public void SaveGame()
        {
            bool ok = true;
            while (ok)
            {
                try
                {
                    // De esta manera, el usuario elige el nombre del archivo a guardar
                    Console.WriteLine("Enter the name for the saved game:");
                    string gameName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(gameName))
                    {
                        Console.WriteLine("Invalid game name. The game was not saved.");
                    }
                    else
                    {
                        // Crear la carpeta si no existe
                        Directory.CreateDirectory(SaveFolderPath);

                        // Ruta completa del archivo
                        string filePath = Path.Combine(SaveFolderPath, $"{gameName}.json");

                        Map gameMap = Map.GetInstance();
                        string gameJson = JsonSerializer.Serialize(gameMap);
                        File.WriteAllText(filePath, gameJson);
                        Console.WriteLine("Game saved successfully");
                        Console.WriteLine($"File saved in: {Path.GetFullPath(filePath)}");
                        ok = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving the game: {ex.Message}");
                }
            }
        }

        public Map LoadGame(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The selected file does not exist.");
                return null;
            }

            try
            {
                string gameJson = File.ReadAllText(filePath);
                Map loadedMap = JsonSerializer.Deserialize<Map>(gameJson);
                Console.WriteLine("Game loaded successfully");
                return loadedMap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading the game: {ex.Message}");
                return null;
            }
        }

        public Map LoadSpecificGame(string gameName)
        {
            string filePath = Path.Combine(SaveFolderPath, $"{gameName}.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"The saved game '{gameName}' does not exist.");
                return null;
            }

            Map loadedMap = null;

            try
            {
                string gameJson = File.ReadAllText(filePath);
                //Console.WriteLine($"Contenido de gameJson: {gameJson}");
                loadedMap = JsonSerializer.Deserialize<Map>(gameJson);
                Console.WriteLine($"Game '{gameName}' loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading the game '{gameName}': {ex.Message}");
            }

            return loadedMap;
        }

        public void ShowSavedGames()
        {
            if (Directory.Exists(SaveFolderPath))
            {
                string[] files = Directory.GetFiles(SaveFolderPath, "*.json");
                Console.WriteLine("Saved game files:");

                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.WriteLine("The saved games folder does not exist.");
            }
        }
        public List<string> GetSavedGames()
        {
            string folderPath = "SavedGames";

            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath, "*.json")
                                .Select(Path.GetFileNameWithoutExtension)
                                .ToList();
            }
            else
            {
                Console.WriteLine("No saved games folder found.");
                return new List<string>();
            }
        }


        //De esta manera nueva de hacer LoadGame no retorna nada en medio del bucle.
        /* public static void LoadGame()
         {
             bool isValidSelection = true;
             List<Map> loadedGames = new List<Map>();

             while (isValidSelection)
             {
                 try
                 {
                     string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json");
                     LoadGamesFromFiles(files, loadedGames);

                     if (loadedGames.Count == 0)
                     {
                         Console.WriteLine("No saved games available.");
                         continue;
                     }

                     DisplaySavedGames(loadedGames);

                     Console.WriteLine("Enter the number of the game you want to load");
                     if (TryGetSelectedGameIndex(Console.ReadLine(), loadedGames.Count, out int selectedGameIndex) && selectedGameIndex > 0)
                     {

                         Map selectedGame = loadedGames[selectedGameIndex - 1];

                     }
                     else
                     {
                         Console.WriteLine("Invalid input. Please enter a valid number.");
                         continue;
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error loading the game: {ex.Message}");
                     Console.WriteLine("An error occurred while loading the game. Please try again.");
                 }
                 finally
                 {
                     isValidSelection = false; // Esto evita que el bucle continúe indefinidamente
                 }
             }
         }

         /*static Map LoadGame()
         {
             bool isValidSelection = true;
             List<Map> loadedGames = new List<Map>();

             while (isValidSelection)
             {
                 try
                 {
                     string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json");
                     LoadGamesFromFiles(files, loadedGames);

                     if (loadedGames.Count == 0)
                     {
                         Console.WriteLine("No saved games avilable.");
                         continue; //en teoria con esto vuelve al princiio del bucle para q el usuario pueda seleccionar otro numero
                     }
                     DisplaySavedGames(loadedGames);

                     Console.WriteLine("Enter the number of the game you wanto to load");
                     if (TryGetSelectedGameIndex(Console.ReadLine(), loadedGames.Count, out int selectedGameIndex) && selectedGameIndex > 0)
                     {
                         return loadedGames[selectedGameIndex - 1];
                     }
                     else
                     {
                         Console.WriteLine("Invalid input. Please enter a valid number.");
                         continue;
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error loading the game: {ex.Message}");
                     Console.WriteLine("An error occurred while loading the game. Please try again.");
                 }

             }
             return null;
         }*/

        /*static void LoadGamesFromFiles(string[] files, List<Map> loadedGames)
        {
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    string content = File.ReadAllText(file);
                  //  Map loadedGame = Map.DeserializeFromJson(content);
               //     loadedGames.Add(loadedGame);
                    Console.WriteLine($"Game: {loadedGames.Count}, loaded successfully");
                }
                else
                {
                    Console.WriteLine("No saved game found");
                }
            }
        }

        static bool TryGetSelectedGameIndex(string input, int maxIndex, out int selectedGameIndex)
        {
            return int.TryParse(input, out selectedGameIndex) && selectedGameIndex > 0 && selectedGameIndex <= maxIndex;
        }


        static void DisplaySavedGames(List<Map> games)
        {
            Console.WriteLine("Saved Games:");
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Game {i + 1}");
            }
        }*/
    }

}
