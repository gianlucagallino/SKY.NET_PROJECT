using SkyNet.Entidades.Mapa;

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
            battery.CurrentChargePercentage = 100;
            battery.Type = 2;
            Id = Convert.ToString("M8-" + Map.M8Counter);
        }

        public M8(int xposition, int yposition) : base(xposition, yposition)
        {
            MaxLoad = 40;
            MaxLoadOriginal = 40;
            OptimalSpeed = 250;
            Battery.MAHCapacity = 12250;
            Battery.CurrentChargePercentage = 100;
            Battery.Type = 2;
            Id = Convert.ToString("M8-" + Map.M8Counter);
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }
        public override string ToString()
        {
            return $"MAH Capacity: {Battery.MAHCapacity}, Type: {Battery.Type}, Current Charge: {Battery.CurrentChargePercentage}%";
        }
    }
}