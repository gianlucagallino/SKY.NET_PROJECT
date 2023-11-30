namespace SkyNet
{
    class UAV : MechanicalOperator
    {


        private float flightHeight;
        private int bladeAmount;
        private bool integratedCamera;


        public float FlightHeight { get; set; }
        public int BladeAmount { get; set; }
        public bool IntegratedCamera { get; set; }
        /* public void Fly()
         {

         }*/
        public UAV(/*float flightHeight, int bladeAmount, bool integratedCamera,*/ string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status)
            : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {
            //flightHeight = 0;
            //bladeAmount = 0;
            //integratedCamera = true;
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