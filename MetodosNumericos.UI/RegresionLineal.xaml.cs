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
using MetodosNumericos.Core;
using System.Collections.ObjectModel;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Logica de interaccion para RegresionLineal.xaml
    /// </summary>
    public partial class RegresionLineal : UserControl
    {
        private ObservableCollection<PuntoXY> puntos;
        private ObservableCollection<ResultadosPorIteracion> resultados;

        public RegresionLineal()
        {
            InitializeComponent();
            puntos = [];
            resultados = [];

            dgPuntos.ItemsSource = puntos;
            dgResultados.ItemsSource = resultados;

            LimpiarResultados();
        }

        private void btnAgregarPunto_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtX.Text, out double x) && double.TryParse(txtY.Text, out double y))
            {
                puntos.Add(new PuntoXY(x, y));
                txtX.Clear();
                txtY.Clear();
                txtX.Focus();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese valores numericos validos para X e Y.",
                    "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (puntos.Count < 2)
            {
                MessageBox.Show("Se necesitan al menos dos puntos para calcular la regresion.",
                    "Datos insuficientes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Calcular la regresion
                var resultadosCalculados = Regresion.CalcularSumas(puntos.ToList());
                var regresion = AnalizadorDeRegresion.ObtenerMejorRegresion(puntos.ToList());


                //formatatear numeros a 6 decimales
                foreach (var resultado in resultadosCalculados)
                {
                    resultado.Xi = Math.Round(resultado.Xi, 6);
                    resultado.Yi = Math.Round(resultado.Yi, 6);
                    resultado.Xi2 = Math.Round(resultado.Xi2, 6);
                    resultado.Yi2 = Math.Round(resultado.Yi2, 6);
                    resultado.XiYi = Math.Round(resultado.XiYi, 6);
                }
                // Actualizar la tabla de resultados
                resultados.Clear();
                foreach (var resultado in resultadosCalculados)
                {
                    resultados.Add(resultado);
                }
                //Si es lineal
                if (regresion.ModeloElegido == TipoRegresion.Lineal)
                {
                    // Mostrar la ecuacion y R2 como si nada
                    txtEcuacion.Text = $"Ecuacion: y = {regresion.A0:F6} + {regresion.A1:F6}x";
                    txtRCuadrado.Text = $"Coeficiente de determinacion (R²): {regresion.RCuadrado:F6}";
                }
                else
                {
                    //Es exponencial
                    txtEcuacion.Text = $"Ecuacion: y = {regresion.A0:F6}e^({regresion.A1:F6}x)";
                    txtRCuadrado.Text = $"Coeficiente de determinacion (R²): {regresion.RCuadrado:F6}";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular la regresion: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarResultados();
        }

        private void LimpiarResultados()
        {
            puntos.Clear();
            resultados.Clear();
            txtX.Clear();
            txtY.Clear();
            txtEcuacion.Text = string.Empty;
            txtRCuadrado.Text = string.Empty;
            txtX.Focus();
        }

        /// <summary>
        /// Para hacer una prueba con datos de ejemplo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEjemplo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarResultados();
            puntos.Add(new PuntoXY(1, 2));
            puntos.Add(new PuntoXY(2, 3));
            puntos.Add(new PuntoXY(3, 5));
            puntos.Add(new PuntoXY(4, 4));
            puntos.Add(new PuntoXY(5, 6));

        }
    }
}
