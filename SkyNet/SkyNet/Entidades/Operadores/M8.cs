using SkyNet.Entidades.Mapa;
using System.Text.Json.Serialization;

/*
    Esta clase representa a un operador mecánico específico M8, hereda las características generales de la clase MechanicalOperator. 
    Posee una relación de asociación con la clase Battery.
 */

namespace SkyNet.Entidades.Operadores
{
    class M8 : MechanicalOperator
    {
        public M8(string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status) : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            MaxLoad = 40;
            MaxLoadOriginal = 40;
            OptimalSpeed = 250;
            battery.MAHCapacity = 12250;
            battery.CurrentChargePercentage = 80;
            battery.Type = 2;
            Id = Convert.ToString("M8-" + Map.M8Counter);
        }
        [JsonConstructor]
        public M8()
        {

        }
        public M8(int xposition, int yposition) : base(xposition, yposition)
        {
            MaxLoad = 40;
            MaxLoadOriginal = 40;
            OptimalSpeed = 250;
            Battery.MAHCapacity = 12250;
            Battery.CurrentChargePercentage = 100;
            Battery.Type = 2;
            Battery.MaxCharge = 100;
            Battery.DamageSimulatorP = new DamageSimulator();
            Id = Convert.ToString("M8-" + Map.M8Counter);
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }
        public override string ToString()
        {
            return $"MAH Capacity: {Battery.MAHCapacity}, Current Charge: {Battery.CurrentChargePercentage}%";
        }
    }
}