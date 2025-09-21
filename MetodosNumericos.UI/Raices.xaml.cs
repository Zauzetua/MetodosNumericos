using MetodosNumericos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MetodosNumericos.Core.RaicesFuncion;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para Raices.xaml
    /// </summary>
    public partial class Raices : UserControl
    {
        public Raices()
        {
            InitializeComponent();
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {

            try
            {
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

                if (!double.TryParse(txtError.Text, out double error) || error <= 0)
                {
                    MessageBox.Show("El error maximo permitido debe de ser mayor que 0", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFuncion.Text))
                {
                    MessageBox.Show("Ingrese una funcion valida.", "Error");
                    return;
                }
                var raices = new RaicesFuncion();
                var opcion = cbxMetodo.SelectedIndex;
                List<ResultadoIteracion> resultadosPorIteracion = [];

                if (opcion == 0)
                {
                    resultadosPorIteracion = raices.Biseccion(txtFuncion.Text, xi, xf, error);
                }
                else if (opcion == 1)
                {
                    resultadosPorIteracion = raices.ReglaFalsa(txtFuncion.Text, xi, xf, error);
                }
                else if (opcion == 2)
                {
                    resultadosPorIteracion = raices.NewtonRaphson(txtFuncion.Text, xi, error);
                }
                else if (opcion == 3)
                {
                    resultadosPorIteracion = raices.Secante(txtFuncion.Text, xi, xf, error);
                }
                else
                {
                    MessageBox.Show("Seleccione un metodo.", "Error");
                    return;
                }
                if (resultadosPorIteracion.Last().Ea > error)
                {
                    MessageBox.Show("Se encontro la raiz. Pero no se llego al criterio de error maximo permitido.", "Atencion");
                }
                var formateados = raices.FormatearResultados(resultadosPorIteracion);
                dgTabla.ItemsSource = formateados;
                txtRaiz.Text = resultadosPorIteracion.Last().Xr.ToString("G4");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }

        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            txtFuncion.Clear();
            txtXi.Clear();
            if (cbxMetodo.SelectedIndex != 2) //Si el metodo no es Newton-Raphson, reiniciar Xf
                txtXf.Clear();
            txtError.Clear();
            txtRaiz.Clear();
            lblIteraciones.Content = "";
            dgTabla.ItemsSource = null;

        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            string general = "Use 'x' como variable independiente\nUse '.' para decimales\nNo use espacios innecesarios, puede ser todo pegado si gusta\n";
            string sqrt = "sqrt(...) para raiz cuadrada\n";
            string abs = "abs(...) para valor absoluto\n";
            string exp = "x^2 para raices\n";
            string multis = "x*3 o 3x o x*sqrt(...) para multiplicacion\n";
            MessageBox.Show("Lineamientos:\n" + general + sqrt + abs + exp + multis, "Ayuda");
        }

        private void cbxMetodo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Si el metodo es Newton-Raphson, deshabilitar el campo de Xf
            if (cbxMetodo.SelectedIndex == 2)
            {
                txtXf.IsEnabled = false;
                txtXf.Text = "0";
            }
            else
            {
                txtXf.IsEnabled = true;

            }

        }
    }
}
