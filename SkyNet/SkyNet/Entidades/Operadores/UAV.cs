using SkyNet.Entidades.Mapa;
using System.Text.Json.Serialization;

/*
    Esta clase representa a un operador mecánico específico, hereda las características generales de la clase MechanicalOperator. 
    Se distingue por sus atributos predefinidos y la inicialización y representación de las características de los operadores UAV
 */

namespace SkyNet.Entidades.Operadores
{
    class UAV : MechanicalOperator
    {
        public UAV(string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status)
            : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            MaxLoad = 5;
            MaxLoadOriginal = 5;
            OptimalSpeed = 150;
            battery=new Battery();
            battery.MAHCapacity = 4000;
            battery.CurrentChargePercentage = 100;
            battery.Type = 3;
            Id = Convert.ToString("UAV-" + Map.UAVCounter);
        }
        [JsonConstructor]
        public UAV(int xposition, int yposition) : base(xposition, yposition)
        {
            MaxLoad = 5;
            MaxLoadOriginal = 5;
            OptimalSpeed = 150;
            Battery = new Battery();
            Battery.MAHCapacity = 4000;
            Battery.CurrentChargePercentage = 100;
            Battery.Type = 3;
            Battery.DamageSimulatorP = new DamageSimulator();
            Id = Convert.ToString("UAV-" + Map.UAVCounter);
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }

        public override string ToString()
        {
            return $"MAH Capacity: {Battery.MAHCapacity}, Current Charge: {Battery.CurrentChargePercentage}%";
        }
    }
}