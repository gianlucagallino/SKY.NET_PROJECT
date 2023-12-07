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
        private double mAhCapacity;
        private int type;
        private double currentChargePercentage;
        private DamageSimulator damageSimulator;
        public double MAHCapacity { get; set; }
        public int Type { get; set; }
        public double CurrentChargePercentage { get; set; }
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
            MaxCharge = maxCharge;
            DamageSimulatorP = damageSimulator;

        }
        [JsonConstructor]
        public Battery()
        {
  
        }
        public void ChargeBattery(double amountBatteryPercentage)
        {
            if (CurrentChargePercentage + amountBatteryPercentage <= 100)
            {
                CurrentChargePercentage += amountBatteryPercentage;
            }
            else { Console.WriteLine("The battery is at its maximum charge level"); }
        }

        public void CompleteBatteryLevel()
        {
            CurrentChargePercentage = 100;
        }

        public void DecreaseBattery(double amountBatteryPercentage)
        {
            double adjustedAmount = DamageSimulatorP.PerforatedBattery ? amountBatteryPercentage * 1.5 : amountBatteryPercentage; //Por que Damage simulator devuelve null

            if (CurrentChargePercentage - amountBatteryPercentage >= 0)
            {
                CurrentChargePercentage -= amountBatteryPercentage;
            }
            else { Console.WriteLine("The battery level cannot go below 0; it is not possible to perform that task."); }
        }
    }
}