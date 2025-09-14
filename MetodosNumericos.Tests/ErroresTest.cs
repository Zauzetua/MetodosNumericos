namespace MetodosNumericos.Tests
{
    public class ErroresTest
    {
        [Test]
        public void CalcularErrorAbsoluto_ValoresValidos_RetornaErrorCorrecto()
        {
            // Arrange
            var errores = new Core.Errores();
            double valorReal = 10.0;
            double valorAproximado = 9.5;

            // Act
            double resultado = errores.CalcularErrorAbsoluto(valorReal, valorAproximado);

            // Assert
            Assert.That(resultado, Is.EqualTo(0.5).Within(6)); // Tolerancia de 6 decimales
        }

        [Test]
        public void CalcularErrorRelativo_ValoresValidos_RetornaErrorCorrecto()
        {
            // Arrange
            var errores = new Core.Errores();
            double valorReal = 10.0;
            double valorAproximado = 9.5;

            // Act
            double resultado = errores.CalcularErrorRelativo(valorReal, valorAproximado);

            // Assert
            Assert.That(resultado, Is.EqualTo(0.05).Within(6)); // Tolerancia de 6 decimales
        }

        [Test]
        public void CalcularErrorAproximadoRelativoPorcentual_ValoresValidos_RetornaErrorCorrecto()
        {
            // Arrange
            var errores = new Core.Errores();
            double valorReal = 10.0;
            double valorAproximado = 9.5;

            // Act
            double resultado = errores.CalcularErrorAproximadoRelativoPorcentual(valorReal, valorAproximado);

            // Assert
            Assert.That(resultado, Is.EqualTo(5.0).Within(6)); // Tolerancia de 6 decimales
        }

        [Test]
        public void CalcularErrorAbsoluto_ValoresNegativos_DebeLanzarExcepcion()
        {
            // Arrange
            var errores = new Core.Errores();
            double valorReal = -10.0;
            double valorAproximado = 9.5;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => errores.CalcularErrorAbsoluto(valorReal, valorAproximado));
            Assert.That(ex.Message, Does.Contain("El valor real debe ser mayor a cero."));

        }

        [Test]
        public void CalcularErrorRelativo_ValoresNegativos_DebeLanzarExcepcion()
        {
            // Arrange
            var errores = new Core.Errores();
            double valorReal = 10.0;
            double valorAproximado = -9.5;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => errores.CalcularErrorRelativo(valorReal, valorAproximado));
            Assert.That(ex.Message, Does.Contain("El valor aproximado debe ser mayor a cero."));
        }

        [Test]
        public void CalcularErrorAproximadoRelativoPorcentual_ValoresNegativos_DebeLanzarExcepcion()
        {
            // Arrange
            var errores = new Core.Errores();
            double valorReal = -10.0;
            double valorAproximado = -9.5;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => errores.CalcularErrorAproximadoRelativoPorcentual(valorReal, valorAproximado));
            Assert.That(ex.Message, Does.Contain("El valor real debe ser mayor a cero."));
        }
    }
}
