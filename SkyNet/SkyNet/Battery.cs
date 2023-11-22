using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    public class Battery
    {
        private double mAhCapacity;
        private int type;
        private double currentCharge;
        private double maxCharge;

        public double MAHCapacity { get; set; }
        public int Type { get; set; }
        public double CurrentCharge { get; set; }
        public double MaxCharge { get; set; }

        public Battery()
        {
           
        }

        public void ChargeBattery(double amountBattery)
        {
            if (currentCharge + amountBattery <= maxCharge)
            {
                currentCharge += amountBattery;
            }
            else { Console.WriteLine("The battery is at its maximum charge level"); }
        }

        public void CompleteBatteryLevel()
        {
            currentCharge = maxCharge;
        }

        public void DecreaseBattery(double amountBattery)
        {
            if (currentCharge - amountBattery >= 0)
            {
                currentCharge-= amountBattery;
            }
            else { Console.WriteLine("The battery level cannot go below 0; it is not possible to perform that task."); }
        }
    }
}
