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
using System.Globalization;
using System.IO;

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
        Matrix[] memory = new Matrix[5];


        public MainWindow()
        {
            InitializeComponent();
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
                //upravit
                case "division":
                    Operation = MatrixOperations.division;
                    m1NumOfColInput.IsEnabled = false;
                    m2NumOfColInput.IsEnabled = true;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, true);
                    break;
                // ----------
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
                case "multiplyByNumber":
                    Operation = MatrixOperations.multiplyByNumber;
                    m1NumOfColInput.IsEnabled = true;
                    m2NumOfColInput.IsEnabled = false;
                    m2NumOfRowInput.IsEnabled = false;
                    b.TextBoxMatrixIsEnabled(m2Grid, true);
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
                case MatrixOperations.division:
                    m1NumOfColInput.Value = m1NumOfRowInput.Value;
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
                case MatrixOperations.multiplyByNumber:
                    m2NumOfColInput.Value = 1;
                    m2NumOfRowInput.Value = 1;
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
            c = Matrix.Solve(Operation, a, b);
            c.CreateTextBoxMatrix(resultGrid, this);
        }

        public void ValidateTextBox(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;
            bool approvedPlusOrMinus = false;

            NumberFormatInfo nfi = new CultureInfo(CultureInfo.CurrentCulture.ToString(), false).NumberFormat;

            if ((e.Text == "+" || e.Text == "-") && ((TextBox)sender).Text.Length == 0)
                approvedDecimalPoint = true;

            if (e.Text == nfi.NumberDecimalSeparator)
            {
                if (!((TextBox)sender).Text.Contains(nfi.NumberDecimalSeparator))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint || approvedPlusOrMinus))
                e.Handled = true;
        }

        private void MemoryClick(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
            if (c != null)
            {
                switch (name)
                {
                    case "m1":
                        memory[0] = c;
                        break;
                    case "m2":
                        memory[1] = c;
                        break;
                    case "m3":
                        memory[2] = c;
                        break;
                    case "m4":
                        memory[3] = c;
                        break;
                    case "m5":
                        memory[4] = c;
                        break;
                    default:
                        break;
                }
            }
        }

        private void InsertFromMemory(object sender, RoutedEventArgs e)
        {
            string memSel = ((ComboBoxItem)from.SelectedItem).Content.ToString();
            string matSel = ((ComboBoxItem)to.SelectedItem).Content.ToString();
            Matrix memTemp = new Matrix();

            switch (memSel)
            {
                case "M1":
                    memTemp = memory[0];
                    break;
                case "M2":
                    memTemp = memory[1];
                    break;
                case "M3":
                    memTemp = memory[2];
                    break;
                case "M4":
                    memTemp = memory[3];
                    break;
                case "M5":
                    memTemp = memory[4];
                    break;
            }


            if (matSel == "A")
            {
                if (a.TextBoxMatrix != null)
                    Matrix.DeleteMatrix(a, m1Grid);
                a = memTemp;
                a.Role = Roles.First;
                a.CreateTextBoxMatrix(m1Grid, this);
                for (int i = 0; i < a.NumOfRows; i++)
                {
                    for (int j = 0; j < a.NumOfColumns; j++)
                    {
                        a.TextBoxMatrix[i, j].Text = a.MatrixData[i, j].ToString();
                    }
                }
            }
            else
            {
                if (b.TextBoxMatrix != null)
                    Matrix.DeleteMatrix(b, m2Grid);
                b = memTemp;
                b.Role = Roles.Second;
                b.CreateTextBoxMatrix(m2Grid, this);
                for (int i = 0; i < b.NumOfRows; i++)
                {
                    for (int j = 0; j < b.NumOfColumns; j++)
                    {
                        b.TextBoxMatrix[i, j].Text = b.MatrixData[i, j].ToString();
                    }
                }
            }
        }

        private void ExpCSVButton_Click(object sender, RoutedEventArgs e)
        {
            if (c != null)
            {
                using (var writer = new StreamWriter("file.csv"))
                {
                    string[] values = new string[c.NumOfColumns];

                    for (int i = 0; i < c.NumOfRows; i++)
                    {
                        for (int j = 0; j < c.NumOfColumns; j++)
                        {
                            values[j] = c.MatrixData[i, j].ToString();
                        }
                        string row = String.Join(";", values);
                        writer.WriteLine(row);
                    }
                }
            }
        }

        private void importCsvButton_Click(object sender, RoutedEventArgs e)
        {
            string[] rowData = new string[5];
            string s;
            double[,] data;
            

            using (var reader = new StreamReader("file.csv"))
            {
                int i = 0;

                while ((s = reader.ReadLine()) != null)
                {
                    rowData[i] = s;
                        //= s.Split(';');

                    //string jmeno = rozdeleno[0];
                    i++;
                }
            }

            int rowNum = rowData.Count(x => x != null);
            int colNum = rowData[0].Count(x => x == ';') + 1;

            data = new double[rowNum, colNum];

            for (int i = 0; i < rowNum; i++)
            {
                string[] separ = rowData[i].Split(';');

                for (int j = 0; j < colNum; j++)
                {
                    data[i, j] = Convert.ToDouble(separ[j]);
                }
            }

            if (((ComboBoxItem)(toCsvCombo.SelectedItem)).Content.ToString() == "A")
            {
                a.MatrixData = data;
                m1NumOfColInput.Value = colNum;
                m1NumOfRowInput.Value = rowNum;
                if (a.TextBoxMatrix != null)
                    Matrix.DeleteMatrix(a, m1Grid);
                a.CreateTextBoxMatrix(m1Grid, this);
                for (int i = 0; i < a.NumOfRows; i++)
                {
                    for (int j = 0; j < a.NumOfColumns; j++)
                    {
                        a.TextBoxMatrix[i, j].Text = a.MatrixData[i, j].ToString();
                    }
                }
            }
            else
            {
                b.MatrixData = data;
                m1NumOfColInput.Value = colNum;
                m1NumOfRowInput.Value = rowNum;
                if (b.TextBoxMatrix != null)
                    Matrix.DeleteMatrix(b, m2Grid);
                b.CreateTextBoxMatrix(m2Grid, this);
                for (int i = 0; i < b.NumOfRows; i++)
                {
                    for (int j = 0; j < b.NumOfColumns; j++)
                    {
                        b.TextBoxMatrix[i, j].Text = b.MatrixData[i, j].ToString();
                    }
                }
            }
        }
    }
}
