using MetodosNumericos.Core;
using System.Windows;
using System.Windows.Controls;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para CalcularErrores.xaml
    /// </summary>
    public partial class CalcularErrores : UserControl
    {
        private readonly Errores _errores = new Errores();
        public CalcularErrores()
        {
            InitializeComponent();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Obtener los valores de entrada
                if (!double.TryParse(txtAprox.Text, out double valorAproximado))
                {
                    MessageBox.Show("Ingrese un numero valido en Valor Aproximado/Obtenido.", "Error");
                    return;
                }
                if (!double.TryParse(txtReal.Text, out double valorVerdadero))
                {
                    MessageBox.Show("Ingrese un numero valido en Valor Verdadero.", "Error");
                    return;
                }
                //Validar que los valores no sean cero para evitar division por cero
                if (valorVerdadero<=0)
                {
                    MessageBox.Show("El Valor Verdadero debe ser mayor a cero.", "Error");
                    return;
                }
                if (valorAproximado <= 0)
                {
                    MessageBox.Show("El Valor Aproximado debe ser mayor a cero.", "Error");
                    return;
                }

                //Calcular los errores
                double errorAbsoluto = _errores.CalcularErrorAbsoluto(valorAproximado, valorVerdadero);
                double errorRelativo = _errores.CalcularErrorRelativo(valorAproximado, valorVerdadero);
                string errorRelativoString = errorRelativo.ToString("P6"); // Formatear como porcentaje
                double errorAproximadoRelativoPorcentual = _errores.CalcularErrorAproximadoRelativoPorcentual(valorAproximado, valorVerdadero);
                //Mostrar los resultados
                txtAbs.Text = errorAbsoluto.ToString("G6");
                txtRel.Text = errorRelativoString;
                txtRelPor.Text = errorAproximadoRelativoPorcentual.ToString("F6") + " %";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error: {ex.Message}","Error");
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtAprox.Clear();
            txtReal.Clear();
            txtAbs.Clear();
            txtRel.Clear();
            txtRelPor.Clear();

        }
    }
}
