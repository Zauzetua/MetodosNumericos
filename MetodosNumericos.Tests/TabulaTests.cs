using MetodosNumericos.Core;
using Xunit;
using static MetodosNumericos.Core.Tabula;

namespace MetodosNumericos.Tests
{
    public class TabulaTests
    {
        [Test]
        public void Tabular_DebeRetornarCorrecto()
        {
            // Arrange
            var tabula = new Tabula();
            Func<double, double> funcion = x => x * x; // y = x^2
            double xi = 0;
            double xf = 2;
            double incx = 1;

            // Act
            var resultados = tabula.Tabular(funcion, xi, xf, incx, out Resultado minimo, out Resultado maximo);

            // Assert
            var resultadosEsperados = new List<Resultado>
            {
                new Resultado { x = 0, y = 0 },
                new Resultado { x = 1, y = 1 },
                new Resultado { x = 2, y = 4 }
            };

            for (int i = 0; i < resultadosEsperados.Count; i++)
            {
                if (resultados[i].x != resultadosEsperados[i].x || resultados[i].y != resultadosEsperados[i].y)
                {
                    throw new Exception($"Test failed at index {i}: expected ({resultadosEsperados[i].x}, {resultadosEsperados[i].y}), got ({resultados[i].x}, {resultados[i].y})");
                }
            }

            if (minimo.x != 0 || minimo.y != 0)
            {
                throw new Exception($"Minimo test failed: expected (0, 0), got ({minimo.x}, {minimo.y})");
            }

            if (maximo.x != 2 || maximo.y != 4)
            {
                throw new Exception($"Maximo test failed: expected (2, 4), got ({maximo.x}, {maximo.y})");
            }

        }

        [Test]
        public void Tabular_IncrementoCero_DebeLanzarExcepcion()
        {
            // Arrange
            var tabula = new Tabula();
            Func<double, double> funcion = x => x * x; // y = x^2
            double xi = 0;
            double xf = 2;
            double incx = 0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                tabula.Tabular(funcion, xi, xf, incx, out Resultado minimo, out Resultado maximo);
            });
        }

        [Test]
        public void Tabular_XiMayorQueXf_DebeLanzarExcepcion()
        {
            // Arrange
            var tabula = new Tabula();
            Func<double, double> funcion = x => x * x; // y = x^2
            double xi = 2;
            double xf = 0;
            double incx = 1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                tabula.Tabular(funcion, xi, xf, incx, out Resultado minimo, out Resultado maximo);
            });
        }

        [Test]
        public void Tabular_FuncionNula_DebeLanzarExcepcion()
        {
            // Arrange
            var tabula = new Tabula();
            Func<double, double> funcion = null; // Funcion nula
            double xi = 0;
            double xf = 2;
            double incx = 1;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                tabula.Tabular(funcion, xi, xf, incx, out Resultado minimo, out Resultado maximo);
            });
        }



    }
}
