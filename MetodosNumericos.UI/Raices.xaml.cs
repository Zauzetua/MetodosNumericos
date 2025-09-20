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
                    MessageBox.Show("Ingrese un numero valido en Xi.","Error");
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
                (double raiz, double iteraciones) resultados;
                if (opcion == 0)
                {
                    resultados = raices.Biseccion(txtFuncion.Text, xi, xf, error);
                }
                else if (opcion == 1)
                {
                    resultados = raices.ReglaFalsa(txtFuncion.Text, xi, xf, error);
                }
                else
                {
                    MessageBox.Show("Seleccione un metodo.", "Error");
                    return;
                }

                txtRaiz.Text = resultados.raiz.ToString("G6");
                //lblIteraciones.Content = "Iteraciones: " + resultados.iteraciones.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,"Error");
            }

        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            txtFuncion.Clear();
            txtXi.Clear();
            txtXf.Clear();
            txtError.Clear();
            txtRaiz.Clear();
            lblIteraciones.Content = "";

        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            string general = "Use 'x' como variable independiente\nUse '.' para decimales\nNo use espacios innecesarios, puede ser todo pegado si gusta\n";
            string sqrt = "sqrt(x) para raiz cuadrada\n";
            string abs = "abs(x) para valor absoluto\n";
            string exp = "x^2 para raices\n";
            string multis = "x*3 o 3x o x*sqrt(...) para multiplicacion\n";
            MessageBox.Show("Lineamientos:\n" +general + sqrt + abs + exp + multis,"Ayuda");
        }
    }
}
