using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Core
{
    public class Gauss
    {

        /// <summary>
        /// Metodop que resuelve un sistema de ecuaciones lineales Ax = b usando eliminacion gaussiana con pivoteo parcial
        /// </summary>
        /// <param name="A"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static double[] Resolver(double[,] A, double[] b)
        {
            ArgumentNullException.ThrowIfNull(A);
            ArgumentNullException.ThrowIfNull(b);

            int n = A.GetLength(0);
            if (A.GetLength(1) != n) throw new ArgumentException("La matriz A debe ser cuadrada");
            if (b.Length != n) throw new ArgumentException("El vector b debe tener la misma cantidad de filas que A");
            if (n < 2 || n > 4) throw new ArgumentException("Solo se permiten tamaños de 2x2 hasta 4x4");

            // Trabajar sobre la matriz aumentada
            double[,] matrizAumentada = Matrices.FormarMatriz(A, b);
            // Clonar para no modificar las matrices originales del usuario
            matrizAumentada = Matrices.ClonarMatriz(matrizAumentada);

            int filas = n;
            int columnas = n + 1;

            // Eliminacion hacia adelante con pivoteo parcial
            for (int k = 0; k < n; k++)
            {
                // Buscar la fila pivote (la de mayor valor absoluto en la columna actual)
                int filaPivote = Matrices.EncontrarFilaPivot(matrizAumentada, k, k);

                // Verificar si la matriz es singular (no tiene solucion unica)
                if (Math.Abs(matrizAumentada[filaPivote, k]) < 1e-14)
                    throw new ArgumentException("La matriz es singular o casi singular, no tiene solucion unica");

                // Intercambiar la fila actual con la fila pivote, hora de pivotear
                if (filaPivote != k)
                    Matrices.IntercambiarFilas(matrizAumentada, filaPivote, k);

                // Eliminar los valores debajo del pivote
                for (int i = k + 1; i < n; i++)
                {
                    double factor = matrizAumentada[i, k] / matrizAumentada[k, k];
                    matrizAumentada[i, k] = 0.0; // forzar a cero
                    for (int j = k + 1; j < columnas; j++)
                    {
                        matrizAumentada[i, j] -= factor * matrizAumentada[k, j];
                    }
                }
            }

            // Sustitucion hacia atras para obtener las soluciones
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double suma = matrizAumentada[i, columnas - 1];
                for (int j = i + 1; j < n; j++)
                    suma -= matrizAumentada[i, j] * x[j];
                x[i] = suma / matrizAumentada[i, i];
            }

            return x;
        }

        /// <summary>
        /// Metodo que resuelve un sistema de ecuaciones lineales representado por una matriz aumentada [A|b] usando eliminacion gaussiana con pivoteo parcial
        /// </summary>
        /// <param name="matrizAumentada"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static double[] ResolverDesdeAumentada(double[,] matrizAumentada)
        {
            ArgumentNullException.ThrowIfNull(matrizAumentada);

            int n = matrizAumentada.GetLength(0);
            int columnas = matrizAumentada.GetLength(1);
            if (columnas != n + 1) throw new ArgumentException("La matriz aumentada debe tener n+1 columnas");
            if (n < 2 || n > 4) throw new ArgumentException("Solo se permiten tamaños de 2x2 hasta 4x4");

            // Clonar para no alterar la original
            double[,] trabajo = Matrices.ClonarMatriz(matrizAumentada);

            // Eliminacion hacia adelante con pivoteo parcial
            for (int k = 0; k < n; k++)
            {
                int filaPivote = Matrices.EncontrarFilaPivot(trabajo, k, k);
                if (Math.Abs(trabajo[filaPivote, k]) < 1e-14)
                    throw new ArgumentException("La matriz es singular o casi singular");
                if (filaPivote != k)
                    Matrices.IntercambiarFilas(trabajo, filaPivote, k);

                for (int i = k + 1; i < n; i++)
                {
                    double factor = trabajo[i, k] / trabajo[k, k];
                    trabajo[i, k] = 0.0;
                    for (int j = k + 1; j < columnas; j++)
                        trabajo[i, j] -= factor * trabajo[k, j];
                }
            }

            // Sustitucion hacia atras
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double suma = trabajo[i, columnas - 1];
                for (int j = i + 1; j < n; j++)
                    suma -= trabajo[i, j] * x[j];
                x[i] = suma / trabajo[i, i];
            }

            return x;
        }
    }

}
