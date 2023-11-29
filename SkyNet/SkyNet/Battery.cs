using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    public class Battery
    {
        private double mAhCapacity;//amperios 
        private int type;//tipo bateria
        private double currentChargePercentage;//porcentaje carga actual
        private DamageSimulator damageSimulator;


        public double MAHCapacity { get; set; }
        public int Type { get; set; }
        public double CurrentChargePercentage { get; set; }
        public double MaxCharge { get; set; }
        public DamageSimulator DamageSimulatorP { get; set; }

        public Battery(DamageSimulator damageSimulator)
        {
            this.damageSimulator = damageSimulator;
        }

        public Battery()
        {
            
        }
        public void ChargeBattery(double amountBatteryPercentage)
        {
            if (currentChargePercentage + amountBatteryPercentage <= 100)
            {
                currentChargePercentage += amountBatteryPercentage;
            }
            else { Console.WriteLine("The battery is at its maximum charge level"); }
        }

        public void CompleteBatteryLevel()
        {
            currentChargePercentage = 100;
        }

        public void DecreaseBattery(double amountBatteryPercentage)
        {
            double adjustedAmount = DamageSimulatorP.PerforatedBattery ? amountBatteryPercentage * 1.5 : amountBatteryPercentage;

            if (currentChargePercentage - amountBatteryPercentage >= 0)
            {
                currentChargePercentage -= amountBatteryPercentage;
            }
            else { Console.WriteLine("The battery level cannot go below 0; it is not possible to perform that task."); }
        }
    }
}