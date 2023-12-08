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
    }
}
