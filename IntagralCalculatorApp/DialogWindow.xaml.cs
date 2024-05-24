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
using System.Windows.Shapes;
using System.Windows.Threading;
using static IntagralCalculatorApp.MainWindow;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IntagralCalculatorApp
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public double LowerBound { get; private set; }
        public double UpperBound { get; private set; }
        public int Intervals { get; private set; }
        public DialogWindow()
        {
            InitializeComponent();

        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(lowerTextBox.Text, out double lowerBound) &&
               double.TryParse(upperTextBox.Text, out double upperBound) &&
               int.TryParse(textBoxPartitions.Text, out int intervals))
            {
                LowerBound = lowerBound;
                UpperBound = upperBound;
                Intervals = intervals;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid numeric values.");
            }
        }

        private void GetParametersFromDialogWindow()
        {

            /*if (double.TryParse(lowerTextBox.Text, out double lowerBound) &&
              double.TryParse(UpperBoundTextBox.Text, out double upperBound) &&
              int.TryParse(IntervalsTextBox.Text, out int intervals))
            {
                LowerBound = lowerBound;
                UpperBound = upperBound;
                Intervals = intervals;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid numeric values.");
            }
           


            string minLimit = lowerTextBox.Text;
            if (!double.TryParse(minLimit, out double a))
            {
                Console.WriteLine("Введенное значение не является числом double.");
                return;
            }

            string maxLimit = upperTextBox.Text;
            if (!double.TryParse(maxLimit, out double b))
            {
                Console.WriteLine("Введенное значение не является числом double.");
                return;
            }

            string part = textBoxPartitions.Text;
            if (!double.TryParse(part, out double n))
            {
                Console.WriteLine("Введенное значение не является числом double.");
                return;
            }
            
            double h = 0.0;
            double sum = 0.0;
            double x = 0.0;
            var step = Math.Round(n / 100);
            h = (b - a) / n;

            await Task.Run(() =>
            {
                for (int i = 0; i <= n; i++)
                {
                    x = Math.Sqrt(a + h * i);
                    sum += x;

                    if (i % step == 0)
                    {
                     //   Dispatcher.Invoke(() => pBar.Value = i / step);
                    }
                }
            });*/
        }
    }

}



