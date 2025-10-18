using System;
using NUnit.Framework;
using MetodosNumericos.Core;

namespace MetodosNumericos.Tests
{
    public class GaussSeidelTests
    {
        [Test]
        public void GaussSeidel_Converges_On2x2_DiagonallyDominant()
        {
            // 4x + y = 9
            // x + 3y = 5
            double[,] A = new double[,] { { 4, 1 }, { 1, 3 } };
            double[] b = new double[] { 9, 5 };

            var (iter, errs) = Gauss.GaussSeidel(A, b, tol: 1e-10, maxIter: 1000);
            var xgs = iter[iter.Length - 1];
            var xexact = Gauss.Resolver(A, b);

            Assert.That(xgs[0], Is.EqualTo(xexact[0]).Within(1e-9));
            Assert.That(xgs[1], Is.EqualTo(xexact[1]).Within(1e-9));
            var lastErr = errs[errs.Length - 1];
            Assert.IsTrue(lastErr[0] < 1e-10 && lastErr[1] < 1e-10);
        }

        [Test]
        public void GaussSeidel_Converges_On3x3_DiagonallyDominant()
        {
            // A is strictly diagonally dominant
            double[,] A = new double[,]
            {
                { 10, -1, 2 },
                { -1, 11, -1 },
                { 2, -1, 10 }
            };
            double[] b = new double[] { 6, 25, -11 };

            var (iter, errs) = Gauss.GaussSeidel(A, b, tol: 1e-9, maxIter: 10000);
            var xgs = iter[iter.Length - 1];
            var xexact = Gauss.Resolver(A, b);

            Assert.That(xgs[0], Is.EqualTo(xexact[0]).Within(1e-8));
            Assert.That(xgs[1], Is.EqualTo(xexact[1]).Within(1e-8));
            Assert.That(xgs[2], Is.EqualTo(xexact[2]).Within(1e-8));
            var lastErr = errs[errs.Length - 1];
            Assert.IsTrue(lastErr[0] < 1e-9 && lastErr[1] < 1e-9 && lastErr[2] < 1e-9);
        }

        [Test]
        public void GaussSeidel_Throws_When_Diagonal_Zero()
        {
            double[,] A = new double[,] { { 0, 1 }, { 1, 2 } }; // A[0,0] == 0
            double[] b = new double[] { 1, 2 };

            Assert.Throws<ArgumentException>(() => Gauss.GaussSeidel(A, b));
        }
    }
}
