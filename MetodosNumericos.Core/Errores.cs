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
            if (valorReal <= 0)
            {
                throw new ArgumentException("El valor real debe ser mayor a cero.", nameof(valorReal));
            }
            if (ValorAproximado <= 0)
            {
                throw new ArgumentException("El valor aproximado debe ser mayor a cero.", nameof(ValorAproximado));
            }
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
            //error absoluto/ valor real
            if (valorReal <= 0)
            {
                throw new ArgumentException("El valor real debe ser mayor a cero.", nameof(valorReal));
            }
            if (ValorAproximado <= 0)
            {
                throw new ArgumentException("El valor aproximado debe ser mayor a cero.", nameof(ValorAproximado));
            }
            return Math.Abs((valorReal - ValorAproximado) / valorReal);
        }

        /// <summary>
        /// Metodo que toma el valor real y el valor aproximado y devuelve el error relativo porcentual
        /// </summary>
        /// <param name="valorReal"></param>
        /// <param name="ValorAproximado"></param>
        /// <returns></returns>
        public double CalcularErrorAproximadoRelativoPorcentual(double valorReal, double ValorAproximado)
        {
            if (valorReal <= 0)
            {
                throw new ArgumentException("El valor real debe ser mayor a cero.", nameof(valorReal));
            }
            if (ValorAproximado <= 0)
            {
                throw new ArgumentException("El valor aproximado debe ser mayor a cero.", nameof(ValorAproximado));
            }
            //error aproximado/ valor aproximado x 100%
            return (Math.Abs(valorReal - ValorAproximado) / Math.Abs(ValorAproximado)) * 100;
        }

    }
}
