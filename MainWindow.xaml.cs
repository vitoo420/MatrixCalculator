using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MahApps.Metro.Controls;
using ControlzEx.Theming;
using System.Text.RegularExpressions;

namespace MatrixCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Matrix a = new Matrix(Roles.First);
        private Matrix b = new Matrix(Roles.Second);
        private Matrix c;
        private MatrixOperations Operation;
        
        
        public MainWindow()
        {
            InitializeComponent();

            //this.mainGrid.ShowGridLines = true;
            //this.m1Grid.ShowGridLines = true;
            //this.m2Grid.ShowGridLines = true;

        }

        private void OperationChanged(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "add":
                    Operation = MatrixOperations.add;
                    m1NumOfColInput.IsEnabled = true;
                    m2NumOfColInput.IsEnabled = false;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, true);
                    break;
                case "subtract":
                    Operation = MatrixOperations.subtract;
                    m1NumOfColInput.IsEnabled = true;
                    m2NumOfColInput.IsEnabled = false;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, true);
                    break;
                case "multiply":
                    Operation = MatrixOperations.multiply;
                    m1NumOfColInput.IsEnabled = true;
                    m2NumOfColInput.IsEnabled = true;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, true);
                    break;
                case "determinant":
                    Operation = MatrixOperations.determinant;
                    m1NumOfColInput.IsEnabled = false;
                    m2NumOfColInput.IsEnabled = false;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, false);
                    break;
                case "inverse":
                    Operation = MatrixOperations.inverse;
                    m1NumOfColInput.IsEnabled = false;
                    m2NumOfColInput.IsEnabled = false;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, false);
                    break;
                case "transpose":
                    Operation = MatrixOperations.transpose;
                    m1NumOfColInput.IsEnabled = true;
                    m2NumOfColInput.IsEnabled = false;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, false);
                    break;
                default:
                    break;
            }
            CheckDimensions();
        }
        private void m1DimensionChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (m1NumOfColInput != null && m1NumOfColInput.Value != null)
                a.NumOfColumns = (int)m1NumOfColInput.Value;
            if (m1NumOfRowInput != null && m1NumOfRowInput.Value != null)
                a.NumOfRows = (int)m1NumOfRowInput.Value;
            if (a.TextBoxMatrix != null)
                Matrix.DeleteMatrix(a, m1Grid);

            a.CreateTextBoxMatrix(m1Grid, this);
            CheckDimensions();
        }

        private void m2DimensionChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (m2NumOfColInput != null && m2NumOfColInput.Value != null)
                b.NumOfColumns = (int)m2NumOfColInput.Value;
            if (m2NumOfRowInput != null && m2NumOfRowInput.Value != null)
                b.NumOfRows = (int)m2NumOfRowInput.Value;
            if (b.TextBoxMatrix != null)
                Matrix.DeleteMatrix(b, m2Grid);

            b.CreateTextBoxMatrix(m2Grid, this);
        }

        //Separatni metoda pro kontrolu rozmeru, aby se zamezilo vadnym rozmerum pri prepinani operaci
        private void CheckDimensions()
        {
            switch (Operation)
            {
                case MatrixOperations.add:
                case MatrixOperations.subtract:
                    if (m2NumOfColInput != null)
                        m2NumOfColInput.Value = m1NumOfColInput.Value;
                    if (m2NumOfRowInput != null)
                        m2NumOfRowInput.Value = m1NumOfRowInput.Value;
                    break;

                case MatrixOperations.multiply:
                    m2NumOfRowInput.Value = m1NumOfColInput.Value;
                    break;
                case MatrixOperations.determinant:
                    m1NumOfColInput.Value = m1NumOfRowInput.Value;
                    break;
                case MatrixOperations.inverse:
                    m1NumOfColInput.Value = m1NumOfRowInput.Value;
                    break;
                case MatrixOperations.transpose:
                    break;
                default:
                    break;
            }
        }

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/vitoo420/MatrixCalculator",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                    ThemeManager.Current.ChangeTheme(this, "Dark.Steel");
                else
                    ThemeManager.Current.ChangeTheme(this, "Light.Blue");
            }
        }

        private void solve_Click(object sender, RoutedEventArgs e)
        {
            Matrix.Solve(Operation, a, b);
        }

        public void ValidateTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
