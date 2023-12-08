using SkyNet.Entidades.Operadores;
using System;
using System.Text.Json.Serialization;

/*
    La clase DamageSimulator simula daños potenciales en operadores mecánicos. Sus atributos representan diferentes tipos de daños
    y proporciona métodos para simular daños aleatorios, reparar los daños y realizar reparaciones específicas en la batería:
    SimulateRandomDamage(), Repair(), entre otros.
 */

namespace SkyNet.Entidades
{
    public class DamageSimulator
    {
        private readonly List<Action<MechanicalOperator>> damageActions;
        private readonly Random random = new Random();

        public bool DamagedEngine { get; private set; }
        public bool StuckServo { get; private set; }
        public bool PerforatedBattery { get; private set; }
        public bool DisconnectedBatteryPort { get; private set; }
        public bool PaintScratch { get; private set; }
        public bool ElectronicLandfill { get; private set; }

        [JsonConstructor]
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
            PaintScratchSimulate,
            ElectronicLandfillSimulate
        };
        }

        public void SimulateRandomDamage(MechanicalOperator oper)
        {
            int randomNumber = random.Next(1, 101);

            if (randomNumber >= 1 && randomNumber <= 5)
            {
                DamageType damageType = (DamageType)random.Next(Enum.GetValues(typeof(DamageType)).Length);
                damageActions[(int)damageType](oper);
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
            //Console.WriteLine("This operator cannot carry weight"); No estoy seguro de que sea el lugar para un mensaje
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
