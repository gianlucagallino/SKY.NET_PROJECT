using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet
{
    internal class Map
    {
        private Cell[,] mapMatrix;

        public Cell[,] MapMatrix { get; set; }

        Map()
        {
            mapMatrix = new Cell[10, 10];
        }

        //ES NECESARIO AGREGAR LOS LIMITES DE CANTIDADES.
        public void CreateMapDistribution(Cell[,] mapMatrix)
        {
            foreach (Cell cell in mapMatrix)
            {
                SetRandomTerrainType(cell);
            }
        }
        public void SetRandomTerrainType(Cell cell)
        {
            Random rng = new Random();
            int x = rng.Next(0, 5);
            cell.TerrainType = x;
        }

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
