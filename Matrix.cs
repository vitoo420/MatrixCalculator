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

        public Matrix(Roles role)
        {
            Role = role;
            NumOfRows = 3;
            NumOfColumns = 3;
        }

        public Matrix(double[,] MatrixData)     //Pouze pro "zpetnou kompatibilitu" - potom smazat
        {
            this.MatrixData = MatrixData;
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
                return new Matrix(addedMatrix);
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
                return new Matrix(substractedMatrix);
            }
            else
            {
                throw new Exception("Matice nemaji stejny rozmer!");
            }
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            double[,] multiplicatedMatrix;

            if (a.MatrixData.GetLength(0) == b.MatrixData.GetLength(1))
            {
                int m = a.MatrixData.GetLength(1);
                int n = a.MatrixData.GetLength(0);
                int p = b.MatrixData.GetLength(1);
                int q = b.MatrixData.GetLength(0);

                multiplicatedMatrix = new double[m, q];

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        multiplicatedMatrix[i, j] = 0;
                        for (int k = 0; k < n; k++)
                        {
                            multiplicatedMatrix[i, j] += a.MatrixData[i, k] * b.MatrixData[k, j];
                        }
                    }
                }
                return new Matrix(multiplicatedMatrix);
            }
            else
            {
                throw new Exception("Matice nejsou typu (m*n) * (n*p)!");
            }
        }

        public static bool operator ==(Matrix a, Matrix b)
        {
            if (a.MatrixData.GetLength(0) == b.MatrixData.GetLength(0) && a.MatrixData.GetLength(1) == b.MatrixData.GetLength(1))
            {
                for (int width = 0; width < a.MatrixData.GetLength(0); width++)
                {
                    for (int height = 0; height < a.MatrixData.GetLength(1); height++)
                    {
                        if (a.MatrixData[width, height] != b.MatrixData[width, height])
                            return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            if (a.MatrixData.GetLength(0) != b.MatrixData.GetLength(0) || a.MatrixData.GetLength(1) != b.MatrixData.GetLength(1))
            {
                return true;
            }

            for (int width = 0; width < a.MatrixData.GetLength(0); width++)
            {
                for (int height = 0; height < a.MatrixData.GetLength(1); height++)
                {
                    if (a.MatrixData[width, height] != b.MatrixData[width, height])
                        return true;
                }
            }
            return false;


        }

        public static Matrix operator -(Matrix matrix)
        {
            double[,] unary = new double[matrix.MatrixData.GetLength(0), matrix.MatrixData.GetLength(1)];

            for (int width = 0; width < matrix.MatrixData.GetLength(0); width++)
            {
                for (int height = 0; height < matrix.MatrixData.GetLength(1); height++)
                {
                    unary[width, height] = -matrix.MatrixData[width, height];
                }
            }

            return new Matrix(unary);
        }

        public double Determinant()
        {
            if (MatrixData.GetLength(0) != MatrixData.GetLength(1))
                throw new Exception("Matice neni ctvercova!");
            else
            {
                if (MatrixData.Length == 1)
                    return MatrixData[0, 0];
                else if (MatrixData.GetLength(0) == 2)
                    return MatrixData[0, 0] * MatrixData[1, 1] - MatrixData[0, 1] * MatrixData[1, 0];
                else if (MatrixData.GetLength(0) == 3)
                {
                    double determinant = MatrixData[0, 0] * MatrixData[1, 1] * MatrixData[2, 2]
                                + MatrixData[0, 1] * MatrixData[1, 2] * MatrixData[2, 0]
                                + MatrixData[0, 2] * MatrixData[1, 0] * MatrixData[2, 1]
                                -
                                (MatrixData[0, 2] * MatrixData[1, 1] * MatrixData[2, 0]
                                + MatrixData[0, 0] * MatrixData[1, 2] * MatrixData[2, 1]
                                + MatrixData[0, 1] * MatrixData[1, 0] * MatrixData[2, 2]);
                    return determinant;
                }
                else
                    throw new Exception("Toto jeste neumim :(");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid">Sub Grid matice</param>
        /// <param name="row">radek v Sub Gridu</param>
        /// <param name="col">sloupec v Sub Gridu</param>
        /// <returns>TextBox objekt</returns>
        private TextBox CreateATextBox(Grid grid, int row, int col)
        {
            TextBox txtb = new TextBox();
            grid.Children.Add(txtb);
            Grid.SetRow(txtb, row);
            Grid.SetColumn(txtb, col);
            txtb.Foreground = new SolidColorBrush(Colors.Black);
            return txtb;
        }

        /// <summary>
        /// Vytvori pole TextBoxu reprezentujici matici, TextBoxy jsou umisteny do gridu, ktery je urceny pro matici.
        /// </summary>
        /// <param name="grid">Sub Grid matice</param>
        public void CreateTextBoxMatrix(Grid grid)
        {
            int iPlus = 0;

            switch (NumOfRows)
            {
                case 1:
                    iPlus = 2;
                    break;
                case 2:
                    iPlus = 1;
                    break;
                case 3:
                    iPlus = 1;
                    break;
                case 4:
                    iPlus = 0;
                    break;
                case 5:
                    iPlus = 0;
                    break;
                default:
                    break;
            }

            //v matematice i v c# multidim. polich se prvni pise radek, potom sloupec
            TextBoxMatrix = new TextBox[NumOfRows, NumOfColumns];

            switch (Role)
            {
                case Roles.First:
                    for (int i = 0; i < NumOfRows; i++)
                    {
                        for (int j = 0; j < NumOfColumns; j++)
                        {
                            TextBoxMatrix[i, j] = CreateATextBox(grid, i + iPlus, j + (5 - NumOfColumns));
                        }
                    }
                    break;
                case Roles.Second:
                    for (int i = 0; i < NumOfRows; i++)
                    {
                        for (int j = 0; j < NumOfColumns; j++)
                        {
                            TextBoxMatrix[i, j] = CreateATextBox(grid, i + iPlus, j);
                        }
                    }
                    break;
                case Roles.Result:
                    break;
                default:
                    break;
            }

        }

        public static void DeleteMatrix(Matrix matrix, Grid grid)
        {
            foreach (var item in matrix.TextBoxMatrix)
            {
                grid.Children.Remove(item);
            }
        }

        public override string ToString()
        {
            string result = "";

            for (int width = 0; width < MatrixData.GetLength(0); width++)
            {
                for (int height = 0; height < MatrixData.GetLength(1); height++)
                {
                    result += String.Format(MatrixData[width, height].ToString() + " ");
                }
                result += "\n";
            }
            return result;
        }
    }

}
