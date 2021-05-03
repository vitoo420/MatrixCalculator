using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.UI.Xaml;

namespace MatrixCalculator
{
    class Matrix
    {
        private int _numOfRows;
        public int NumOfRows
        {
            get { return _numOfRows; }
            set
            {
                _numOfRows = value;
                if (_numOfRows == 0)
                    _numOfRows = 3;
            }
        }
        private int _numOfColumns;
        public int NumOfColumns
        {
            get { return _numOfColumns; }
            set
            {
                _numOfColumns = value;
                if (_numOfColumns == 0)
                    _numOfColumns = 3;
            }
        }
        public TextBox[,] TextBoxMatrix { get; set; }
        public double[,] MatrixData { get; set; }
        public Roles Role { get; set; }

        public Matrix()
        {

        }

        public Matrix(Roles role)
        {
            Role = role;
            NumOfRows = 3;
            NumOfColumns = 3;
        }

        public Matrix(Roles role, double[,] data)
        {
            this.Role = role;
            this.MatrixData = data;
            NumOfRows = data.GetLength(0);
            NumOfColumns = data.GetLength(1);
        }

        /// <summary>
        /// Vytvoří TextBox v gridu
        /// </summary>
        /// <param name="grid">Sub Grid matice</param>
        /// <param name="row">radek v Sub Gridu</param>
        /// <param name="col">sloupec v Sub Gridu</param>
        /// <returns>TextBox objekt</returns>
        private TextBox CreateATextBox(Grid grid, int row, int col, MatrixCalculator.MainWindow window)
        {
            TextBox txtb = new TextBox();
            grid.Children.Add(txtb);
            Grid.SetRow(txtb, row);
            Grid.SetColumn(txtb, col);
            txtb.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(window.ValidateTextBox);
            return txtb;
        }

        /// <summary>
        /// Vytvoří pole TextBoxů reprezentující matici, TextBoxy jsou umístěny do gridu, který je určený pro matici.
        /// </summary>
        /// <param name="grid">Grid matice</param>
        /// <param name="window">Instance okna pro možnost přidání validace TB</param>
        public void CreateTextBoxMatrix(Grid grid, MatrixCalculator.MainWindow window)
        {
            int iPlus = 0;                                  //Nastaveni pozicovacich promennych 
            int jPlus = 0;

            if (Role == Roles.First)
                jPlus = 5 - NumOfColumns;

            switch (NumOfRows)
            {
                case 1:
                    iPlus = 2;
                    break;
                case 2:
                case 3:
                    iPlus = 1;
                    break;
                case 4:
                case 5:
                    iPlus = 0;
                    break;
                default:
                    break;
            }

            //v matematice i v c# multidim. polich se prvni pise radek, potom sloupec
            TextBoxMatrix = new TextBox[NumOfRows, NumOfColumns];

            for (int i = 0; i < NumOfRows; i++)
            {
                for (int j = 0; j < NumOfColumns; j++)
                {
                    TextBoxMatrix[i, j] = CreateATextBox(grid, i + iPlus, j + jPlus, window);
                    if (this.Role == Roles.Result)
                    { 
                        TextBoxMatrix[i, j].Text = MatrixData[i, j].ToString();
                        TextBoxMatrix[i, j].IsReadOnly = true;
                    }
                }
            }

        }

        /// <summary>
        /// Zakáže či povolí zapisovat do TB
        /// </summary>
        /// <param name="grid">Grid matice</param>
        /// <param name="isEnabled">true - povolit, false - zakázat</param>
        public void TextBoxMatrixIsEnabled(Grid grid, bool isEnabled)
        {
            bool _isEnabled = isEnabled;
            foreach (var item in this.TextBoxMatrix)
            {
                item.IsEnabled = _isEnabled;
            }
        }

        /// <summary>
        /// Odstraní TextBoxové pole
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="grid"></param>
        public static void DeleteMatrix(Matrix matrix, Grid grid)
        {
            foreach (var item in matrix.TextBoxMatrix)
            {
                grid.Children.Remove(item);
            }
        }

