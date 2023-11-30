using SkyNet.Entidades.Operadores;

namespace SkyNet.Entidades
{
    internal class ConcreteMechanicalOperator : MechanicalOperator
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public Location LocationP { get; set; }
    }
}