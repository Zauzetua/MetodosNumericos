using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class RaicesFuncion
    {
        int Iteraciones { get; set; }

        /// <summary>
        /// Funcion que implementa el metodo de biseccion, devuelve el valor de la raiz y el numero de iteraciones
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="xi"></param>
        /// <param name="xf"></param>
        /// <param name="eamax"></param>
        /// <returns></returns>
        public (double raiz, int iteraciones) Biseccion(Func<double, double> funcion, double xi, double xf, double eamax)
        {
            double xr = 0;
            double ea = 100;
            Iteraciones = 0;

            var fXi = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xi);
            var fXf = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xf);

            if (fXi * fXf >= 0)
            {
                throw new ArgumentException("La funcion no tiene raices en el intervalo dado.");
            }

            while (ea > eamax)
            {
                double xrOld = xr;
                xr = (xi + xf) / 2;
                Iteraciones++;

                var fXr = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xr);
                fXi = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xi);

                if (xr != 0)
                {
                    ea = Math.Abs((xr - xrOld) / xr) * 100;
                }

                if (fXi * fXr < 0)
                {
                    xf = xr;
                }
                else
                {
                    xi = xr;
                }
            }

            return (xr, Iteraciones);
        }

        /// <summary>
        /// Metodo que implementa el algoritmo de Regla falsa para encontrar raices, devuelve la raiz y el num de iteraciones
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="xi"></param>
        /// <param name="xf"></param>
        /// <param name="eamax"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public (double raiz, int iteraciones) ReglaFalsa(Func<double, double> funcion, double xi, double xf, double eamax)
        {
            double xr = 0;
            double ea = 100;
            Iteraciones = 0;

            var fXi = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xi);
            var fXf = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xf);

            if (fXi * fXf >= 0)
            {
                throw new ArgumentException("La funcion no tiene raices en el intervalo dado.");
            }

            while (ea > eamax)
            {
                double xrOld = xr;
                xr = xf - (fXf * (xi - xf)) / (fXi - fXf);
                Iteraciones++;

                var fXr = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xr);
                fXi = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xi);

                if (xr != 0)
                {
                    ea = Math.Abs((xr - xrOld) / xr) * 100;
                }

                if (fXi * fXr < 0)
                {
                    xf = xr;
                    fXf = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xf);
                }
                else
                {
                    xi = xr;
                    fXi = EvaluarFuncion.Evaluar(funcion.Method.ToString(), xi);
                }
            }

            return (xr, Iteraciones);


        }



    }
}
