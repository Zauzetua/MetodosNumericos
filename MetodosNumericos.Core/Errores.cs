using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class Errores
    {
        /// <summary>
        /// Metodo que toma el valor real y el valor aproximado y devuelve el error absoluto
        /// </summary>
        /// <param name="valorReal">Valor real</param>
        /// <param name="ValorAproximado">Valor absoluto</param>
        /// <returns>Error (real-aprox)</returns>
        public double CalcularErrorAbsoluto(double valorReal, double ValorAproximado)
        {
            return Math.Abs(valorReal - ValorAproximado);
        }
        /// <summary>
        /// Metodo que toma el valor real y el valor aproximado y devuelve el error relativo
        /// </summary>
        /// <param name="valorReal"></param>
        /// <param name="ValorAproximado"></param>
        /// <returns></returns>
        public double CalcularErrorRelativo(double valorReal, double ValorAproximado)
        {
            return Math.Abs((valorReal - ValorAproximado) / valorReal);
        }

    }
}
