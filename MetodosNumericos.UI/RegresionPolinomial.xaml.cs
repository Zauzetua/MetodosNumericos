using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using MetodosNumericos.Core;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para RegresionPolinomial.xaml
    /// </summary>
    public partial class RegresionPolinomial : UserControl
    {
        private ObservableCollection<PuntoXY> puntos;
        private ObservableCollection<ResultadosPolinomiales> resultados;
        private int gradoActual;

        public RegresionPolinomial()
        {
            InitializeComponent();
            puntos = new ObservableCollection<PuntoXY>();
            resultados = new ObservableCollection<ResultadosPolinomiales>();
            
            dgPuntos.ItemsSource = puntos;
            dgResultados.ItemsSource = resultados;
            
            cmbGrado.SelectedIndex = 0;
            LimpiarResultados();
        }

        private void ActualizarColumnasResultados()
        {
            // Limpiar columnas existentes excepto X e Y
            while (dgResultados.Columns.Count > 2)
            {
                dgResultados.Columns.RemoveAt(2);
            }

            // Agregar columnas para potencias de X
            for (int i = 2; i <= gradoActual * 2; i++)
            {
                dgResultados.Columns.Add(new DataGridTextColumn
                {
                    Header = $"X^{i}",
                    Binding = new Binding($"PotenciasX[{i}]") { StringFormat = "F4" }
                });
            }

            // Agregar columnas para X*Y
            for (int i = 1; i <= gradoActual; i++)
            {
                dgResultados.Columns.Add(new DataGridTextColumn
                {
                    Header = $"X^{i}·Y",
                    Binding = new Binding($"XY[{i}]") { StringFormat = "F4" }
                });
            }
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
                MessageBox.Show("Por favor, ingrese valores numéricos válidos para X e Y.",
                    "Error de entrada", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (puntos.Count < 2)
            {
                MessageBox.Show("Se necesitan al menos dos puntos para calcular la regresión.",
                    "Datos insuficientes", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                gradoActual = int.Parse((cmbGrado.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "1");

                // Verificar si hay suficientes puntos para el grado seleccionado
                if (puntos.Count < gradoActual + 1)
                {
                    MessageBox.Show($"Se necesitan al menos {gradoActual + 1} puntos para una regresión de grado {gradoActual}.",
                        "Datos insuficientes", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Calcular la regresión
                var (coeficientes, resultadosCalculados) = Core.RegresionPolinomial.CalcularRegresionPolinomial(puntos.ToList(), gradoActual);

                // Actualizar las columnas del DataGrid
                ActualizarColumnasResultados();

                // Actualizar la tabla de resultados
                resultados.Clear();
                foreach (var resultado in resultadosCalculados)
                {
                    resultados.Add(resultado);
                }

                // Mostrar la ecuación
                txtEcuacion.Text = $"Ecuación: {Core.RegresionPolinomial.ObtenerEcuacion(coeficientes)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular la regresión: {ex.Message}",
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
            cmbGrado.SelectedIndex = 0;
            gradoActual = 1;
            ActualizarColumnasResultados();
            txtX.Focus();
        }

        /// <summary>
        /// Boton para agregar puntos de ejemplo de una cuadratica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEjemplo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarResultados();
            puntos.Add(new PuntoXY(1, 2));
            puntos.Add(new PuntoXY(2, 5));
            puntos.Add(new PuntoXY(3, 10));
            puntos.Add(new PuntoXY(4, 17));
            puntos.Add(new PuntoXY(5, 26));
            cmbGrado.SelectedIndex = 1;


        }
    }
}
