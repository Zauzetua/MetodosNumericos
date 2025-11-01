using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MetodosNumericos.Core;

namespace MetodosNumericos.UI
{
    public partial class RegresionLinealMultiple : UserControl
    {
        private ObservableCollection<PuntoMultiple> puntos;
        private List<VariableInput> variablesInput;

        public RegresionLinealMultiple()
        {
            InitializeComponent();
            puntos = new ObservableCollection<PuntoMultiple>();
            variablesInput = new List<VariableInput>();
            dgPuntos.ItemsSource = puntos;
            Loaded += RegresionLinealMultiple_Loaded;
        }

        private void RegresionLinealMultiple_Loaded(object sender, RoutedEventArgs e)
        {
            cmbNumVariables.SelectedIndex = 0;
            ConfigurarVariables(2); 
        }

        private void ConfigurarVariables(int numVariables)
        {
            variablesInput.Clear();
            dgPuntos.AutoGenerateColumns = false;

            // Agregar campos para variables X
            for (int i = 0; i < numVariables; i++)
            {
                variablesInput.Add(new VariableInput { Name = $"X{i + 1}:", Value = string.Empty });
            }

            // Agregar campo Y
            variablesInput.Add(new VariableInput { Name = "Y:", Value = string.Empty });

            icVariables.ItemsSource = null;
            icVariables.ItemsSource = variablesInput;

            // Configurar columnas del grid
            dgPuntos.Columns.Clear();
            for (int i = 0; i < numVariables; i++)
            {
                dgPuntos.Columns.Add(new DataGridTextColumn
                {
                    Header = $"X{i + 1}",
                    Binding = new System.Windows.Data.Binding($"X[{i}]")
                });
            }
            dgPuntos.Columns.Add(new DataGridTextColumn
            {
                Header = "Y",
                Binding = new System.Windows.Data.Binding("Y")
            });
        }

        private void cmbNumVariables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbNumVariables.SelectedItem != null)
            {
                int numVariables = int.Parse((cmbNumVariables.SelectedItem as ComboBoxItem ?? new ComboBoxItem()).Content.ToString() ?? "2");
                ConfigurarVariables(numVariables);
                LimpiarResultados();
            }
        }

        private void btnAgregarPunto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var valores = variablesInput.Select(v => v.Value).ToList();
                if (valores.Any(string.IsNullOrWhiteSpace))
                {
                    MessageBox.Show("Por favor, complete todos los campos.",
                        "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int numVariables = valores.Count - 1; // Restamos 1 por Y
                var x = new double[numVariables];
                for (int i = 0; i < numVariables; i++)
                {
                    if (!double.TryParse(valores[i], out x[i]))
                    {
                        MessageBox.Show($"Valor invalido para X{i + 1}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (!double.TryParse(valores.Last(), out double y))
                {
                    MessageBox.Show("Valor invalido para Y",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                puntos.Add(new PuntoMultiple(x, y));

                // Limpiar campos
                foreach (var variable in variablesInput)
                {
                    variable.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar punto: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (puntos.Count < variablesInput.Count)
                {
                    MessageBox.Show($"Se necesitan al menos {variablesInput.Count} puntos para calcular la regresion we.",
                        "Datos insuficientes", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var resultado = Core.RegresionLinealMultiple.CalcularRegresion(puntos.ToList());
                txtEcuacion.Text = resultado.ObtenerEcuacion();

                // Mostrar la matriz de coeficientes
                var matrizResultados = new ObservableCollection<object>
                {
                    new { Coeficiente = "a", Valor = resultado.B0.ToString("F4") }
                };
                for (int i = 0; i < resultado.Coeficientes.Length; i++)
                {
                    matrizResultados.Add(new
                    {
                        Coeficiente = $"a{i + 1}",
                        Valor = resultado.Coeficientes[i].ToString("F4")
                    });
                }
                dgMatriz.ItemsSource = matrizResultados;
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

        private void btnEjemplo_Click(object sender, RoutedEventArgs e)
        {
            LimpiarResultados();
            cmbNumVariables.SelectedIndex = 0; // Seleccionar 2 variables
            puntos.Add(new PuntoMultiple([0, 0], 5));
            puntos.Add(new PuntoMultiple([2, 1], 10));
            puntos.Add(new PuntoMultiple([2.5, 2], 9));
            puntos.Add(new PuntoMultiple([1, 3], 0));
            puntos.Add(new PuntoMultiple([4, 6], 3));
            puntos.Add(new PuntoMultiple([7, 2], 27));


        }

        private void LimpiarResultados()
        {
            puntos.Clear();
            foreach (var variable in variablesInput)
            {
                variable.Value = string.Empty;
            }
            txtEcuacion.Text = string.Empty;
            if (dgMatriz.ItemsSource != null)
                ((ObservableCollection<object>)dgMatriz.ItemsSource).Clear();
        }
    }

    public class VariableInput : System.ComponentModel.INotifyPropertyChanged
    {
        private string name = string.Empty;
        private string value = string.Empty;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}