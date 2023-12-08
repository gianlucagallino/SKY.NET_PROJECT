using SkyNet.Entidades.Mapa;

namespace SkyNet.Menu
{
    /*
       SaveOrLoadGame es una clase encargada de gestionar el guardado y carga de partidas en el juego. 
       Permite al usuario guardar el estado actual del mapa en un archivo JSON, cargar partidas específicas y 
       obtener la lista de partidas guardadas. La clase utiliza un directorio predefinido para almacenar los 
       archivos de guardado y proporciona funciones para realizar estas operaciones de manera interactiva.

     */
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
                    Message.SaveGameName();
                    string gameName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(gameName))
                    {
                        Message.InvalidName();
                    }
                    else
                    {
                        // Crear la carpeta si no existe
                        Directory.CreateDirectory(SaveFolderPath);

                        // Ruta completa del archivo
                        string filePath = Path.Combine(SaveFolderPath, $"{gameName}.json");

                        Map gameMap = Map.GetInstance();
                        string gameJson = Map.GetInstance().SerializeToJson();
                        File.WriteAllText(filePath, gameJson);
                        Message.GameSavedSuccessfully();
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
                loadedMap = Map.BuildMapFromJson(gameJson);
                Message.LoadGameSuccessfully(gameName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading the game '{gameName}': {ex.Message}");
            }

            return loadedMap;
        }

        public List<string> GetSavedGames()
        {
            string folderPath = "SavedGames";

            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath, "*.json").Select(Path.GetFileNameWithoutExtension).ToList();
            }
            else
            {
                Message.NoGameSaved();
                return new List<string>();
            }
        }

    }

}
