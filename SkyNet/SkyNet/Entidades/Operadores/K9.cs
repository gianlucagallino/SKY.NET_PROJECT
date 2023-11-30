namespace SkyNet.Entidades.Operadores
{
    class K9 : MechanicalOperator
    {

        private string sensorType;
        private string movility;

        public string SensorType { get; set; }
        public string Movility { get; set; }

        public K9(/*string sensorType, string Movility, */string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status)
            : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            //this.sensorType = string.Empty;
            //this.movility = string.Empty;
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
        /*public void Patrol()
{
}*/
    }
}