using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    public static class Message
    {

        //("The battery is at its maximum charge level")
        public static void BatteryMaxiumCharge()
        {
            Console.WriteLine("The battery is at its maximum charge level");
        }
        //("Invalid battery percentage")
        public static void InvalidBatteryPercentage()
        {
            Console.WriteLine("Invalid battery percentage");
        }

        public static void BatteryCannotNegative()
        {
            Console.WriteLine("The battery level cannot go below 0; it is not possible to perform that task.");
        }
        public static void OperatorsLakeProhibition()
        {
            Console.WriteLine("M8 and K9 cannot enter the lake.");
        }

        public static void PathNotFound()
        {
            Console.WriteLine("No path found for this unit.");
        }
        public static void DestinationReached()
        {
            Console.WriteLine("Destination reached!");
        }
        public static void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue");
        }
        public static void AmontNonNegative()
        {
            Console.WriteLine("Amount must be non-negative for Transfer Battery.");
        }
        public static void BatteryValidationFailure()
        {
            Console.WriteLine("Transfer Battery aborted due to battery validation failure.");
        }
        public static void AmontNonNegativeLoad()
        {
            Console.WriteLine("Amount must be non-negative for Transfer Load.");
        }
        public static void MuchLoad()
        {
            Console.WriteLine("Transfer Load failed. Destination operator cannot hold that much load.");
        }
        public static void BatteryCapacity()
        {
            Console.WriteLine("Battery validation failed. Not enough battery capacity for the transfer.");
        }
        public static void NodeListEmpty()
        {
            Console.WriteLine("The list of nodes is empty. Unable to find the closest node.");
        }
        public static void OperatorNotDamaged()
        {
            Console.WriteLine("This operator is not damaged.");
            
        }
        public static void DesiredMapSize()
        {
            Console.Write("Please, enter your desired map size between 30-100 (Recommended: 30): ");
        }
        public static void StartingBuildMap()
        {
            Console.WriteLine("Starting BuildMapFromJson method...");
        }
        public static void DeserializationSuccessfully()
        {
            Console.WriteLine("Serialization model deserialized successfully.");
        }
        public static void MapCreated()
        {
            Console.WriteLine("Map object created successfully.");
        }
        public static void PropertiesMapCreated()
        {
            Console.WriteLine("Map properties set successfully.");
        }
        public static void BuildMapFinished() 
        {
            Console.WriteLine("BuildMapFromJson method finished.");
        }
        public static void SaveGameName()
        {
            Console.WriteLine("Enter the name for the saved game:");
        }
        public static void InvalidName()
        {
            Console.WriteLine("Invalid game name. The game was not saved.");
        }
        public static void GameSavedSuccessfully()
        {
            Console.WriteLine("Game saved successfully");
        }
        public static void LoadGameSuccessfully(string name)
        {
            Console.WriteLine($"Game '{name}' loaded successfully");
        }
        public static void NoGameSaved()
        {
            Console.WriteLine("No saved games folder found.");
        }
    }
}
