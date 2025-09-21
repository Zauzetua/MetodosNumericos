using MetodosNumericos.Core;
using System;
using System.Collections;
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
    /// Lógica de interacción para ComparativaRaices.xaml
    /// </summary>
    public partial class ComparativaRaices : UserControl
    {
        public ComparativaRaices()
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
                var resultadosBiseccion = raices.FormatearResultados(raices.Biseccion(txtFuncion.Text, xi, xf, error)).Last();
                var resultadosRetglaFalsa = raices.FormatearResultados(raices.ReglaFalsa(txtFuncion.Text, xi, xf, error)).Last();
                var resultadosSecante = raices.FormatearResultados(raices.Secante(txtFuncion.Text, xi, xf, error)).Last();
                var resultadosNewtonRaphson = raices.FormatearResultados(raices.NewtonRaphson(txtFuncion.Text, xi, error)).Last();

                var modeloDatos = new object[]
                {
                    new { Metodo = "Biseccion", Iteraciones=resultadosBiseccion.Iteracion, resultadosBiseccion.Xr, resultadosBiseccion.Fxr, resultadosBiseccion.Ea},
                    new { Metodo = "Regla Falsa", Iteraciones=resultadosRetglaFalsa.Iteracion, resultadosRetglaFalsa.Xr, resultadosRetglaFalsa.Fxr, resultadosRetglaFalsa.Ea},
                    new { Metodo = "Secante", Iteraciones=resultadosSecante.Iteracion, resultadosSecante.Xr, resultadosSecante.Fxr, resultadosSecante.Ea},
                    new { Metodo = "Newton Raphson", Iteraciones=resultadosNewtonRaphson.Iteracion, resultadosNewtonRaphson.Xr, resultadosNewtonRaphson.Fxr, resultadosNewtonRaphson.Ea}

                };
                dgTabla.ItemsSource = modeloDatos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            txtFuncion.Clear();
            txtXi.Clear();
            txtXf.Clear();
            txtError.Clear();
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
    }
}
