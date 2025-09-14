using System.Text;
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
        }


        private void MenuItem_Errores_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(new CalcularErrores());
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
            MainContent.Children.Add(new CalcularErrores());
        }
    }
}