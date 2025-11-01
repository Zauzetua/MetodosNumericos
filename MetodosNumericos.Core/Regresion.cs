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
        /// Calcula los coeficientes de la regresion polinomial
        /// </summary>
        /// <param name="puntos">Lista de puntos XY</param>
        /// <param name="grado">Grado del polinomio</param>
        /// <returns>Coeficientes y resultados  </returns>
        public static (double[] coeficientes, List<ResultadosPolinomiales> resultados) CalcularRegresionPolinomial(List<PuntoXY> puntos, int grado)
        {
            if (puntos == null || puntos.Count < grado + 1)
            {
                throw new ArgumentException($"Se necesitan al menos {grado + 1} puntos para una regresion de grado {grado}");
            }

            int n = puntos.Count;
            var resultados = new List<ResultadosPolinomiales>();

            // Crear la matriz de coeficientes y el vector de terminos independientes (matriz ps)
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
        /// Obtiene la ecuacion del polinomio en formato legible
        /// </summary>
        public static string ObtenerEcuacion(double[] coeficientes)
        {
            var terminos = new List<string>();

            for (int i = 0; i < coeficientes.Length; i++)
            {
                if (Math.Abs(coeficientes[i]) < 1e-10) continue;

                string termino = $"{coeficientes[i]:F4}"; //6 ya son muchos
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
        public required double[] PotenciasX { get; set; }  // Almacena X^1, X^2...
        public required double[] XY { get; set; }          // Almacena X^i*Y para cada grado
    }

    /// <summary>
    /// Representa un punto en el espacio multidimensional
    /// </summary>
    public class PuntoMultiple
    {
        /// <summary>
        /// Variables independientes
        /// </summary>
        public double[] X { get; set; }

        /// <summary>
        /// Variable dependiente (Y)
        /// </summary>
        public double Y { get; set; }

        public PuntoMultiple(double[] x, double y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Contiene los resultados de un calculo de regresion lineal multiple
    /// </summary>
    public class ResultadoRegresionMultiple
    {
        /// <summary>
        /// Termino independiente 
        /// </summary>
        public double B0 { get; set; }

        /// <summary>
        /// Coeficientes de las variables independientes
        /// </summary>
        public required double[] Coeficientes { get; set; }

        /// <summary>
        /// Obtiene la ecuacion de regresion en formato legible
        /// </summary>
        public string ObtenerEcuacion()
        {
            var terminos = new List<string>
            {
                $"{B0:F4}"
            };

            for (int i = 0; i < Coeficientes.Length; i++)
            {
                if (Math.Abs(Coeficientes[i]) < 1e-10) continue;
                terminos.Add($"{Coeficientes[i]:F4}X{i + 1}");
            }

            return $"Y = {string.Join(" + ", terminos)}";
        }
    }

    /// <summary>
    /// Clase que maneja la regresion lineal multiple
    /// </summary>
    public class RegresionLinealMultiple
    {
        /// <summary>
        /// Calcula los coeficientes de la regresion lineal multiple usando el metodo de minimos cuadrados
        /// </summary>
        /// <param name="puntos">Lista de puntos multidimensionales</param>
        /// <returns>Resultado con los coeficientes</returns>
        public static ResultadoRegresionMultiple CalcularRegresion(List<PuntoMultiple> puntos)
        {
            if (puntos == null || !puntos.Any())
                throw new ArgumentException("La lista de puntos no puede estar vacia");

            int n = puntos.Count; // Numero de observaciones
            int k = puntos[0].X.Length; // Numero de variables independientes

            if (n < k + 1)
                throw new ArgumentException($"Se necesitan al menos {k + 1} observaciones para {k} variables independientes");

            // Crear la matriz X (incluyendo columna de 1s para β₀)
            double[,] matrizX = new double[n, k + 1];
            double[] vectorY = new double[n];

            // Llenar la matriz X y el vector Y
            for (int i = 0; i < n; i++)
            {
                matrizX[i, 0] = 1; // Termino constante
                for (int j = 0; j < k; j++)
                {
                    matrizX[i, j + 1] = puntos[i].X[j];
                }
                vectorY[i] = puntos[i].Y;
            }

            // Calcular X'X
            double[,] xtx = new double[k + 1, k + 1];
            for (int i = 0; i <= k; i++)
                for (int j = 0; j <= k; j++)
                    for (int m = 0; m < n; m++)
                        xtx[i, j] += matrizX[m, i] * matrizX[m, j];

            // Calcular X'Y
            double[] xty = new double[k + 1];
            for (int i = 0; i <= k; i++)
                for (int m = 0; m < n; m++)
                    xty[i] += matrizX[m, i] * vectorY[m];

            // Resolver el sistema (X'X)β = X'Y usando Gauss
            var coeficientes = Gauss.Resolver(xtx, xty); //amo reutilizar el metodo de Gauss

            // Calcular valores predichos
            double ssResidual = 0;
            for (int i = 0; i < n; i++)
            {
                double yPredicho = coeficientes[0]; // a
                for (int j = 0; j < k; j++)
                {
                    yPredicho += coeficientes[j + 1] * puntos[i].X[j];
                }
                ssResidual += Math.Pow(vectorY[i] - yPredicho, 2);
            }

            return new ResultadoRegresionMultiple
            {
                B0 = coeficientes[0],
                Coeficientes = coeficientes.Skip(1).ToArray(),
            };
        }
    }
}
