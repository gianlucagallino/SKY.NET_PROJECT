using System.Text.Json.Serialization;

namespace SkyNet.Entidades
{
    /*
    La clase Battery representa la fuente de energía para los operadores mecánicos.
    Su función principal es gestionar la carga y descarga de la batería, considerando simulaciones de daño proporcionadas
    por el DamageSimulator.
    */
    public class Battery
    {
        public double MAHCapacity { get; set; }
        public int Type { get; set; }
        public double CurrentChargePercentage { get; set; }

        public double MaximumChargePercentage { get; set; }
        public double MaxCharge { get; set; }
        public DamageSimulator DamageSimulatorP { get; set; }

        public Battery(DamageSimulator damageSimulator)
        {
            this.DamageSimulatorP = damageSimulator;
        }

        public Battery(double mahCapacity, int type, double currentChargePercentage, double maxCharge, DamageSimulator damageSimulator)
        {
            MAHCapacity = mahCapacity;
            Type = type;
            CurrentChargePercentage = currentChargePercentage;
            MaximumChargePercentage = 100;
            MaxCharge = maxCharge;
            DamageSimulatorP = damageSimulator;

        }
        [JsonConstructor]
        public Battery()
        {

        }
        public void ChargeBattery(double amountBatteryPercentage)
        {
            if (IsValidBatteryPercentage(amountBatteryPercentage))
            {
                if (CurrentChargePercentage + amountBatteryPercentage <= MaximumChargePercentage)
                {
                    CurrentChargePercentage += amountBatteryPercentage;
                }
                else
                {
                    LogMessage("The battery is at its maximum charge level");
                }
            }
            else
            {
                LogMessage("Invalid battery percentage");
            }
        }

        public void CompleteBatteryLevel()
        {
            CurrentChargePercentage = MaximumChargePercentage;
        }

        public void DecreaseBattery(double amountBatteryPercentage)
        {
            double adjustedAmount = DamageSimulatorP.PerforatedBattery ? amountBatteryPercentage * 1.5 : amountBatteryPercentage;

            if (IsValidBatteryPercentage(adjustedAmount))
            {
                if (CurrentChargePercentage - adjustedAmount >= 0)
                {
                    CurrentChargePercentage -= adjustedAmount;
                }
                else
                {
                    LogMessage("The battery level cannot go below 0; it is not possible to perform that task.");
                }
            }
            else
            {
                LogMessage("Invalid battery percentage");
            }
        }

        private bool IsValidBatteryPercentage(double amountBatteryPercentage)
        {
            bool ok = false;

            if (amountBatteryPercentage >= 0 && amountBatteryPercentage <= CurrentChargePercentage)
            { 
                ok = true;
            }
             return ok ;
        }

        private void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}