using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class Derivadas
    {

        public double Derivar(Func<double, double> f, double x)
        {
            double h = 1e-6;
            var resultado = (f(x + h) - f(x - h)) / (2 * h);
            return resultado;
        }
    }
}