        /// <summary>
        /// Překopíruje data z textboxu do pole double
        /// </summary>
        private void TextBoxArrayToDoubleArray()
        {
            this.MatrixData = new double[NumOfRows, NumOfColumns];
            for (int i = 0; i < NumOfRows; i++)
            {
                for (int j = 0; j < NumOfColumns; j++)
                {
                    Double.TryParse(this.TextBoxMatrix[i, j].Text, out this.MatrixData[i, j]);
                    this.TextBoxMatrix[i, j].Text = (this.MatrixData[i, j]).ToString();
                }
            }
        }

       
        /// <summary>
        /// Provádí výpočty
        /// </summary>
        /// <param name="operation">Vybraná operace</param>
        /// <param name="a">matice</param>
        /// <param name="b">matice</param>
        /// <returns>Výsledná matice</returns>
        public static Matrix Solve(MatrixOperations operation, Matrix a, Matrix b)
        {
            a.TextBoxArrayToDoubleArray();
            b.TextBoxArrayToDoubleArray();
            switch (operation)
            {
                case MatrixOperations.add:
                    return a + b;
                case MatrixOperations.subtract:
                    return a - b;
                case MatrixOperations.multiply:
                    return a * b;
                case MatrixOperations.multiplyByNumber:
                    return a * b.MatrixData[0,0];
                case MatrixOperations.division:
                    return a / b;
                case MatrixOperations.determinant:
                    double[,] determinant = new double[1, 1];
                    determinant[0, 0] = Determinant(a.MatrixData);
                    return new Matrix(Roles.Result, determinant);
                case MatrixOperations.inverse:
                    return Invert(a);
                case MatrixOperations.transpose:
                    return Transpose(a);
                default:
                    throw new Exception("Neznama operace");
            }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            double[,] addedMatrix;

            if (a.MatrixData.GetLength(0) == b.MatrixData.GetLength(0) && a.MatrixData.GetLength(1) == b.MatrixData.GetLength(1))
            {
                addedMatrix = new double[a.MatrixData.GetLength(0), a.MatrixData.GetLength(1)];

                for (int width = 0; width < a.MatrixData.GetLength(0); width++)
                {
                    for (int height = 0; height < a.MatrixData.GetLength(1); height++)
                    {
                        addedMatrix[width, height] = a.MatrixData[width, height] + b.MatrixData[width, height];
                    }
                }
                return new Matrix(Roles.Result, addedMatrix);
            }
            else
            {
                throw new Exception("Matice nemaji stejny rozmer!");
            }
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            double[,] substractedMatrix;

            if (a.MatrixData.GetLength(0) == b.MatrixData.GetLength(0) && a.MatrixData.GetLength(1) == b.MatrixData.GetLength(1))
            {
                substractedMatrix = new double[a.MatrixData.GetLength(0), a.MatrixData.GetLength(1)];

                for (int width = 0; width < a.MatrixData.GetLength(0); width++)
                {
                    for (int height = 0; height < a.MatrixData.GetLength(1); height++)
                    {
                        substractedMatrix[width, height] = a.MatrixData[width, height] - b.MatrixData[width, height];
                    }
                }
                return new Matrix(Roles.Result, substractedMatrix);
            }
            else
            {
                throw new Exception("Matice nemaji stejny rozmer!");
            }
        }

        public static Matrix operator /(Matrix a, Matrix b) //dělení matice maticí
        {
            Matrix pom = Invert(a);
            return pom * b;  
        }

        public static Matrix operator *(Matrix a, Matrix b) //násobení matice maticí
        {
            double[,] c;

            if (a.MatrixData.GetLength(1)==b.MatrixData.GetLength(0))
            {
                c = new double[b.MatrixData.GetLength(0), b.MatrixData.GetLength(1)];
                for (int i = 0; i < a.MatrixData.GetLength(0); i++)
                {
                    for (int j = 0; j < b.MatrixData.GetLength(1); j++)
                    {
                        for (int k = 0; k < a.MatrixData.GetLength(1); k++)
                        {
                            c[i, j] += a.MatrixData[i, k] * b.MatrixData[k, j];
                        }
                    }
                }
                return new Matrix(Roles.Result, c);

            }
            else
            {
                throw new Exception("Matice maji nespravny rozmer.");
            }
            
            
        }

