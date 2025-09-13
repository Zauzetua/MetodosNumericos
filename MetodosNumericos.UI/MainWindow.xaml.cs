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

            // Por defecto mostramos Tabulacion
            MainContent.Content = new TabulacionView();
        }

        private void MenuItem_Tabulacion_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TabulacionView();
        }

        private void MenuItem_OtraVista_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Errores(); // OtraVista debe ser un UserControl
        }
    }
}