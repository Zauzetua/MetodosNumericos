namespace MetodosNumericos.Core
{
    public class Matrices
    {
        /// <summary>
        /// Este metodo recibe la matriz de los coeficientes y el vector de terminos independientes. Forma la matriz chingona para Gauss
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Si alguna de las 2 matrices es null</exception>
        public static double[,] FormarMatriz(double[,] A, double[] b)
        {
            ArgumentNullException.ThrowIfNull(A); // Si no hay coeficientes.

            ArgumentNullException.ThrowIfNull(b); // Si no hay terminos independientes.
            int n = A.GetLength(0);
            if (A.GetLength(1) != n) throw new ArgumentException("La matriz A no es correcta");
            if (b.Length != n) throw new ArgumentException("Hay mas/menos terminos independientes de los que deberia");

            double[,] matriz = new double[n, n + 1]; //Formar la matriz chida
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matriz[i, j] = A[i, j];
                }
                matriz[i, n] = b[i];
            }
            return matriz;
        }

        /// <summary>
        /// Metodo que intercambia dos filas de una matriz, sirve para pivotear gg
        /// </summary>
        /// <param name="matriz"></param>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <exception cref="ArgumentNullException">Si alguna de las filas es null</exception>
        public static void IntercambiarFilas(double[,] matriz, int r1, int r2)
        {
            ArgumentNullException.ThrowIfNull(matriz);
            int columnas = matriz.GetLength(1);
            for (int j = 0; j < columnas; j++) //Intercambiar las filas
            {
                (matriz[r2, j], matriz[r1, j]) = (matriz[r1, j], matriz[r2, j]);
            }
        }

        /// <summary>
        /// Metodo que encuentra y devuelve la fila con el mayor valor absoluto en una columna dada, empezando desde una fila dada
        /// </summary>
        /// <param name="M"></param>
        /// <param name="col"></param>
        /// <param name="filaInicial"></param>
        /// <returns></returns>
        public static int EncontrarFilaPivot(double[,] M, int col, int filaInicial)
        {
            int filas = M.GetLength(0);
            int candidato = filaInicial;
            double mejorValor = Math.Abs(M[filaInicial, col]);
            for (int i = filaInicial + 1; i < filas; i++)
            {
                double val = Math.Abs(M[i, col]);
                if (val > mejorValor)
                {
                    mejorValor = val;
                    candidato = i;
                }
            }
            return candidato;
        }

        /// <summary>
        /// Metodo que clona una matriz, para poder moverle a una sin cambiar la original
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static double[,] ClonarMatriz(double[,] M)
        {
            if (M == null) return null;
            int r = M.GetLength(0);
            int c = M.GetLength(1);
            double[,] outM = new double[r, c];
            for (int i = 0; i < r; i++) for (int j = 0; j < c; j++) outM[i, j] = M[i, j];
            return outM;
        }
    }
}
