namespace SkyNet.Entidades.Operadores
{
    class K9 : MechanicalOperator
    {

        public K9(string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status)
            : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {

            maxLoad = 250;
            maxLoadOriginal = 250;
            optimalSpeed = 100;
            battery.MAHCapacity = 6500;
            battery.CurrentChargePercentage = 100;
            battery.Type = 1;
        }

        public K9(int xposition, int yposition) : base(xposition, yposition)
        {
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }

    }
}