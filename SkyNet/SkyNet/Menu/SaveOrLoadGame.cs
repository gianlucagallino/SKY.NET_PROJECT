using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SkyNet.Entidades.Mapa;

namespace SkyNet.Menu
{
    public class SaveOrLoadGame
    {
        static void SaveGame()
        {
            try
            {
                //De esta manera el usuario elige el nombre del archivo a guardar
                Console.WriteLine("Enter the name for the saved game:");
                string gameName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(gameName))
                {
                    Console.WriteLine("Invalid game name. The game was not saved.");
                    return;
                }
                string gameJson = Map.GetInstance().SerializeToJson();
                File.WriteAllText($"{gameName}.json", gameJson);
                Console.WriteLine("Game saved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving the game: {ex.Message}");
            }
        }

        //De esta manera nueva de hacer LoadGame no retorna nada en medio del bucle.
        static void LoadGame()
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

        static void LoadGamesFromFiles(string[] files, List<Map> loadedGames)
        {
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    string content = File.ReadAllText(file);
                    Map loadedGame = Map.DeserializeFromJson(content);
                    loadedGames.Add(loadedGame);
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
        }
    }

}
