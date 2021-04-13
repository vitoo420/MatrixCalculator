using System;
using System.Collections.Generic;
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

namespace MatrixCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public List<int> Dimensions { get; set; } = new List<int>()
        {
            1, 2, 3, 4, 5
        };
        private Matrix a = new Matrix(Roles.First);
        private Matrix b = new Matrix(Roles.Second);
        private Matrix c;

        public MainWindow()
        {

            InitializeComponent();
            //this.mainGrid.ShowGridLines = true;
            //this.m1Grid.ShowGridLines = true;
            //this.m2Grid.ShowGridLines = true;
        }

        //private void m1DimensionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Pri inicializaci jeste neni nastavena default hodnota, proto je nutne otestovat a pripadne zustane default hodnota tridy Matrix (3),
        //    //pri zmene je vhodne zmenit i IsSelected item ComboBoxu

        //    if (m1NumOfRowInput != null)
        //        a.NumOfRows = Convert.ToInt32(((ComboBoxItem)m1NumOfRowInput.SelectedItem).Content);
        //    if (m1NumOfColInput != null)
        //        a.NumOfColumns = Convert.ToInt32(((ComboBoxItem)m1NumOfColInput.SelectedItem).Content);

        //    //Pokud existuje pole textboxu tak je odstraneno
        //    if (a.TextBoxMatrix != null)
        //        Matrix.DeleteMatrix(a, m1Grid);

        //    a.CreateTextBoxMatrix(this.m1Grid);
        //}

        //private void m2DimensionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //Pri inicializaci jeste neni nastavena default hodnota, proto je nutne otestovat a pripadne zustane default hodnota tridy Matrix (3),
        //    //pri zmene je vhodne zmenit i IsSelected item ComboBoxu

        //    if (m2NumOfRowInput != null)
        //        b.NumOfRows = Convert.ToInt32(((ComboBoxItem)m2NumOfRowInput.SelectedItem).Content);
        //    if (m2NumOfColInput != null)
        //        b.NumOfColumns = Convert.ToInt32(((ComboBoxItem)m2NumOfColInput.SelectedItem).Content);

        //    //Pokud existuje pole textboxu tak je odstraneno
        //    if (b.TextBoxMatrix != null)
        //        Matrix.DeleteMatrix(b, m2Grid);

        //    b.CreateTextBoxMatrix(this.m2Grid);

        //}

        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            // Launch the GitHub site...
        }

        private void DeployCupCakes(object sender, RoutedEventArgs e)
        {
            // deploy some CupCakes...
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {

                }
            }
        }

        private void OperationChanged(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "add":

                    break;
                case "subtract":
                    break;
                case "multiply":
                    break;
                case "determinant":
                    break;
                default:
                    break;
            }
        }
        private void m1DimensionChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (m1NumOfColInput != null && m1NumOfColInput.Value != null)
                a.NumOfColumns = (int)m1NumOfColInput.Value;
            if (m1NumOfRowInput != null && m1NumOfRowInput.Value != null)
                a.NumOfRows = (int)m1NumOfRowInput.Value;
            if (a.TextBoxMatrix != null)
                Matrix.DeleteMatrix(a, m1Grid);

            a.CreateTextBoxMatrix(this.m1Grid);
        }

        private void m2DimensionChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            
            if (m2NumOfColInput != null && m2NumOfColInput.Value != null)
                b.NumOfColumns = (int)m2NumOfColInput.Value;
            if (m2NumOfRowInput != null && m2NumOfRowInput.Value != null)
                b.NumOfRows = (int)m2NumOfRowInput.Value;
            if (b.TextBoxMatrix != null)
                Matrix.DeleteMatrix(b, m2Grid);

            b.CreateTextBoxMatrix(this.m2Grid);
        }
    }
}
