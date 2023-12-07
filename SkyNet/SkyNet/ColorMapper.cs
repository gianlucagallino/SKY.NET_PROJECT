namespace SkyNet
{


    public class ColorMapper
    {
        // Define a dictionary to map TerrainType to ConsoleColor
        private readonly Dictionary<int, ConsoleColor> TerrainColorMap = new Dictionary<int, ConsoleColor>
    {
        { 0, ConsoleColor.White },
        { 1, ConsoleColor.Green },
        { 2, ConsoleColor.Blue },
        { 3, ConsoleColor.Yellow },
        { 4, ConsoleColor.DarkYellow },
        { 5, ConsoleColor.Magenta }
    };

        // Function to get the ConsoleColor based on TerrainType
        public ConsoleColor GetColorForTerrainType(int terrainType)
        {
            // If the TerrainType is not found, default to ConsoleColor.White
            return TerrainColorMap.TryGetValue(terrainType, out ConsoleColor color) ? color : ConsoleColor.White;
        }
    }

}
