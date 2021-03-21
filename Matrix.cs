using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MatrixCalculator
{
    class Matrix
    {
        public int NumOfRows { get; set; } = 3;
        public int NumOfColumns { get; set; } = 3;
        public TextBox[,] TextBoxMatrix { get; set; }
        public double[,] MatrixData { get; set; }

        public int TextBoxMatXPos { get; set; }
        public int TextBoxMatYPos { get; set; }

        const int TextBoxHeight = 50;
        const int TextBoxWidth = 100;
        const int spaceBetween = 10; 

        public Matrix(int xPos, int yPos)
        {
            TextBoxMatXPos = xPos;
            TextBoxMatYPos = yPos;
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
        /// <param name="canvas">Kreslici plocha</param>
        /// <param name="x">X souradnice</param>
        /// <param name="y">Y souradnice</param>
        /// <returns>TextBox objekt</returns>
        private TextBox CreateATextBox(Canvas canvas, int x, int y)
        {
            TextBox txtb = new TextBox();

            //v matematice i v c# multidim. polich se prvni pise radek, potom sloupec

            txtb.Height = TextBoxHeight;
            txtb.Width = TextBoxWidth;
            txtb.FontSize = 20;

            //txtb.Background = new SolidColorBrush(Colors.Orange);
            txtb.Foreground = new SolidColorBrush(Colors.Black);
            
            canvas.Children.Add(txtb);

            Canvas.SetLeft(txtb, x);
            Canvas.SetTop(txtb, y);

            return txtb;
        }

        /// <summary>
        /// Vytvori pole TextBoxu reprezentujici matici, pozice je cerpana z vlastnosti objektu
        /// </summary>
        /// <param name="canvas">Kreslici plocha</param>
        public void CreateTextBoxMatrix(Canvas canvas)
        {
            TextBoxMatrix = new TextBox[NumOfRows, NumOfColumns];
            int tempX = TextBoxMatXPos;
            int tempY = TextBoxMatYPos;
            
            for (int i = 0; i < NumOfRows; i++)
            {
                for (int j = 0; j < NumOfColumns; j++)
                {
                    TextBoxMatrix[i, j] = CreateATextBox(canvas, tempX, tempY);

                    if (j == NumOfColumns - 1)
                    {
                        tempX -= (NumOfColumns - 1) * TextBoxWidth + (NumOfColumns -1) * spaceBetween;
                        tempY += TextBoxHeight + spaceBetween;
                    }    
                    else
                    {
                        tempX += TextBoxWidth + spaceBetween;
                    }      
                }
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
