using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace IntagralCalculatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       InputClass inputClass;
        BackgroundWorker backgroundWorker;
        private double lowerBound;
        private double upperBound;
        private int intervals;

        public MainWindow()
        {           
            InitializeComponent();
            MessageBox.Show("MainProcess: " + Thread.CurrentThread.ManagedThreadId);
            inputClass = new InputClass();
            //cancelButton.IsEnabled = false;
            // Инициализация BackgroundWorker
           // backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;





            // Инициализация BackgroundWorker
            /*       backgroundWorker = new BackgroundWorker();
                    backgroundWorker.DoWork += backgroundWorker_DoWork;
                    backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                    backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
                    backgroundWorker.WorkerReportsProgress = true;
                    backgroundWorker.WorkerSupportsCancellation = true;*/
        }


        private bool ShowInputDialog()//диалоговое окно
        {
            DialogWindow dw = new DialogWindow();
            if (dw.ShowDialog() == true)
            {
                lowerBound = dw.LowerBound;
                upperBound = dw.UpperBound;
                intervals = dw.Intervals;
                inputClass.From = dw.LowerBound;
                inputClass.To = dw.UpperBound;
                inputClass.Step = dw.Intervals;
                return true;
            }
            return false;
        }

      



        /*
        private void dispatcherButton_Click(object sender, RoutedEventArgs e)
        {

          //  double result = 0.0;
            if (ShowInputDialog())
            {
                dispatcherButton.IsEnabled = false;
                backgroundWorkerButton.IsEnabled = false;
                pBar.Value = 0;

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    double result = CalculateIntegral(lowerBound, upperBound, intervals, ReportProgress);
                    ResultTextBlock.Text = $"Result: {result}";
                }));
            }
            ResultTextBlock.Text = $"Result: {result}";
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
        }
        */


        private void dispatcherButton_Click(object sender, RoutedEventArgs e)

        {
            if (ShowInputDialog())
            { 
                dispatcherButton.IsEnabled = false;
                backgroundWorkerButton.IsEnabled = false;
                pBar.Value = 0;
                
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    double result = CalculateIntegral(lowerBound, upperBound, intervals, ReportProgress);
                    ResultTextBlock.Text = $"Result: {result}";
                    dispatcherButton.IsEnabled = true;
                    backgroundWorkerButton.IsEnabled = true;
                }));
            }
        }



        private double CalculateIntegral(double a, double b, int n, Action<int> reportProgress)
        {
            double h = (b - a) / n;
            double sum = 0.0;

            for (int i = 0; i < n; i++)
            {
                double x = a + h * i;
                sum += Math.Sqrt(x) * h;
                reportProgress((i + 1) * 100 / n);
            }

            return sum;
        }


        private double CalculateIntegral(InputClass inputClass, Action<int> reportProgress)
        {
            double h = (inputClass.To - inputClass.From) / inputClass.Step;
            double sum = 0.0;

            for (int i = 0; i < inputClass.Step; i++)
            {
                double x = inputClass.From + h * i;
                sum += Math.Sqrt(x) * h;
                reportProgress((i + 1) * 100 / inputClass.Step);
                //pBar.Value = sum;
            }

            return sum;
        }




        private double CalculateIntegral(InputClass inputClass, BackgroundWorker background)
        {
            double h = (inputClass.To - inputClass.From) / inputClass.Step;
            double sum = 0.0;

            for (int i = 0; i < inputClass.Step; i++)
            {
                double x = inputClass.From + h * i;
                sum += Math.Sqrt(x) * h;
                

                int iteration = (int)sum / 100;
                if ((i % iteration == 0) && (backgroundWorker != null))
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        // Возврат без какой-либо дополнительной работы
                        return 0.0;
                    }

                    if (backgroundWorker.WorkerReportsProgress)
                    {

                        backgroundWorker.ReportProgress(i / iteration);

                       // reportProgress((i + 1) * 100 / inputClass.Step);
                    }
                }

                //pBar.Value = sum;
            }

            return sum;
        }







        private bool ShowInputDialog(InputClass inputClass)//диалоговое окно
        {
            DialogWindow dw = new DialogWindow();
            if (dw.ShowDialog() == true)
            {
                lowerBound = dw.LowerBound;
                upperBound = dw.UpperBound;
                intervals = dw.Intervals;
                inputClass.From = dw.LowerBound;
                inputClass.To = dw.UpperBound;
                inputClass.Step = dw.Intervals;
                MessageBox.Show(inputClass.From.ToString() + " " + inputClass.To.ToString() + " " + inputClass.Step.ToString());
                return true;
            }
            return false;
        }


        private void backgroundWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowInputDialog(inputClass))
            {
                dispatcherButton.IsEnabled = false;
                backgroundWorkerButton.IsEnabled = false;
                cancelButton.IsEnabled = true;
                pBar.Value = 0;
                
                backgroundWorker.RunWorkerAsync(inputClass);
            }
        }

        private void ReportProgress(int progress)
        {
            Dispatcher.Invoke(() => pBar.Value = progress);
        }
        /*
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
        }


        private void btnstop_Click(object sender, RoutedEventArgs e)
        {/*
            if (backgroundWorker.IsBusy)
            {
                // Отмена BackgroundWorker
                backgroundWorker.CancelAsync();
            }
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
            // pBar.Value = 0;

            backgroundWorker.CancelAsync();
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;

        }*/

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
        }
        /*
        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
           // if (backgroundWorker.IsBusy)
           // {
                backgroundWorker.CancelAsync();
           // }
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            //pBar.Value = 0;
        }*/



        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            {
                InputClass input = (InputClass)e.Argument;
                e.Result = CalculateIntegral(input, progress =>
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    backgroundWorker.ReportProgress(progress);
                });
            }
        }

            private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        /*
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double result = 0.0;
            if (e.Cancelled)
            {
                MessageBox.Show("Поиск отменен");
            }
            else
            if (e.Error != null)
            {
                // Ошибка была сгенерирована обработчиком события DoWork
                MessageBox.Show(e.Error.Message, "Произошла ошибка");
            }
            else result = (double)e.Result;

            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;            
        }*/


        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Поиск отменен");
            }
            else if (e.Error != null)
            {// Ошибка была сгенерирована обработчиком события DoWork
                MessageBox.Show($"Error: {e.Error.Message}", "Произошла ошибка");
            }
            else
            {
                double result = (double)e.Result;
                ResultTextBlock.Text = $"Result: {result}";
            }

            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
        }

        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
            backgroundWorker.CancelAsync();
           
        }
    }
}