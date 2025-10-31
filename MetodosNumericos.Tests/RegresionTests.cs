using MetodosNumericos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Tests
{
    public class RegresionTests
    {
        /// <summary>
        /// Probar que jala normal
        /// </summary>
        [Test]
        public void TestCalcularRegresionLineal()
        {
            // Arrange
            var puntos = new List<PuntoXY>
            {
                new(1, 2),
                new(2, 3),
                new(3, 5),
                new(4, 4)
            };
            // Act
            var sumas = Regresion.CalcularSumas(puntos);
            var resultado = Regresion.RegresionLineal(sumas);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(resultado.A0, Is.EqualTo(1.5).Within(0.01), "Interseccion incorrecta");
                Assert.That(resultado.A1, Is.EqualTo(0.8).Within(0.01), "Pendiente incorrecta");
            });
        }

        /// <summary>
        /// Prueba de pocos puntos
        /// </summary>
        [Test]
        public void TestCalcularRegresionLineal_InsufficientPoints()
        {
            // Arrange
            var puntos = new List<PuntoXY>
                {
                    new(1, 2)
                };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Regresion.CalcularSumas(puntos), "Deberia lanzar una excepcion por puntos insuficientes");
        }

        /// <summary>
        /// Prueba de que el denominador sea cero
        /// <summary>
        [Test]
        public void TestCalcularRegresionLineal_ZeroDenominator()
        {
            // Arrange
            var puntos = new List<PuntoXY>
            {
                    new(1, 2),
                    new(1, 3),
                    new(1, 4)
                };
            // Act
            var sumas = Regresion.CalcularSumas(puntos);
            // Assert
            Assert.Throws<InvalidOperationException>(() => Regresion.RegresionLineal(sumas), "Deberia lanzar una excepcion por denominador cero");
        }


    }
}
