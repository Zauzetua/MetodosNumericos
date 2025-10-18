using System.Windows;
using System.Windows.Controls;


namespace MetodosNumericos.UI
{
    /// <summary>
    /// Lógica de interacción para ConfigurarMatriz.xaml
    /// </summary>
    public partial class ConfigurarMatriz : Window
    {
        public double[,] MatrizA { get; private set; }
        public double[] VectorB { get; private set; }
        private int n;
        public ConfigurarMatriz()
        {

            InitializeComponent();
        }

        private void BtnGenerar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtN.Text, out n) || n < 2 || n > 20)
            {
                MessageBox.Show("Ingresa un tamaño valido entre 2 y 20.");
                return;
            }

            gridMatriz.Children.Clear();
            gridMatriz.RowDefinitions.Clear();
            gridMatriz.ColumnDefinitions.Clear();

            for (int i = 0; i < n; i++)
                gridMatriz.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int j = 0; j < n + 1; j++)
                gridMatriz.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var tb = new TextBox { Width = 50, Margin = new Thickness(2), Tag = (i, j) };
                    Grid.SetRow(tb, i);
                    Grid.SetColumn(tb, j);
                    gridMatriz.Children.Add(tb);
                }

                // Columna b
                var tbB = new TextBox { Width = 50, Margin = new Thickness(5), Background = System.Windows.Media.Brushes.LightYellow };
                Grid.SetRow(tbB, i);
                Grid.SetColumn(tbB, n);
                gridMatriz.Children.Add(tbB);
            }
        }

        private void btnMatrizPrueba(object sender, RoutedEventArgs e)
        {
            txtN.Text = "10";
            BtnGenerar_Click(null, null);
            double[,] prueba = new double[,]
            {
                { 10, -1, 2, 0, 0, 0, 0, 0, 0, 0 },
                { -1, 11, -1, 3, 0, 0, 0, 0, 0, 0 },
                { 2, -1, 10, -1, 0, 0, 0, 0, 0, 0 },
                { 0, 3, -1, 8, -1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, -1, 5, -1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, -1, 4, -1, 0, 0, 0 },
                { 0, 0, 0, 0, 0, -1, 6, -1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, -1, 7, -1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, -1, 8, -1 },
                { 0, 0, 0, 0, 0, 0, 0, 0, -1, 9 }
            };
            double[] bPrueba = new double[] { 6, 25, -11, 15, 10, 10, 10, 10, 10, 10 };
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    foreach (var child in gridMatriz.Children)
                    {
                        if (child is TextBox tb && tb.Tag is ValueTuple<int, int> pos)
                        {
                            int row = pos.Item1, col = pos.Item2;
                            if (row == i && col == j)
                                tb.Text = prueba[i, j].ToString();
                        }
                        else if (child is TextBox tbB && Grid.GetColumn(tbB) == 10)
                        {
                            int row = Grid.GetRow(tbB);
                            if (row == i)
                                tbB.Text = bPrueba[i].ToString();
                        }
                    }
                }
            }


        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (n == 0)
            {
                MessageBox.Show("Primero genera la matriz.");
                return;
            }

            MatrizA = new double[n, n];
            VectorB = new double[n];

            foreach (var child in gridMatriz.Children)
            {
                if (child is TextBox tb && tb.Tag is ValueTuple<int, int> pos)
                {
                    int i = pos.Item1, j = pos.Item2;
                    if (double.TryParse(tb.Text, out double val))
                        MatrizA[i, j] = val;
                    else
                    {
                        MessageBox.Show($"Valor invalido en posicion A[{i + 1},{j + 1}]");
                        return;
                    }
                }
                else if (child is TextBox tbB && Grid.GetColumn(tbB) == n)
                {
                    int i = Grid.GetRow(tbB);
                    if (double.TryParse(tbB.Text, out double val))
                        VectorB[i] = val;
                    else
                    {
                        MessageBox.Show($"Valor invalido en b[{i + 1}]");
                        return;
                    }
                }
            }

            DialogResult = true;
            Close();
        }
    }
}

