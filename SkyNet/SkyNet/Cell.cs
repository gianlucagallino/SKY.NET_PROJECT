using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Cell
    {
        private double terrainType;

        public double TerrainType { set; get; }

        Cell()
        {
            terrainType = 0;
        }

        //Es necesario agregar una sobrecarga de "interaaccion", con cada celda, correspondiente a su tipo. Posible uso de sobrecarga de metodo. 

        /* Referencias de TerrainType (CONSIDERAR MOVER SISTEMA A ENUM)
         * 0- Terreno Neutro (baldio, planicie, bosque, sector urbano)
         * 1- Vertedero
         * 2-Lago
         * 3-Vertedero electronico
         * 4-Sitio de reciclaje (Implementar maximo 5)
         * 5-Cuartel general(maximo 3)
         */
    }
}
