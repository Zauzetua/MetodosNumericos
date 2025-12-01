using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using MetodosNumericos.Core;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para InterpolacionLagrange.xaml
    /// </summary>
    public partial class InterpolacionLagrange : UserControl
    {
        public InterpolacionLagrange()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo para calcular la interpolacion de Lagrange.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] xs = ParseDoubles(txtX.Text);
                double[] ys = ParseDoubles(txtY.Text);

                if (xs.Length != ys.Length)
                {
                    MessageBox.Show("Los arreglos x e y deben tener la misma longitud.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (xs.Length == 0)
                {
                    MessageBox.Show("Debe ingresar al menos un punto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!double.TryParse(txtXi.Text, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out double xi))
                {
                    if (!double.TryParse(txtXi.Text, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out xi))
                    {
                        MessageBox.Show("Xi no es un numero válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                double yi = Interpolacion.Lagrange(xs, ys, xi);

                var table = new List<PointItem>();
                for (int i = 0; i < xs.Length; i++)
                {
                    table.Add(new PointItem { X = xs[i].ToString("F6", CultureInfo.InvariantCulture), Y = ys[i].ToString("F6", CultureInfo.InvariantCulture) });
                }
                dgPoints.ItemsSource = table;

                int degree = xs.Length - 1;
                lblDegree.Text = degree.ToString();

                lblResult.Text = $"({xi.ToString("F6", CultureInfo.InvariantCulture)}, {yi.ToString("F6", CultureInfo.InvariantCulture)})";
            }
            catch (FormatException fex)
            {
                MessageBox.Show(fex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metodo para convertir una cadena de texto en un arreglo de doubles.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private double[] ParseDoubles(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new double[0];

            var parts = text.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<double>();
            foreach (var p in parts)
            {
                if (double.TryParse(p, NumberStyles.Any, CultureInfo.InvariantCulture, out double v) ||
                    double.TryParse(p, NumberStyles.Any, CultureInfo.CurrentCulture, out v))
                {
                    list.Add(v);
                }
                else
                {
                    throw new FormatException($"Valor invalido: '{p}'");
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Clase de apoyo para mostrar los puntos en el DataGrid.
        /// </summary>
        private class PointItem
        {
            public string X { get; set; }
            public string Y { get; set; }
        }

        /// <summary>
        /// Metodo para limpiar los campos de texto y resultados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtX.Clear();
            txtY.Clear();
            txtXi.Clear();
            lblResult.Text = string.Empty;
            lblDegree.Text = string.Empty;
            dgPoints.ItemsSource = null;
        }

        /// <summary>
        /// Metodo para cargar un ejemplo predefinido.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEjemplo_Click(object sender, RoutedEventArgs e)
        {
            //El de la tarea
            txtX.Text = "2, 3, 5, 6";
            txtY.Text = "4, 5.25, 19.75, 36";
            txtXi.Text = "3.5"; 
        }
    }
}
