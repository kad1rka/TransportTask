using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace TransportTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static TextBox[,] grid;
        private static TextBox[] supply;
        private static TextBox[] demand;
        private static int rows = 3;
        private static int columns = 5;
        public MainWindow()
        {
           
            InitializeComponent();
            tableGrid.ShowGridLines = true;
            grid = new TextBox[rows, columns];
            supply = new TextBox[grid.GetLength(0)];
            demand = new TextBox[grid.GetLength(1)];
            
            for (int i = 0; i < rows+1; ++i)
            {
                for (int j = 0; j < columns+1; j++)
                {
                    if (i == rows && j == columns)
                    {
                        continue;
                    }
                    TextBox tx = new TextBox();
                    Grid.SetRow(tx, i);
                    Grid.SetColumn(tx, j);
                    if (i < rows && j < columns)
                    {
                        grid[i,j] = tx;
                    }
                    if (j == 5)
                    {
                        supply[i] = tx;
                        tx.Background = new SolidColorBrush(Color.FromRgb(225, 135, 136));
                    }
                    if (i == 3)
                    {
                        demand[j] = tx;
                        tx.Background = new SolidColorBrush(Color.FromRgb(225, 225, 136));
                    }
                    tx.FontSize = 20;
                    tx.VerticalContentAlignment = VerticalAlignment.Center;
                    tx.HorizontalContentAlignment = HorizontalAlignment.Center;
                    tableGrid.Children.Add(tx);
                }

            }
            

        }

        private void NorthWestButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                int[,] gridInt = new int[rows, columns];
                int[] supplyInt = new int[gridInt.GetLength(0)];
                int[] demandInt = new int[gridInt.GetLength(1)];
                for (int i = 0; i < grid.GetLength(0); ++i)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        gridInt[i, j] = int.Parse(grid[i, j].Text);
                    }

                }

                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    supplyInt[i] = int.Parse(supply[i].Text);
                }
                for (int i = 0; i < grid.GetLength(1); i++)
                {
                    demandInt[i] = int.Parse(demand[i].Text);
                }

                var array = NorthWest(gridInt, supplyInt, demandInt);

                int sum = 0;

                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        sum += int.Parse(grid[i, j].Text) * array[i, j];
                        grid[i, j].Text = array[i, j].ToString();


                    }
                }

                resultTextBlock.Text = sum.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void MinElementButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int[,] gridInt = new int[rows, columns];
                int[] supplyInt = new int[gridInt.GetLength(0)];
                int[] demandInt = new int[gridInt.GetLength(1)];
                for (int i = 0; i < grid.GetLength(0); ++i)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        gridInt[i, j] = int.Parse(grid[i, j].Text);
                    }

                }

                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    supplyInt[i] = int.Parse(supply[i].Text);
                }
                for (int i = 0; i < grid.GetLength(1); i++)
                {
                    demandInt[i] = int.Parse(demand[i].Text);
                }

                var array = MinElement(gridInt, supplyInt, demandInt);

                int sum = 0;

                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        sum += int.Parse(grid[i, j].Text) * array[i, j];
                        grid[i, j].Text = array[i, j].ToString();


                    }
                }

                resultTextBlock.Text = sum.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Error ); 
            }

            
        }
        private int[,] NorthWest(int[,] grid, int[] supply, int[] demand)
        {
            int i = 0; // Индекс поставщика
            int j = 0; // Индекс клиента
            int[,] transportationPlan = new int[supply.Length, demand.Length];

            while (i < supply.Length && j < demand.Length)
            {
                // Выбираем минимальное значение между предложением и спросом
                int quantity = Math.Min(supply[i], demand[j]);
                transportationPlan[i, j] = quantity;

                // Вычитаем количество из запасов и потребностей
                supply[i] -= quantity;
                demand[j] -= quantity;

                // Если запасы исчерпаны, переходим к следующему поставщику
                if (supply[i] == 0)
                {
                    i++;
                }
                // Если потребности исчерпаны, переходим к следующему клиенту
                if (demand[j] == 0)
                {
                    j++;
                }
            }
            return transportationPlan;
        }

        private int[,] MinElement(int [,] grid, int[] supply, int[] demand)
        {
            int totalSupply = 0;
            int totalDemand = 0;
            int[,] transportationPlan = new int[supply.Length, demand.Length];


            // Считаем общее количество запасов и потребностей
            foreach (var s in supply) totalSupply += s;
            foreach (var d in demand) totalDemand += d;

            // Пока есть запасы и потребности
            while (totalSupply > 0 && totalDemand > 0)
            {
                // Находим минимальный элемент
                int minCost = int.MaxValue;
                int minRow = -1;
                int minCol = -1;

                for (int i = 0; i < supply.Length; i++)
                {
                    for (int j = 0; j < demand.Length; j++)
                    {
                        if (supply[i] > 0 && demand[j] > 0 && grid[i, j] < minCost)
                        {
                            minCost = grid[i, j];
                            minRow = i;
                            minCol = j;
                        }
                    }
                }

                // Выбираем максимальное количество, которое можно отправить
                int quantity = Math.Min(supply[minRow], demand[minCol]);
                transportationPlan[minRow, minCol] = quantity;

                // Обновляем запасы и потребности
                supply[minRow] -= quantity;
                demand[minCol] -= quantity;

                // Обновляем общее количество запасов и потребностей
                totalSupply -= quantity;
                totalDemand -= quantity;
            }
            return transportationPlan;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].Clear();
                    demand[j].Clear();
                }
                supply[i].Clear();
            }

            resultTextBlock.Text = "?";
        }

        
    }
}
