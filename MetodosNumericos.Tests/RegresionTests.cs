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

        //Polinomiales

        [Test]
        public void TestCalcularRegresionPolinomial_Grado2()
        {
            // Arrange
            var puntos = new List<PuntoXY>
            {
                new(1, 6),
                new(2, 11),
                new(3, 18),
                new(4, 27)
            };
            // Act
            var (coeficientes, resultados) = RegresionPolinomial.CalcularRegresionPolinomial(puntos,2);
            var ecuacion = RegresionPolinomial.ObtenerEcuacion(coeficientes);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(coeficientes, Has.Length.EqualTo(3), "Numero de coeficientes incorrecto");
                Assert.That(coeficientes[0], Is.EqualTo(3).Within(0.01), "Coeficiente a0 incorrecto");
                Assert.That(coeficientes[1], Is.EqualTo(2).Within(0.01), "Coeficiente a1 incorrecto");
                Assert.That(coeficientes[2], Is.EqualTo(1).Within(0.01), "Coeficiente a2 incorrecto");
                Assert.That(ecuacion, Is.EqualTo("y = 3.0000 + 2.0000x + 1.0000x^2"), "Ecuacion incorrecta");
            });
        }

        //prueba de grado insuficiente
        [Test]
        public void TestCalcularRegresionPolinomial_InsufficientPoints()
        {
            // Arrange
            var puntos = new List<PuntoXY>
            {
                    new(1, 2),
                    new(2, 3)
                };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RegresionPolinomial.CalcularRegresionPolinomial(puntos, 2), "Deberia lanzar una excepcion por puntos insuficientes para el grado");
        }

    }
}