        public static Matrix operator *(Matrix a, double b)
        {
            double[,] c = new double[a.MatrixData.GetLength(0), a.MatrixData.GetLength(1)];
            try
            {

                for (int i = 0; i < a.MatrixData.GetLength(0); i++)
                {
                    for (int j = 0; j < a.MatrixData.GetLength(1); j++)
                    {
                        c[i, j] = a.MatrixData[i, j] * b;
                    }
                }
            }
            catch (Exception e) { Console.WriteLine("{0} Error", e); }
            return new Matrix(Roles.Result,c);
        }

        //Inverzní matice
        public static Matrix Invert(Matrix matrix)
        {
            double[,] d = new double[matrix.MatrixData.GetLength(0), matrix.MatrixData.GetLength(1)];

            for (int i = 0; i < d.GetLength(0); i++)
            {
                for (int j = 0; j < d.GetLength(1); j++)
                {
                    d[i,j]= MinusOneOrPlusOne(i+j)*Determinant(matrix.SubMatrix(i, j));
                }
            }

            return new Matrix(Roles.Result, (Transpose(d) * (1 / Determinant(matrix.MatrixData))).MatrixData);
        }

        public static double MinusOneOrPlusOne(int x)
        {
            if (x % 2 != 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public double[,] SubMatrix(int radek, int sloupec)
        {
            double[,] pom = new double[MatrixData.GetLength(0) - 1, MatrixData.GetLength(1) - 1];
            int indexI = 0;

            for (int i = 0; i < MatrixData.GetLength(0); i++)
            {
                if (i != radek)
                {
                    int indexJ = 0;
                    for (int j = 0; j < MatrixData.GetLength(1); j++)
                    {
                        if (j != sloupec)
                        {
                            pom[indexI, indexJ] = MatrixData[i, j];
                            indexJ++;
                        }
                    }
                    indexI++;
                }
                  
            }

            return pom;
        }

        //Transponovaná matice
        public static Matrix Transpose(Matrix m)
        {
            return Transpose(m.MatrixData);
        }

        public static Matrix Transpose(double[,] matrix)
        {
            double[,] c = new double[matrix.GetLength(1),matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    c[j, i] = matrix[i, j];
                }
            }
            return new Matrix(Roles.Result, c);
        }

        //Determinant
        //Liebnizovo pravidlo
        public static double Determinant(Matrix m)
        {
            return Determinant(m.MatrixData);
        }
        public static double Determinant(double[,] submatrix)
        {
            double d = 0;
            //matice 5x5 
            if (submatrix.GetLength(0) == 5 && submatrix.GetLength(1) == 5)
            {
                d = (submatrix[0, 4] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 1] -
                    submatrix[0, 3] * submatrix[1, 4] * submatrix[2, 2] * submatrix[3, 1] -
                    submatrix[0, 4] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 1] +
                    submatrix[0, 2] * submatrix[1, 4] * submatrix[2, 3] * submatrix[3, 1] +
                    submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 4] * submatrix[3, 1] -
                    submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 4] * submatrix[3, 1] -
                    submatrix[0, 4] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 2] +
                    submatrix[0, 3] * submatrix[1, 4] * submatrix[2, 1] * submatrix[3, 2] +
                    submatrix[0, 4] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 2] -
                    submatrix[0, 1] * submatrix[1, 4] * submatrix[2, 3] * submatrix[3, 2] -
                    submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 4] * submatrix[3, 2] +
                    submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 4] * submatrix[3, 2] +
                    submatrix[0, 4] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 3] -
                    submatrix[0, 2] * submatrix[1, 4] * submatrix[2, 1] * submatrix[3, 3] -
                    submatrix[0, 4] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 3] +
                    submatrix[0, 1] * submatrix[1, 4] * submatrix[2, 2] * submatrix[3, 3] +
                    submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 4] * submatrix[3, 3] -
                    submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 4] * submatrix[3, 3] -
                    submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 4] +
                    submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 4] +
                    submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 4] -
                    submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 4] -
                    submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 4] +
                    submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 4]) * submatrix[4, 0] -
                    (submatrix[0, 4] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 0] -
                    submatrix[0, 3] * submatrix[1, 4] * submatrix[2, 2] * submatrix[3, 0] -
                    submatrix[0, 4] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 0] +
                    submatrix[0, 2] * submatrix[1, 4] * submatrix[2, 3] * submatrix[3, 0] +
                    submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 4] * submatrix[3, 0] -
                    submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 4] * submatrix[3, 0] -
                    submatrix[0, 4] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 2] +
                    submatrix[0, 3] * submatrix[1, 4] * submatrix[2, 0] * submatrix[3, 2] +
                    submatrix[0, 4] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 2] -
                    submatrix[0, 0] * submatrix[1, 4] * submatrix[2, 3] * submatrix[3, 2] -
                    submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 4] * submatrix[3, 2] +
                    submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 4] * submatrix[3, 2] +
                    submatrix[0, 4] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 3] -
                    submatrix[0, 2] * submatrix[1, 4] * submatrix[2, 0] * submatrix[3, 3] -
                    submatrix[0, 4] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 3] +
                    submatrix[0, 0] * submatrix[1, 4] * submatrix[2, 2] * submatrix[3, 3] +
                    submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 4] * submatrix[3, 3] -
                    submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 4] * submatrix[3, 3] -
                    submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 4] +
                    submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 4] +
                    submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 4] -
                    submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 4] -
                    submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 4] +
                    submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 4]) * submatrix[4, 1] +
                    (submatrix[0, 4] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 0] -
                    submatrix[0, 3] * submatrix[1, 4] * submatrix[2, 1] * submatrix[3, 0] -
                    submatrix[0, 4] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 0] +
                    submatrix[0, 1] * submatrix[1, 4] * submatrix[2, 3] * submatrix[3, 0] +
                    submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 4] * submatrix[3, 0] -
                    submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 4] * submatrix[3, 0] -
                    submatrix[0, 4] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 1] +
                    submatrix[0, 3] * submatrix[1, 4] * submatrix[2, 0] * submatrix[3, 1] +
                    submatrix[0, 4] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 1] -
                    submatrix[0, 0] * submatrix[1, 4] * submatrix[2, 3] * submatrix[3, 1] -
                    submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 4] * submatrix[3, 1] +
                    submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 4] * submatrix[3, 1] +
                    submatrix[0, 4] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 3] -
                    submatrix[0, 1] * submatrix[1, 4] * submatrix[2, 0] * submatrix[3, 3] -
                    submatrix[0, 4] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 3] +
                    submatrix[0, 0] * submatrix[1, 4] * submatrix[2, 1] * submatrix[3, 3] +
                    submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 4] * submatrix[3, 3] -
                    submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 4] * submatrix[3, 3] -
                    submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 4] +
                    submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 4] +
                    submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 4] -
                    submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 4] -
                    submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 4] +
                    submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 4]) * submatrix[4, 2] -
                    (submatrix[0, 4] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 0] -
                    submatrix[0, 2] * submatrix[1, 4] * submatrix[2, 1] * submatrix[3, 0] -
                    submatrix[0, 4] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 0] +
                    submatrix[0, 1] * submatrix[1, 4] * submatrix[2, 2] * submatrix[3, 0] +
                    submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 4] * submatrix[3, 0] -
                    submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 4] * submatrix[3, 0] -
                    submatrix[0, 4] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 1] +
                    submatrix[0, 2] * submatrix[1, 4] * submatrix[2, 0] * submatrix[3, 1] +
                    submatrix[0, 4] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 1] -
                    submatrix[0, 0] * submatrix[1, 4] * submatrix[2, 2] * submatrix[3, 1] -
                    submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 4] * submatrix[3, 1] +
                    submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 4] * submatrix[3, 1] +
                    submatrix[0, 4] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 2] -
                    submatrix[0, 1] * submatrix[1, 4] * submatrix[2, 0] * submatrix[3, 2] -
                    submatrix[0, 4] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 2] +
                    submatrix[0, 0] * submatrix[1, 4] * submatrix[2, 1] * submatrix[3, 2] +
                    submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 4] * submatrix[3, 2] -
                    submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 4] * submatrix[3, 2] -
                    submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 4] +
                    submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 4] +
                    submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 4] -
                    submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 4] -
                    submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 4] +
                    submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 4]) * submatrix[4, 3] +
                    (submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 0] -
                    submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 0] -
                    submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 0] +
                    submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 0] +
                    submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 0] -
                    submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 0] -
                    submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 1] +
                    submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 1] +
                    submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 1] -
                    submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 1] -
                    submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 1] +
                    submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 1] +
                    submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 2] -
                    submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 2] -
                    submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 2] +
                    submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 2] +
                    submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 2] -
                    submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 2] -
                    submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 3] +
                    submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 3] +
                    submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 3] -
                    submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 3] -
                    submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 3] +
                    submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 3]) * submatrix[4, 4];
                return d;
            }
            //matice 4x4
            else if (submatrix.GetLength(0) == 4 && submatrix.GetLength(1) == 4)
            {
                d = (submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 3]) - (submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 2]) - (submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 3])
                    + (submatrix[0, 0] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 1]) + (submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 2]) - (submatrix[0, 0] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 1])
                    - (submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 3]) + (submatrix[0, 1] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 3]) + (submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 3])
                    - (submatrix[0, 1] * submatrix[1, 2] * submatrix[2, 3] * submatrix[3, 0]) - (submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 2]) + (submatrix[0, 1] * submatrix[1, 3] * submatrix[2, 2] * submatrix[3, 0])
                    + (submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 3]) - (submatrix[0, 2] * submatrix[1, 0] * submatrix[2, 3] * submatrix[3, 1]) - (submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 3])
                    + (submatrix[0, 2] * submatrix[1, 1] * submatrix[2, 3] * submatrix[3, 0]) + (submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 0] * submatrix[3, 1]) - (submatrix[0, 2] * submatrix[1, 3] * submatrix[2, 1] * submatrix[3, 0])
                    - (submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 1] * submatrix[3, 2]) + (submatrix[0, 3] * submatrix[1, 0] * submatrix[2, 2] * submatrix[3, 1]) + (submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 0] * submatrix[3, 2])
                    - (submatrix[0, 3] * submatrix[1, 1] * submatrix[2, 2] * submatrix[3, 0]) - (submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 0] * submatrix[3, 1]) + (submatrix[0, 3] * submatrix[1, 2] * submatrix[2, 1] * submatrix[3, 0]);
                return d;
            }   
            //matice 3x3
            else if (submatrix.GetLength(0) == 3 && submatrix.GetLength(1) == 3)
            {
                d = (submatrix[0, 0] * submatrix[1, 1] * submatrix[2, 2]) + (submatrix[1, 0] * submatrix[2, 1] * submatrix[0, 2]) + (submatrix[2, 0] * submatrix[0, 1] * submatrix[1, 2]) - (submatrix[1, 0] * submatrix[0, 1] * submatrix[2, 2])
                    - (submatrix[0, 0] * submatrix[2, 1] * submatrix[1, 2]) - (submatrix[2, 0] * submatrix[1, 1] * submatrix[0, 2]);
                return d;
            }
            //matice 2x2
            else if (submatrix.GetLength(0) == 2 && submatrix.GetLength(1) == 2)
            {
                d = (submatrix[0, 0] * submatrix[1, 1]) - (submatrix[1, 0] * submatrix[0, 1]);
                return d;
            }
            else if (submatrix.GetLength(0) ==1 && submatrix.GetLength(1) == 1)
            {
                d = (submatrix[0, 0]);
                return d;
            }
            else
            {
                throw new Exception("Nelze spocitat determinant matice.");
            }

        }
    }

}
