using MetodosNumericos.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para TabulacionView.xaml
    /// </summary>
    public partial class TabulacionView : UserControl
    {
        /// <summary>
        /// Instancia de la clase Tabula para realizar la tabulación de funciones.
        /// </summary>
        private readonly Tabula _tabula = new Tabula();
        public TabulacionView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento click del boton para realizar la tabulacion de la funcion seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTabular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Obtener los valores de entrada
                if (!double.TryParse(txtXi.Text, out double xi))
                {
                    MessageBox.Show("Ingrese un numero valido en Xi.", "Error");
                    return;
                }
                if (!double.TryParse(txtXf.Text, out double xf))
                {
                    MessageBox.Show("Ingrese un numero valido en Xf.", "Error");
                    return;
                }
                if (!double.TryParse(txtIncx.Text, out double incx) || incx <= 0)
                {
                    MessageBox.Show("Ingrese un incremento valido (mayor a 0).", "Error");
                    return;
                }
                //Validar el rango
                if (xi >= xf)
                {
                    MessageBox.Show("Xi debe ser menor que Xf.", "Error");
                    return;
                }

                //Elegir la funcion a tabular
                Func<double, double> f;
                if (cbxFuncion.SelectedIndex == 0)
                    f = x => 4 * Math.Pow(x, 3) - 6 * Math.Pow(x, 2) + 7 * x - 2.3; // y = 4x^3 - 6x^2 + 7x - 2.3
                else if (cbxFuncion.SelectedIndex == 1)
                    f = x => Math.Pow(x, 2) * Math.Sqrt(Math.Abs(Math.Cos(x))) - 5; // y = x^2 * sqrt(|cos(x)|) - 5
                else
                {
                    MessageBox.Show("Seleccione una funcion valida.", "Error");
                    return;
                }



                var resultados = _tabula.Tabular(f, xi, xf, incx, out var minimo, out var maximo);

                dgResultados.ItemsSource = resultados;

                MessageBox.Show($"Minimo: X={minimo.x:F6}, Y={minimo.y:F6}\n" +
                            $"Maximo: X={maximo.x:F6}, Y={maximo.y:F6}");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
