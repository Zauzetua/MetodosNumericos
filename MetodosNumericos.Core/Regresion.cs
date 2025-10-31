using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{

    public enum TipoRegresion
    {
        Lineal,
        Polinomial,
        Exponencial,
        Logaritmica
    }

    public class MejorAjusteRegresion
    {
        /// <summary>
        /// El tipo de modelo que se determino como el mejor ajuste.
        /// </summary>
        public TipoRegresion ModeloElegido { get; set; }

        /// <summary>
        /// El coeficiente de determinacion (R2) del modelo elegido.
        /// </summary>
        public double RCuadrado { get; set; }

        /// <summary>
        /// Coeficiente 'a0'.
        /// Si es Lineal, es el Intercepto.
        /// Si es Exponencial, es ln(a) de la ecuacion linearizada.
        /// </summary>
        public double A0 { get; set; }

        /// <summary>
        /// Coeficiente 'a1'.
        /// Si es Lineal, es la Pendiente.
        /// Si es Exponencial, es ln(b) de la ecuacion linearizada.
        /// </summary>
        public double A1 { get; set; }

        /// <summary>
        /// Obtiene la ecuacion legible del modelo.
        /// </summary>
        public string ObtenerEcuacion()
        {
            if (ModeloElegido == TipoRegresion.Lineal)
            {
                return $"y = {A0:F4} + {A1:F4}x";
            }
            else
            {
                // Revertimos los coeficientes para la ecuacion final
                double a = Math.Exp(A0);
                double b = Math.Exp(A1);
                return $"y = {a:F4} * {b:F4}^x";
            }
        }
    }

    /// <summary>
    /// Contiene los resultados de un calculo de regresion lineal simple (A0, A1, R^2).
    /// </summary>
    public class ResultadoRegresion
    {
        public double A0 { get; set; }
        public double A1 { get; set; }
        public double RCuadrado { get; set; }
    }
    public class Regresion
    {
        /// <summary>
        /// Metodo para calcular las sumas necesarias para la regresion lineal
        /// </summary>
        /// <param name="puntos"></param>
        /// <returns></returns>
        public static List<ResultadosPorIteracion> CalcularSumas(List<PuntoXY> puntos)
        {
            if (puntos.Count() < 2 || puntos == null)
            {
                throw new ArgumentException("La lista de puntos debe contener al menos dos puntos.");
            }
            List<ResultadosPorIteracion> resultados = [];

            foreach (var punto in puntos)
            {
                ResultadosPorIteracion resultado = new ResultadosPorIteracion
                {
                    //formatear a 6 decimales
                    Xi = punto.X,
                    Yi = punto.Y,
                    Xi2 = Math.Pow(punto.X, 2),
                    XiYi = punto.X * punto.Y,
                    Yi2 = Math.Pow(punto.Y, 2)
                };

                resultados.Add(resultado);
            }

            return resultados;
        }

        /// <summary>
        /// Metodo que realiza la regresion lineal y devuelve los coeficientes a0 y a1
        /// </summary>
        /// <param name="resultados"></param>
        /// <returns></returns>
        public static ResultadoRegresion RegresionLineal(List<ResultadosPorIteracion> resultados)
        {
            int n = resultados.Count;
            double sumX = resultados.Sum(r => r.Xi);
            double sumY = resultados.Sum(r => r.Yi);
            double sumX2 = resultados.Sum(r => r.Xi2);
            double sumXY = resultados.Sum(r => r.XiYi);
            double sumY2 = resultados.Sum(r => r.Yi2);
            var denominator = (n * sumX2 - Math.Pow(sumX, 2));
            if (denominator <= 0)
            {
                throw new InvalidOperationException("No se puede calcular la regresion lineal debido a una division por cero.");
            }
            double a1 = ((n * sumXY) - (sumX * sumY)) / denominator;
            double a0 = (sumY - a1 * sumX) / n;

            double numR = (n * sumXY) - (sumX * sumY);
            double denR_X = (n * sumX2) - (sumX * sumX);
            double denR_Y = (n * sumY2) - (sumY * sumY);

            if (Math.Abs(denR_X) < 1e-10 || Math.Abs(denR_Y) < 1e-10)
            {
                return new ResultadoRegresion { A0 = a0, A1 = a1, RCuadrado = 1.0 };
            }

            double r = numR / Math.Sqrt(denR_X * denR_Y);

            return new ResultadoRegresion
            {
                A0 = a0,
                A1 = a1,
                RCuadrado = r * r
            };
        }
    }

    /// <summary>
    /// Analiza multiples modelos de regresion y elige el mejor.
    /// </summary>
    public class AnalizadorDeRegresion
    {
        /// <summary>
        /// Compara un modelo de regresion lineal y uno exponencial,
        /// y devuelve los coeficientes del que mejor se ajuste (mayor R^2).
        /// </summary>
        public static MejorAjusteRegresion ObtenerMejorRegresion(List<PuntoXY> puntos)
        {
            //Probar Modelo Lineal
            var resLineal = Regresion.CalcularSumas(puntos);
            var resultadoLineal = Regresion.RegresionLineal(resLineal);

            //Probar Modelo Exponencial
            MejorAjusteRegresion resultadoExponencial = null!;
            bool esExponencialValido = true;

            try
            {
                var puntosLinearizados = puntos
                    .Select(p => new PuntoXY(p.X, Math.Log(p.Y))) // Transformacion
                    .ToList();


                var resExpLineal = Regresion.RegresionLineal(Regresion.CalcularSumas(puntosLinearizados));
                // Transformamos 
                double a = Math.Exp(resExpLineal.A0);
                double b = resExpLineal.A1;

                //
                var yPred = puntos.Select(p => a * Math.Exp(b * p.X)).ToList();
                var yMean = puntos.Average(p => p.Y);
                double ssTot = puntos.Sum(p => Math.Pow(p.Y - yMean, 2));
                double ssRes = puntos.Select((p, i) => Math.Pow(p.Y - yPred[i], 2)).Sum();
                double r2Exp = 1 - ssRes / ssTot;
                resultadoExponencial = new MejorAjusteRegresion
                {
                    ModeloElegido = TipoRegresion.Exponencial,
                    RCuadrado = r2Exp,
                    A0 = a,
                    A1 = b
                };
            }
            catch
            {
                esExponencialValido = false;
            }

            //Comparar y decidir
            if (!esExponencialValido || resultadoLineal.RCuadrado >= (resultadoExponencial?.RCuadrado ?? 0))
            {
                return new MejorAjusteRegresion
                {
                    ModeloElegido = TipoRegresion.Lineal,
                    RCuadrado = resultadoLineal.RCuadrado,
                    A0 = resultadoLineal.A0,
                    A1 = resultadoLineal.A1
                };
            }
            else
            {
                return resultadoExponencial ?? new MejorAjusteRegresion();
            }
        }
    }

    /// <summary>
    /// Representa un punto en el espacio XY.
    /// </summary>
    public class PuntoXY
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PuntoXY(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Clase que almacena los resultados parciales por cada iteracion
    /// </summary>
    public class ResultadosPorIteracion
    {
        public double Xi { get; set; }
        public double Yi { get; set; }
        public double Xi2 { get; set; }
        public double Yi2 { get; set; }
        public double XiYi { get; set; }
    }

    /// <summary>
    /// Clase que maneja la regresión polinomial de cualquier grado
    /// </summary>
    public class RegresionPolinomial
    {
        /// <summary>
        /// Calcula los coeficientes de la regresión polinomial
        /// </summary>
        /// <param name="puntos">Lista de puntos XY</param>
        /// <param name="grado">Grado del polinomio (1 para lineal, 2 para cuadrática, etc.)</param>
        /// <returns>Coeficientes del polinomio y resultados por iteración</returns>
        public static (double[] coeficientes, List<ResultadosPolinomiales> resultados) CalcularRegresionPolinomial(List<PuntoXY> puntos, int grado)
        {
            if (puntos == null || puntos.Count < grado + 1)
            {
                throw new ArgumentException($"Se necesitan al menos {grado + 1} puntos para una regresion de grado {grado}");
            }

            int n = puntos.Count;
            var resultados = new List<ResultadosPolinomiales>();

            // Crear la matriz de coeficientes y el vector de terminos independientes
            double[,] matrizA = new double[grado + 1, grado + 1];
            double[] vectorB = new double[grado + 1];

            // Calcular las sumas necesarias
            foreach (var punto in puntos)
            {
                var resultado = new ResultadosPolinomiales
                {
                    X = punto.X,
                    Y = punto.Y,
                    PotenciasX = new double[2 * grado + 1],
                    XY = new double[grado + 1]
                };

                // Calcular potencias de X
                for (int i = 0; i <= 2 * grado; i++)
                {
                    resultado.PotenciasX[i] = Math.Pow(punto.X, i);
                }

                // Calcular X*Y para cada grado
                for (int i = 0; i <= grado; i++)
                {
                    resultado.XY[i] = punto.Y * Math.Pow(punto.X, i);
                }

                resultados.Add(resultado);
            }

            // Llenar la matriz de coeficientes
            for (int i = 0; i <= grado; i++)
            {
                for (int j = 0; j <= grado; j++)
                {
                    matrizA[i, j] = resultados.Sum(r => r.PotenciasX[i + j]);
                }
                vectorB[i] = resultados.Sum(r => r.XY[i]);
            }

            // Resolver el sistema de ecuaciones usando el metodo de Gauss
            var coeficientes = Gauss.Resolver(matrizA, vectorB);

            return (coeficientes, resultados);
        }

        /// <summary>
        /// Obtiene la ecuación del polinomio en formato legible
        /// </summary>
        public static string ObtenerEcuacion(double[] coeficientes)
        {
            var terminos = new List<string>();

            for (int i = 0; i < coeficientes.Length; i++)
            {
                if (Math.Abs(coeficientes[i]) < 1e-10) continue;

                string termino = $"{coeficientes[i]:F4}";
                if (i > 0)
                {
                    termino += $"x";
                    if (i > 1)
                    {
                        termino += $"^{i}";
                    }
                }
                terminos.Add(termino);
            }

            return $"y = {string.Join(" + ", terminos)}";
        }
    }

    /// <summary>
    /// Almacena los resultados parciales para la regresión polinomial
    /// </summary>
    public class ResultadosPolinomiales
    {
        public double X { get; set; }
        public double Y { get; set; }
        public required double[] PotenciasX { get; set; }  // Almacena X^1, X^2, ... X^(2n)
        public required double[] XY { get; set; }          // Almacena X^i*Y para cada grado
    }
}
