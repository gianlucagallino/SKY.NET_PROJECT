using System.Text.Json.Serialization;

/*
    Esta clase guarda a través de sus propiedades, las coordenadas X e Y de cada objeto y nodo creado en la clase Map.
    Posee un método ToString() que es utilizado para concatenar las dos variables y poder guardarlas en la Base de Datos
    para registrar los últimos lugares según sus coordenadas donde han estado los operadores. 
    De esta manera si la coordenada X=12 y la coordenadaY=6 el ToString() devolvera 126.
    También es utilizado para la serialización y deserialización de objetos JSON del Programa.
 */

namespace SkyNet.Entidades
{
    public class Location
    {
        private int locationX;
        private int locationY;

        public int LocationX { get; set; }
        public int LocationY { get; set; }

        [JsonConstructor]
        public Location()
        {
            LocationX = 0;
            LocationY = 0;
        }
        public Location(int hor, int vert)
        {
            LocationX = hor;
            LocationY = vert;
        }

        public override string ToString()
        {
            return LocationX.ToString() + LocationY.ToString();
        }
    }
}