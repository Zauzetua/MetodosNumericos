using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class Tabula
    {
        /// <summary>
        /// Clase que representa un par (x, y)
        /// </summary>
        public class Resultado
        {
            public double x { get; set; }
            public double y { get; set; }
        }

        /// <summary>
        /// Tabula una funcion en el rango [xi, xf] con incremento incx.
        /// </summary>
        /// <param name="funcion">Funcion a tabular</param>
        /// <param name="xi"> Inicio en x</param>
        /// <param name="xf"> Final en x</param>
        /// <param name="incx"> Incremento en x</param>
        /// <param name="minimo"> Valor minimo de (x,y) en la tabulacion</param>
        /// <param name="maximo">Valor maximo de (x,y) en la tabulacion</param>
        /// <returns></returns>
        public List<Resultado> Tabular(Func<double, double> funcion, double xi, double xf, double incx, out Resultado minimo, out Resultado maximo)
        {
            var resultados = new List<Resultado>();

            double minY = double.MaxValue, maxY = double.MinValue;
            double minX = xi, maxX = xi;

            for (double x = xi; x <= xf; x += incx) //Recorrer  el rango
            {
                double y = funcion(x);
                resultados.Add(new Resultado { x = x, y = y });

                if (y < minY) { minY = y; minX = x; }
                if (y > maxY) { maxY = y; maxX = x; }
            }

            minimo = new Resultado { x = minX, y = minY };
            maximo = new Resultado { x = maxX, y = maxY };

            return resultados;
        }
    }

}

