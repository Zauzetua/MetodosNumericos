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
        public (double raiz, int iteraciones) Biseccion(string funcion, double xi, double xf, double eamax)
        {
            try
            {
                double xr = 0;
                double ea = 100;
                Iteraciones = 0;


                var fXi = EvaluarFuncion.Evaluar(funcion, xi);
                var fXf = EvaluarFuncion.Evaluar(funcion, xf);

                if (eamax.Equals(0))
                {
                    throw new ArgumentException("El error maximo permitido debe de ser mayor que 0");
                }


                if (fXi * fXf >= 0)
                {
                    throw new ArgumentException("La funcion no tiene raices en el intervalo dado.");
                }

                while (ea > eamax)
                {
                    double xrOld = xr;
                    xr = (xi + xf) / 2;

                    Iteraciones++;

                    var fXr = EvaluarFuncion.Evaluar(funcion, xr);
                    fXi = EvaluarFuncion.Evaluar(funcion, xi);


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
            catch (ArgumentException)
            {
                throw;
            }
            catch (NCalc.Exceptions.NCalcParserException ex)
            {
                throw new Exception("Error al convertir la funcion, asegurese de haberla escrito bien");
            }

            catch (Exception ex)
            {
                throw new Exception("Error al evaluar la funcion: " + ex.Message);
            }

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
        public (double raiz, int iteraciones) ReglaFalsa(string funcion, double xi, double xf, double eamax)
        {
            try
            {
                if (funcion is null || funcion.Equals(""))
                {
                    throw new ArgumentException("La funcion no puede estar vacia");

                }
                if (eamax.Equals(0))
                {
                    throw new ArgumentException("El error maximo permitido debe de ser mayor que 0");
                }

                double xr = 0;
                double ea = 100;
                Iteraciones = 0;

                var fXi = EvaluarFuncion.Evaluar(funcion, xi);
                var fXf = EvaluarFuncion.Evaluar(funcion, xf);

                if (fXi * fXf >= 0)
                {
                    throw new ArgumentException("La funcion no tiene raices en el intervalo dado.");
                }

                while (ea > eamax)
                {
                    double xrOld = xr;
                    xr = xf - (fXf * (xi - xf)) / (fXi - fXf);
                    Iteraciones++;

                    var fXr = EvaluarFuncion.Evaluar(funcion, xr);
                    fXi = EvaluarFuncion.Evaluar(funcion, xi);

                    if (xr != 0)
                    {
                        ea = Math.Abs((xr - xrOld) / xr) * 100;
                    }

                    if (fXi * fXr < 0)
                    {
                        xf = xr;
                        fXf = EvaluarFuncion.Evaluar(funcion, xf);
                    }
                    else
                    {
                        xi = xr;
                        fXi = EvaluarFuncion.Evaluar(funcion, xi);
                    }
                }

                return (xr, Iteraciones);


            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (NCalc.Exceptions.NCalcParserException ex)
            {
                throw new Exception("Error al convertir la funcion, asegurese de haberla escrito bien");
            }

            catch (Exception ex)
            {
                throw new Exception("Error al evaluar la funcion: " + ex.Message);
            }




        }
    }
}
