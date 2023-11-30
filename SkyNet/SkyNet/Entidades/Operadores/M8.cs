namespace SkyNet.Entidades.Operadores
{
    class M8 : MechanicalOperator
    {
        public M8( string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status) : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            maxLoad = 40;
            maxLoadOriginal = 40;
            optimalSpeed = 250;
            battery.MAHCapacity = 12250;
            battery.CurrentChargePercentage = 100;
            battery.Type = 2;
        }

        public M8(int xposition, int yposition) : base(xposition, yposition)
        {
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }

    }
}