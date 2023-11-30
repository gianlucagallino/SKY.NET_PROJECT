using SkyNet.Entidades.Operadores;

namespace SkyNet.Entidades
{
    public class DamageSimulator
    {
        private readonly List<Action<MechanicalOperator>> damageActions;
        private bool damagedEngine;
        private bool stuckServo;
        private bool perforatedBattery;
        private bool disconectedBatteryPort;
        private bool paintScratch;
        private bool electronicLandfill;

        public bool DamagedEngine { get; set; }
        public bool StuckServo { get; set; }
        public bool PerforatedBattery { get; set; }
        public bool DisconnectedBatteryPort { get; set; }
        public bool PaintScratch { get; set; }

        public bool ElectronicLandfill { get; set; }
        public DamageSimulator()
        {
            DamagedEngine = false;
            StuckServo = false;
            PerforatedBattery = false;
            DisconnectedBatteryPort = false;
            PaintScratch = false;
            ElectronicLandfill = false;


            damageActions = new List<Action<MechanicalOperator>>
            {
                CompromisedMotorSimulate,
                StuckServoSimulate,
                PerforatedBatterySimulate,
                DisconnectedBatteryPortSimulate,
                PaintScratchSimulate
            };
        }
        public void SimulateRandomDamage(MechanicalOperator oper)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 101);

            if (randomNumber >= 1 && randomNumber <= 5)
            {
                int randomIndex = random.Next(damageActions.Count);
                damageActions[randomIndex](oper);
            }
        }

        public void CompromisedMotorSimulate(MechanicalOperator oper)
        {
            oper.OptimalSpeed /= 2;
            DamagedEngine = true;
        }

        public void StuckServoSimulate(MechanicalOperator oper)
        {
            oper.MaxLoad = 0;
            Console.WriteLine("This operator cannot carry weight");
            StuckServo = true;
        }

        public void PerforatedBatterySimulate(MechanicalOperator oper)
        {
            PerforatedBattery = true;
        }

        public void DisconnectedBatteryPortSimulate(MechanicalOperator oper)
        {
            oper.Battery.ChargeBattery(0);
            DisconnectedBatteryPort = true;

        }
        public void PaintScratchSimulate(MechanicalOperator oper)
        {
            PaintScratch = true;
        }

        public void ElectronicLandfillSimulate(MechanicalOperator oper)
        {
            oper.Battery.MaxCharge = 80;
        }

        public void Repair(MechanicalOperator oper)
        {
            DamagedEngine = false;
            StuckServo = false;
            PerforatedBattery = false;
            DisconnectedBatteryPort = false;
            PaintScratch = false;
            oper.Battery.MaxCharge = 100;
            oper.Battery.CompleteBatteryLevel();
            oper.OptimalSpeed = 100;

        }


        public void RepairBatteryOnly(MechanicalOperator oper)
        {
            PerforatedBattery = false;
            oper.Battery.MaxCharge = 100;
            oper.Battery.CompleteBatteryLevel();
            oper.OptimalSpeed = 100;
            oper.MaxLoad = oper.MaxLoadOriginal;
        }

    }
}
