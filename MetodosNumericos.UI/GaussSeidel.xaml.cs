using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para GaussSeidel.xaml
    /// </summary>
    public partial class GaussSeidel : UserControl
    {
        private double[,] A;
        private double[] b;
        public GaussSeidel()
        {
            InitializeComponent();
        }

        public DataTable CrearTablaResultados(double[][] iteraciones, double[][] errores)
        {
            int n = iteraciones[0].Length;
            DataTable tabla = new();

            // columnas
            tabla.Columns.Add("Iteracion", typeof(int));
            for (int i = 0; i < n; i++)
                tabla.Columns.Add($"x{i + 1}", typeof(double));

            for (int i = 0; i < n; i++)
                tabla.Columns.Add($"err{i + 1}", typeof(double));

            // Llenar filas
            for (int k = 0; k < iteraciones.Length; k++)
            {
                var fila = tabla.NewRow();
                fila["Iteracion"] = k + 1;

                // Agregar valores de x
                for (int i = 0; i < n; i++)
                    fila[$"x{i + 1}"] = Math.Round(iteraciones[k][i], 6);

                // Agregar valores de error
                for (int i = 0; i < n; i++)
                    fila[$"err{i + 1}"] = Math.Round(errores[k][i], 6);

                tabla.Rows.Add(fila);
            }

            return tabla;
        }


        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            if (A == null || b == null)
            {
                MessageBox.Show("Por favor, configure la matriz A y el vector b primero.");
                return;
            }

            try
            {
                if(!Core.Gauss.ReordenarParaGaussSeidel(ref A, ref b))
                {
                    MessageBox.Show("No se pudo reordenar la matriz para el metodo de Gauss-Seidel. Podria no converger");
                }
                var (iteraciones, errores) = Core.Gauss.GaussSeidel(A, b, 1e-3, 100);
                var tablaResultados = CrearTablaResultados(iteraciones, errores);
                dgResultados.ItemsSource = tablaResultados.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al calcular: {ex.Message}");
            }

            return;
        }
        private void btnConfigurar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConfigurarMatriz();
            if (dialog.ShowDialog() == true)
            {
                A = dialog.MatrizA;
                b = dialog.VectorB;
                MessageBox.Show("Matriz configurada correctamente.");
            }

        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            dgResultados.ItemsSource = null;
            dgResultados.Items.Clear();
        }
    }


}

