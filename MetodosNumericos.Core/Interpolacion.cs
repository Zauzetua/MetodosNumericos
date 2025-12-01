using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class Interpolacion
    {
        /// <summary>
        /// Metodo que realiza la interpolacion de Lagrange
        /// </summary>
        /// <param name="x">Valores de X</param>
        /// <param name="y">Valores de Y</param>
        /// <param name="xi">Valor de x a buscar su Y</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Si ambos arreglos no son de igual tamaño</exception>
        public static double Lagrange(double[] x, double[] y, double xi)
        {
            int n = x.Length;
            double resultado = 0.0;

            if (n != y.Length) 
            {
                throw new ArgumentException("Los arreglos x e y deben tener la misma longitud.");
            }

            for (int i = 0; i < n; i++)
            {
                double termino = y[i];
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        termino *= (xi - x[j]) / (x[i] - x[j]);
                    }
                }
                resultado += termino;
            }

            return resultado;
        }
    }
}
