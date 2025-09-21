namespace MetodosNumericos.Core
{
    public class Derivadas
    {
        /// <summary>
        /// Metodo que deriva una funcion en un punto x usando la definicion de la derivada.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Derivar(Func<double, double> f, double x)
        {
            double h = 1e-6;
            var resultado = (f(x + h) - f(x - h)) / (h);
            return resultado;
        }
    }
}
