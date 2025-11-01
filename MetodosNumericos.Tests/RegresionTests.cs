using MetodosNumericos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Tests
{
    /// <summary>
    /// Clase de pruebas para regresion
    /// </summary>
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

        /// <summary>
        /// Metodo que prueba la regresion polinomial de grado 2
        /// </summary>
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

        /// <summary>
        /// Prueba de regresion polinomial con puntos insuficientes
        /// </summary>
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

        /// <summary>
        /// Prueba de regresion lineal multiple, me agarre un ejemplo de youtube
        /// </summary>
        [Test]
        public void TestCalcularRegresionLinealMultiple()
        {
            // Arrange
            var datos = new List<PuntoMultiple>
            {
                new([2.6, 27], 40), //1
                new([4, 21], 24), //2
                new([3.4, 15], 20), //3
                new([3, 42], 48), //4
                new([3.2, 45], 40), //5
                new([2.4, 36], 60), //6
                new([3.20, 18], 20), //7
                new([2.80, 30], 48), //8
                new([2.00, 45], 68),
                new([2.2, 63], 80),

            };
            // Act
            var resultado= RegresionLinealMultiple.CalcularRegresion(datos);
            var ecuacion = resultado.ObtenerEcuacion();

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(resultado.B0, Is.EqualTo(65.6).Within(0.1), "Coeficiente B0 incorrecto");
                Assert.That(resultado.Coeficientes[0], Is.EqualTo(-16.4).Within(0.1), "Coeficiente B1 incorrecto");
                Assert.That(resultado.Coeficientes[1], Is.EqualTo(0.78).Within(0.1), "Coeficiente B2 incorrecto");

            });
        }

        /// <summary>
        /// prueba de regresion lineal multiple con puntos insuficientes
        /// </summary>
        [Test]
        public void TestCalcularRegresionLinealMultiple_InsufficientPoints()
        {
            // Arrange
            var datos = new List<PuntoMultiple>
            {
                new([2.6, 27], 40), //1
            };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => RegresionLinealMultiple.CalcularRegresion(datos), "Deberia lanzar una excepcion por puntos insuficientes");
        }

    }
}
