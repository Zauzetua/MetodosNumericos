using NUnit.Framework;
using MetodosNumericos.Core;

namespace MetodosNumericos.Tests
{
    public class GaussTests
    {
        [Test]
        public void Solve_2x2_Works()
        {
            double[,] A = new double[,] { { 3, 2 }, { 1, 2 } };
            double[] b = new double[] { 5, 5 };
            double[] x = Gauss.Resolver(A, b);
            Assert.AreEqual(0.0, x[0], 1e-9);
            Assert.AreEqual(2.5, x[1], 1e-9);
        }

        [Test]
        public void Solve_3x3_Works()
        {
            double[,] A = new double[,] {
                { 2, -1, 0 },
                { -1, 2, -1 },
                { 0, -1, 2 }
            };
            double[] b = new double[] { 1, 0, 1 };
            double[] x = Gauss.Resolver(A, b);
            // Expected solution obtained analytically
            Assert.AreEqual(1.0, x[0], 1e-9);
            Assert.AreEqual(1.0, x[1], 1e-9);
            Assert.AreEqual(1.0, x[2], 1e-9);
        }

        [Test]
        public void Solve_4x4_Works()
        {
            double[,] A = new double[,] {
                {4, 1, 2, -1},
                {3, 6, -1, 2},
                {2, -1, 5, -3},
                {4, 1, -3, -8}
            };
            double[] b = new double[] { 5, 9, 1, -6 };
            double[] x = Gauss.Resolver(A, b);
            // Validate Ax ~= b
            double[] Ax = new double[4];
            for (int i = 0; i < 4; i++)
            {
                double sum = 0;
                for (int j = 0; j < 4; j++) sum += A[i, j] * x[j];
                Ax[i] = sum;
                Assert.AreEqual(b[i], Ax[i], 1e-8);
            }
        }

        [Test]
        public void Solve_Pivoting_Required_Works()
        {
            // Se ocupa pivoteo
            double[,] A = new double[,] {
                { 0, 2, 3 },
                { 1, -1, 1 },
                { 2, 3, 1 }
            };
            double[] b = [5, 2, 4];
            double[] x = Gauss.Resolver(A, b);
            double[] Ax = new double[3];
            for (int i = 0; i < 3; i++)
            {
                double sum = 0;
                for (int j = 0; j < 3; j++) sum += A[i, j] * x[j];
                Ax[i] = sum;
                Assert.AreEqual(b[i], Ax[i], 1e-8);
            }
        }
    }
}
