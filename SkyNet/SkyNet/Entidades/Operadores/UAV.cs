namespace SkyNet.Entidades.Operadores
{
    class UAV : MechanicalOperator
    {
        public UAV( string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status)
            : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            maxLoad = 5;
            maxLoadOriginal = 5;
            optimalSpeed = 150;
            battery.MAHCapacity = 4000;
            battery.CurrentChargePercentage = 100;
            battery.Type = 3;
        }

        public UAV(int xposition, int yposition) : base(xposition, yposition)
        {
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }
    }
}