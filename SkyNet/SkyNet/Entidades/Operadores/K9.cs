﻿using SkyNet.Entidades.Mapa;
using System.Text.Json.Serialization;

/*
    Esta clase representa a un operador mecánico específico, hereda las características generales de la clase MechanicalOperator. 
    Se distingue por sus atributos predefinidos y la inicialización y representación de las características de los operadores K9
 */

namespace SkyNet.Entidades.Operadores
{
    class K9 : MechanicalOperator
    {

        public K9(string id, double maxLoad, double maxLoadOriginal, Battery battery, Location location, string status)
            : base(maxLoad, maxLoadOriginal, battery, location, status, id)
        {

            MaxLoad = 250;
            MaxLoadOriginal = 250;
            OptimalSpeed = 100;
            battery.MAHCapacity = 6500;
            battery.CurrentChargePercentage = 100;
            battery.Type = 1;
            Id = Convert.ToString("K9-" + Map.K9Counter);
        }
        [JsonConstructor]
        public K9(int xposition, int yposition) : base(xposition, yposition)
        {
            MaxLoad = 250;
            MaxLoadOriginal = 250;
            OptimalSpeed = 100;
            Battery = new Battery();
            Battery.MAHCapacity = 6500;
            Battery.CurrentChargePercentage = 100;
            Battery.Type = 1;
            Battery.DamageSimulatorP = new DamageSimulator();
            Id = Convert.ToString("K9-" + Map.K9Counter);
            LocationP.LocationX = xposition;
            LocationP.LocationY = yposition;
        }
        public override string ToString()
        {
            return $"MAH Capacity: {Battery.MAHCapacity}, Current Charge: {Battery.CurrentChargePercentage}%";
        }
    }
}