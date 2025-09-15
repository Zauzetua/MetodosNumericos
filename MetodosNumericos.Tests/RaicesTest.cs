using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Tests
{
    public class RaicesTest
    {
        [Test]
        public void Biseccion_EncuentraRaiz_Correctamente()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act
            var (raiz, iteraciones) = raices.Biseccion(funcion, xi, xf, eamax);

            // Assert
            Assert.That(2, Is.EqualTo(raiz).Within(6));
        }
        [Test]
        public void Biseccion_FuncionSinRaiz_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 + 4"; // No tiene raices reales
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.Biseccion(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("La funcion no tiene raices en el intervalo dado."));
        }

        [Test]
        public void Biseccion_IntervaloInvalido_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 3;
            double xf = 5; // Ambos valores positivos, no hay cambio de signo
            double eamax = 0.01;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.Biseccion(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("La funcion no tiene raices en el intervalo dado."));
        }

        [Test]
        public void Biseccion_ErrorMaximoCero_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0;
            double xf = 3;
            double eamax = 0; // Error maximo no puede ser cero

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.Biseccion(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("El error maximo permitido debe de ser mayor que 0"));
        }

        [Test]
        public void ReglaFalsa_EncuentraRaiz_Correctamente()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act
            var (raiz, iteraciones) = raices.ReglaFalsa(funcion, xi, xf, eamax);

            // Assert
            Assert.That(2, Is.EqualTo(raiz).Within(6));
        }

        [Test]
        public void ReglaFalsa_FuncionSinRaiz_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 + 4"; // No tiene raices reales
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.ReglaFalsa(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("La funcion no tiene raices en el intervalo dado."));
        }

        [Test]
        public void ReglaFalsa_IntervaloInvalido_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 3;
            double xf = 5; // Ambos valores positivos, no hay cambio de signo
            double eamax = 0.01;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.ReglaFalsa(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("La funcion no tiene raices en el intervalo dado."));
        }

        [Test]
        public void ReglaFalsa_ErrorMaximoCero_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0;
            double xf = 3;
            double eamax = 0; // Error maximo no puede ser cero

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.ReglaFalsa(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("El error maximo permitido debe de ser mayor que 0"));
        }

    }
}
