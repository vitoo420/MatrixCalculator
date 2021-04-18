﻿using System;
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

        /// <summary>
        /// 
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
        /// Vytvori pole TextBoxu reprezentujici matici, TextBoxy jsou umisteny do gridu, ktery je urceny pro matici.
        /// </summary>
        /// <param name="grid">Sub Grid matice</param>
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
                }
            }

        }

        public void TextBoxMatrixIsEnabled(Grid grid, bool isEnabled)
        {
            bool _isEnabled = isEnabled;
            foreach (var item in this.TextBoxMatrix)
            {
                item.IsEnabled = _isEnabled;
            }
        }

        public static void DeleteMatrix(Matrix matrix, Grid grid)
        {
            foreach (var item in matrix.TextBoxMatrix)
            {
                grid.Children.Remove(item);
            }
        }

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

        //Bude vracet objekt Matrix
        public static void Solve(MatrixOperations operation, Matrix a, Matrix b)
        {
            a.TextBoxArrayToDoubleArray();
            b.TextBoxArrayToDoubleArray();
            switch (operation)
            {
                case MatrixOperations.add:
                    break;
                case MatrixOperations.subtract:
                    break;
                case MatrixOperations.multiply:
                    break;
                case MatrixOperations.determinant:
                    break;
                case MatrixOperations.inverse:
                    break;
                case MatrixOperations.transpose:
                    break;
                default:
                    break;
            }
        }
    }

}
