using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodosNumericos.Tests
{
    public class InterpolacionTests
    {
        [Test]
        public void Lagrange_Interpolacion_CalculaValorCorrecto()
        {
            // Datos de prueba
            double[] xs = [2, 3, 5, 6];
            double[] ys = [4,5.25,19.75,36];
            double xi = 3.5;

            // Valor esperado calculado manualmente
            double expectedYi = 7.093;

            // Llamada al método de interpolacion
            double yi = Core.Interpolacion.Lagrange(xs, ys, xi);

            // Verificacion del resultado
            Assert.That(yi, Is.EqualTo(expectedYi).Within(1e-3), "El valor interpolado no es correcto.");
        }

        [Test]
        public void Lagrange_Interpolacion_ArreglosDeDiferenteLongitud_LanzaExcepcion()
        {
            // Datos de prueba con arreglos de diferente longitud
            double[] xs = [1, 2, 3];
            double[] ys = [4, 5];

            // Verificacion de que se lanza una excepcion
            Assert.Throws<ArgumentException>(() => Core.Interpolacion.Lagrange(xs, ys, 2.5), "No se lanzo la excepción esperada para arreglos de diferente longitud.");
        }

        [Test]
        public void Lagrange_Interpolacion_ArreglosVacios_LanzaExcepcion()
        {
            // Datos de prueba con arreglos vacios
            double[] xs = [];
            double[] ys = [];

            // Verificacion de que se lanza una excepcion
            Assert.Throws<ArgumentException>(() => Core.Interpolacion.Lagrange(xs, ys, 2.5), "No se lanzo la excepción esperada para arreglos vacios.");
        }


    }
}
