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
        private Matrix a = new Matrix(50, 50);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void m1NumOfRowInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Pri inicializaci jeste neni nastavena default hodnota, proto je nutne otestovat a pripadne zustane default hodnota tridy Matrix (3),
            //pri zmene je vhodne zmenit i IsSelected item ComboBoxu
            if (((ComboBoxItem)m1NumOfRowInput.SelectedItem).Content != null) a.NumOfRows = Convert.ToInt32(((ComboBoxItem)m1NumOfRowInput.SelectedItem).Content);

            //Pokud existuje pole textboxu tak je odstraneno
            if (a.TextBoxMatrix != null)
            {
                foreach (var item in a.TextBoxMatrix)
                {
                    this.canvas.Children.Remove(item);
                }
            }

            a.CreateTextBoxMatrix(this.canvas);
        }

        private void m1NumOfColInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Pri inicializaci jeste neni nastavena default hodnota, proto je nutne otestovat a pripadne zustane default hodnota tridy Matrix (3),
            //pri zmene je vhodne zmenit i IsSelected item ComboBoxu
            if (((ComboBoxItem)m1NumOfColInput.SelectedItem).Content != null) a.NumOfColumns = Convert.ToInt32(((ComboBoxItem)m1NumOfColInput.SelectedItem).Content);

            //Pokud existuje pole textboxu tak je odstraneno
            if (a.TextBoxMatrix != null)
            {
                foreach (var item in a.TextBoxMatrix)
                {
                    this.canvas.Children.Remove(item);
                }
            }

            a.CreateTextBoxMatrix(this.canvas);
        }

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
    }
}
