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
        /// Clase que representa el resultado de cada iteracion para encontrar raices.
        /// </summary>
        public class ResultadoIteracion
        {
            public int Iteracion { get; set; }
            public double Xi { get; set; }
            public double Xf { get; set; }
            public double Xr { get; set; }
            public double Fxi { get; set; }
            public double Fxf { get; set; }
            public double Fxr { get; set; }
            public double FxiFxr { get; set; }
            public double Ea { get; set; }
        }

        /// <summary>
        /// Funcion que implementa el metodo de biseccion, devuelve una lista con los resultados de cada iteracion
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="xi"></param>
        /// <param name="xf"></param>
        /// <param name="eamax"></param>
        /// <returns></returns>
        public List<ResultadoIteracion> Biseccion(string funcion, double xi, double xf, double eamax)
        {
            try
            {
                if (funcion is null || funcion.Equals(""))
                {
                    throw new ArgumentException("La funcion no puede estar vacia");

                }
                var resultados = new List<ResultadoIteracion>();
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
                    fXf = EvaluarFuncion.Evaluar(funcion, xf);
                    resultados.Add(new ResultadoIteracion
                    {
                        Iteracion = Iteraciones,
                        Xi = xi,
                        Xf = xf,
                        Xr = xr,
                        Fxi = fXi,
                        Fxf = fXf,
                        Fxr = fXr,
                        Ea = ea,
                        FxiFxr = fXi * fXr
                    });



                    if (Math.Abs(fXr) < eamax)
                    {
                        ea = eamax;
                        resultados.Last().Ea = ea;
                        break;
                    }

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

                return (resultados);
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
        public List<ResultadoIteracion> ReglaFalsa(string funcion, double xi, double xf, double eamax)
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
                var resultados = new List<ResultadoIteracion>();
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
                    fXf = EvaluarFuncion.Evaluar(funcion, xf);
                    resultados.Add(new ResultadoIteracion
                    {
                        Iteracion = Iteraciones,
                        Xi = xi,
                        Xf = xf,
                        Xr = xr,
                        Fxi = fXi,
                        Fxf = fXf,
                        Fxr = fXr,
                        Ea = ea,
                        FxiFxr = fXi * fXr
                    });

                    if (Math.Abs(fXr) < eamax)
                    {
                        ea = eamax;
                        resultados.Last().Ea = ea;
                        break;
                    }

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

                return (resultados);


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
        /// Metodo que implementa el metodo de Newton Raphson para encontrar raices, entrega una lista con los resultados de cada iteracion
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="xi"></param>
        /// <param name="eamax"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ResultadoIteracion> NewtonRaphson(string funcion, double xi, double eamax)
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
                var funciones = new Funciones();
                var derivadas = new Derivadas();

                var resultados = new List<ResultadoIteracion>();
                var xr = 0.0;
                var ea = 100.0;
                Iteraciones = 0;
                while (ea > eamax)
                {
                    double xrOld = xr;
                    var fXi = funciones.ConvertirAFunc(funcion);
                    var fXiDerivada = derivadas.Derivar(fXi, xi);

                    if (fXiDerivada == 0)
                    {
                        throw new ArgumentException("La derivada es cero, no se puede continuar con el metodo.");
                    }
                    xr = xi - (fXi(xi) / fXiDerivada);
                    Iteraciones++;

                    var fXr = funciones.ConvertirAFunc(funcion)(xr);
                    resultados.Add(new ResultadoIteracion
                    {
                        Iteracion = Iteraciones,
                        Xi = xi,
                        Xr = xr,
                        Fxi = fXi(xi),
                        Fxr = fXr,
                        Ea = ea
                    });

                    if (Math.Abs(fXr) < eamax)
                    {
                        ea = eamax;
                        resultados.Last().Ea = ea;
                        break;
                    }

                    if (xr != 0)
                    {
                        ea = Math.Abs((xr - xrOld) / xr) * 100;
                    }

                    xi = xr;

                }
                return (resultados);

            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (NCalc.Exceptions.NCalcParserException)
            {
                throw new Exception("Error al convertir la funcion, asegurese de haberla escrito bien");
            }

            catch (Exception ex)
            {
                throw new Exception("Error al evaluar la funcion: " + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que implementa el metodo de la secante para encontrar raices, devuelve una lista con los resultados de cada iteracion
        /// </summary>
        /// <param name="funcion"></param>
        /// <param name="xi"></param>
        /// <param name="xf"></param>
        /// <param name="eamax"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ResultadoIteracion> Secante(string funcion, double xi, double xf, double eamax)
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
                var resultados = new List<ResultadoIteracion>();
                double xr = 0;
                double ea = 100;
                Iteraciones = 0;

                var fXi = EvaluarFuncion.Evaluar(funcion, xi);
                var fXi1 = EvaluarFuncion.Evaluar(funcion, xf);

                if (fXi * fXi1 >= 0)
                {
                    throw new ArgumentException("La funcion no tiene raices en el intervalo dado.");
                }

                while (ea > eamax)
                {
                    double xrOld = xr;
                    xr = xf - (fXi1 * (xi - xf)) / (fXi - fXi1);
                    Iteraciones++;

                    var fXr = EvaluarFuncion.Evaluar(funcion, xr);
                    fXi = EvaluarFuncion.Evaluar(funcion, xi);
                    fXi1 = EvaluarFuncion.Evaluar(funcion, xf);
                    resultados.Add(new ResultadoIteracion
                    {
                        Iteracion = Iteraciones,
                        Xi = xi,
                        Xf = xf,
                        Xr = xr,
                        Fxi = fXi,
                        Fxf = fXi1,
                        Fxr = fXr,
                        Ea = ea,
                        FxiFxr = fXi * fXr
                    });

                    if (Math.Abs(fXr) < eamax)
                    {
                        ea = eamax;
                        resultados.Last().Ea = ea;
                        break;
                    }

                    if (xr != 0)
                    {
                        ea = Math.Abs((xr - xrOld) / xr) * 100;
                    }

                    xi = xf;
                    fXi = EvaluarFuncion.Evaluar(funcion, xi);
                    xf = xr;
                    fXi1 = EvaluarFuncion.Evaluar(funcion, xf);
                }

                return resultados;
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
        /// Metodo que formatea los resultados a 4 decimales.
        /// </summary>
        /// <param name="resultados"></param>
        /// <returns></returns>
        public List<ResultadoIteracion> FormatearResultados(List<ResultadoIteracion> resultados)
        {
            return resultados.Select(r => new ResultadoIteracion
            {
                Iteracion = r.Iteracion,
                Xi = Math.Round(r.Xi, 4),
                Xf = Math.Round(r.Xf, 4),
                Xr = Math.Round(r.Xr, 4),
                Fxi = Math.Round(r.Fxi, 4),
                Fxf = Math.Round(r.Fxf, 4),
                Fxr = Math.Round(r.Fxr, 4),
                Ea = Math.Round(r.Ea, 4),
                FxiFxr = Math.Round(r.FxiFxr, 4)
            }).ToList();
        }
    }
}
