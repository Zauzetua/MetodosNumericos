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
            var resultado = raices.Biseccion(funcion, xi, xf, eamax);

            // Assert
            Assert.That(2, Is.EqualTo(resultado.Last().Xr).Within(6));
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
            var resultado = raices.ReglaFalsa(funcion, xi, xf, eamax);

            // Assert
            Assert.That(2, Is.EqualTo(resultado.Last().Xr).Within(6));
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
        
        [Test]
        public void NewtonRaphson_EncuentraRaiz_Correctamente()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 3; // Punto inicial cercano a la raiz
            double eamax = 0.001;

            // Act
            var resultado = raices.NewtonRaphson(funcion, xi, eamax);

            // Assert
            Assert.That(2, Is.EqualTo(resultado.Last().Xr).Within(6));
        }

        [Test]
        public void NewtonRaphson_PuntoInicialNoValido_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0; // Punto inicial no valido
            double eamax = 0.001;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.NewtonRaphson(funcion, xi, eamax));
            Assert.That(ex.Message, Does.Contain("La derivada es cero, no se puede continuar con el metodo."));
        }

        [Test]
        public void NewtonRaphson_ErrorMaximoCero_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 3; // Punto inicial cercano a la raiz
            double eamax = 0; // Error maximo no puede ser cero

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.NewtonRaphson(funcion, xi, eamax));
            Assert.That(ex.Message, Does.Contain("El error maximo permitido debe de ser mayor que 0"));
        }

        [Test]
        public void NewtonRaphson_FuncionNoValida_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 -"; // Funcion no valida
            double xi = 3; // Punto inicial cercano a la raiz
            double eamax = 0.001;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => raices.NewtonRaphson(funcion, xi, eamax));
            Assert.That(ex.Message, Does.Contain("Error al convertir la funcion, asegurese de haberla escrito bien"));
        }

        [Test]
        public void NewtonRaphson_FuncionSinRaiz_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 + 4"; // No tiene raices reales
            double xi = 3; // Punto inicial
            double eamax = 0.001;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => raices.NewtonRaphson(funcion, xi, eamax));
            Assert.That(ex.Message, Does.Contain("Se alcanzo el numero maximo de iteraciones sin converger."));
        }

        [Test]
        public void Secante_EncuentraRaiz_Correctamente()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act
            var resultado = raices.Secante(funcion, xi, xf, eamax);

            // Assert
            Assert.That(2, Is.EqualTo(resultado.Last().Xr).Within(6));
        }

        [Test]
        public void Secante_ErrorMaximoCero_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 - 4"; // La raiz es 2
            double xi = 0;
            double xf = 3;
            double eamax = 0; // Error maximo no puede ser cero

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.Secante(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("El error maximo permitido debe de ser mayor que 0"));
        }

        [Test]
        public void Secante_FuncionNoValida_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 -"; // Funcion no valida
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => raices.Secante(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("Error al convertir la funcion, asegurese de haberla escrito bien"));
        }

        [Test]
        public void Secante_FuncionSinRaiz_ArgumentException()
        {
            // Arrange
            var raices = new Core.RaicesFuncion();
            string funcion = "x^2 + 4"; // No tiene raices reales
            double xi = 0;
            double xf = 3;
            double eamax = 0.01;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => raices.Secante(funcion, xi, xf, eamax));
            Assert.That(ex.Message, Does.Contain("La funcion no tiene raices en el intervalo dado."));
        }
    }
}
