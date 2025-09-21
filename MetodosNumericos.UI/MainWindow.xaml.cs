using System.Windows;
using System.Windows.Controls;

namespace MetodosNumericos.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void MenuItem_Tabulacion_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            var view = new TabulacionView();
            Grid.SetRow(view, 0);
            Grid.SetColumn(view, 0);
            MainContent.Children.Add(view);
        }


        private void MenuItem_Errores_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new CalcularErrores());
        }

        private void MenuItem_Raices_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            var view = new Raices();
            Grid.SetRow(view, 0);
            Grid.SetColumn(view, 0);
            MainContent.Children.Add(view);
        }

        private void MenuItem_Comparativa_Raices_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            var view = new ComparativaRaices();
            Grid.SetRow(view, 0);
            Grid.SetColumn(view, 0);
            MainContent.Children.Add(view);
        }

        private void btnTabular_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            var view = new TabulacionView();
            Grid.SetRow(view, 0);
            Grid.SetColumn(view, 0);
            MainContent.Children.Add(view);
        }

        private void btnErrores_Click(object sender, RoutedEventArgs e)
        {

            MainContent.Children.Clear();
            var view = new CalcularErrores();
            Grid.SetRow(view, 0);
            Grid.SetColumn(view, 0);
            MainContent.Children.Add(view);

        }

        private void btnRaices_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            var view = new Raices();
            Grid.SetRow(view, 0);
            Grid.SetColumn(view, 0);
            MainContent.Children.Add(view);

        }
    }
}