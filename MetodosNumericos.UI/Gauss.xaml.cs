using System.Windows;
using System.Windows.Controls;
using CoreGauss = MetodosNumericos.Core.Gauss;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para Gauss.xaml
    /// </summary>
    public partial class Gauss : UserControl
    {
        public Gauss()
        {
            InitializeComponent();
            cbDimension.SelectedIndex = 0; // Seleccion inicial (por defecto 2x2)
            btnGenerar_Click(this, new RoutedEventArgs());
        }

        // Evento: genera dinamicamente los cuadros de texto segun la dimension seleccionada
        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            spEntradasMatriz.Children.Clear(); // Reiniciar campos
            txtMensajes.Clear(); // Limpiar mensajes

            int n = 2; // Dimension por defecto
            try { n = int.Parse((string)cbDimension.SelectedItem); } catch { n = 2; }

            // Crear un Grid para las entradas de la matriz A
            Grid gridA = new() { Margin = new Thickness(0, 0, 0, 6) };

            // Agregar definicion de columnas
            for (int c = 0; c < n; c++)
                gridA.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            // Agregar una fila extra para los encabezados (x1, x2, ...)
            gridA.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // Agregar filas para los cuadros de texto
            for (int r = 0; r < n; r++)
                gridA.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // --- Crear los encabezados de columna (x1, x2, x3, ...) ---
            for (int j = 0; j < n; j++)
            {
                Label lbl = new Label
                {
                    Content = $"x{j + 1}",
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(2)
                };
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, j);
                gridA.Children.Add(lbl);
            }

            // --- Crear los cuadros de texto para los coeficientes A[i,j] ---
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    TextBox txt = new TextBox
                    {
                        Width = 60,
                        Height = 26,
                        Margin = new Thickness(2),
                        FontFamily = new System.Windows.Media.FontFamily("Arial"),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Name = $"a_{i}_{j}"
                    };
                    // +1 porque la fila 0 se usa para los encabezados
                    Grid.SetRow(txt, i + 1);
                    Grid.SetColumn(txt, j);
                    gridA.Children.Add(txt);
                }
            }

            // --- Crear los cuadros de texto para el vector b ---
            StackPanel spVectorB = new()
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(8, 0, 0, 0)
            };

            // Encabezado para el vector b
            Label lblB = new Label
            {
                Content = "b",
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(2)
            };
            spVectorB.Children.Add(lblB);

            // Campos del vector b
            for (int i = 0; i < n; i++)
            {
                TextBox txtB = new TextBox()
                {
                    Width = 80,
                    Height = 26,
                    Margin = new Thickness(2),
                    FontFamily = new System.Windows.Media.FontFamily("Arial"),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Name = $"b_{i}"
                };
                spVectorB.Children.Add(txtB);
            }

            // --- Contenedor horizontal que une matriz A y vector b ---
            StackPanel fila = new StackPanel() { Orientation = Orientation.Horizontal };
            fila.Children.Add(gridA);
            fila.Children.Add(spVectorB);
            spEntradasMatriz.Children.Add(fila);
        }


        // Evento: limpia los campos y resultados, tambien elimina los campos
        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            spEntradasMatriz.Children.Clear();
            dgResultados.ItemsSource = null;
            txtMensajes.Clear();
        }

        // Evento: calcula la solucion del sistema usando eliminacion gaussiana con pivoteo.
        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            txtMensajes.Clear();
            try
            {
                int n = int.Parse((string)cbDimension.SelectedItem);
                if (n < 2 || n > 4)
                {
                    MessageBox.Show("Dimension invalida, debe estar entre 2 y 4");
                    txtMensajes.Text = "La dimension debe estar entre 2 y 4";
                    return;
                }

                double[,] A = new double[n, n];
                double[] b = new double[n];

                // Buscar los controles dentro del StackPanel principal
                var fila = spEntradasMatriz.Children.OfType<StackPanel>().FirstOrDefault();
                if (fila == null)
                {
                    MessageBox.Show("No se encontraron los campos de entrada");
                    txtMensajes.Text = "No se encontraron los campos de entrada";
                    return;
                }

                var gridA = fila.Children.OfType<Grid>().FirstOrDefault();
                var spB = fila.Children.OfType<StackPanel>().FirstOrDefault(s => s.Children.Count > 0);

                if (gridA == null || spB == null)
                {
                    MessageBox.Show("No se encontraron los campos de entrada");
                    txtMensajes.Text = "No se encontraron los campos de entrada";
                    return;
                }

                // Leer la matriz A
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        string nombre = $"a_{i}_{j}";
                        var txt = gridA.Children.OfType<TextBox>().FirstOrDefault(t => t.Name == nombre);
                        if (txt == null)
                        {
                            MessageBox.Show("Falta el campo de matriz: " + nombre);
                            txtMensajes.Text = "Falta el campo de matriz: " + nombre;
                            return;
                        }
                        if (!double.TryParse(txt.Text, out double valor))
                        {
                            MessageBox.Show("Valor invalido en A[" + i + "," + j + "]");
                            txtMensajes.Text = "Valor invalido en A[" + i + "," + j + "]";
                            return;
                        }
                        A[i, j] = valor;
                    }
                }

                // Leer el vector b
                for (int i = 0; i < n; i++)
                {
                    var txt = spB.Children.OfType<TextBox>().ElementAt(i);
                    if (txt == null)
                    {
                        MessageBox.Show("Falta el campo del vector b: " + i);
                        txtMensajes.Text = "Falta el campo del vector b: " + i;
                        return;
                    }
                    if (!double.TryParse(txt.Text, out double valor))
                    {
                        MessageBox.Show("Valor invalido en b[" + i + "]");
                        txtMensajes.Text = "Valor invalido en b[" + i + "]";
                        return;
                    }
                    b[i] = valor;
                }

                // Seleccionar metodo segun la seleccion del usuario
                string metodo = (string)cbxMetodo.SelectedItem ?? "Gauss";
                double[] x;
                if (metodo.Contains("Jordan") || metodo.Contains("jordan", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Gauss-Jordan
                    x = CoreGauss.ResolverGaussJordan(A, b);
                }
                else
                {
                    // Gauss 
                    x = CoreGauss.Resolver(A, b);
                }

                //redondear a 4 decimales
                for (int i = 0; i < x.Length; i++)
                    x[i] = Math.Round(x[i], 4);

                // Mostrar resultados en un DataGrid
                var lista = new List<object>();
                for (int i = 0; i < n; i++)
                    lista.Add(new { Variable = $"x{i + 1}", Valor = x[i] });

                dgResultados.ItemsSource = lista;
                txtMensajes.Text = "Solucion calculada correctamente.";
                MostrarMatrizAumentada(A, b);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                txtMensajes.Text = "Error: " + ex.Message;
            }
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            string general = "No use variables, solo ponga los coeficientes (Y su signo) de estas en orden\nUse '.' para decimales\nNo use espacios innecesarios\nNo utilice letras o simbolos.";

            MessageBox.Show("Lineamientos:\n" + general, "Ayuda");

        }

        private void MostrarMatrizAumentada(double[,] A, double[] b)
        {
            int n = A.GetLength(0);
            string matriz = "Matriz Aumentada [A|b]:\n";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matriz += $"{A[i, j],8:F4} ";
                }
                matriz += $"| {b[i],8:F4}\n";
            }
            MessageBox.Show(matriz, "Matriz Aumentada");
        }
    }

}
