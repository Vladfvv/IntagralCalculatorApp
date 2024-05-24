using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            inputClass = new InputClass();
            // BackgroundWorker
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));

            // в коде
            //backgroundWorker.WorkerReportsProgress = true;
            //backgroundWorker.WorkerSupportsCancellation = true;
            //backgroundWorker.DoWork += backgroundWorker_DoWork;
            //backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            //backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            //backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
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






        private void dispatcherButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherButton.IsEnabled = false;
            backgroundWorkerButton.IsEnabled = false;
            pBar.Value = 0;            
            double result = 0.0;
            if (ShowInputDialog())
            {


                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    result = CalculateIntegral(lowerBound, upperBound, intervals, ReportProgress);

                });
            }
            ResultTextBlock.Text = $"Result: {result}";
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;            
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
            }

            return sum;
        }




        private void backgroundWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherButton.IsEnabled = false;
            backgroundWorkerButton.IsEnabled = false;            
            pBar.Value = 0;
            if (ShowInputDialog())
            {
                BackgroundWorker worker = new BackgroundWorker
                {
                    WorkerReportsProgress = true
                };
                worker.DoWork += (s, args) =>
                {
                    args.Result = CalculateIntegral(lowerBound, upperBound, intervals, worker.ReportProgress);
                };
                worker.ProgressChanged += (s, args) =>
                {
                    pBar.Value = args.ProgressPercentage;
                };
                worker.RunWorkerCompleted += (s, args) =>
                {
                    ResultTextBlock.Text = $"Result: {args.Result}";

                };
                worker.RunWorkerAsync();               
            }
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            //{
            //    backgroundWorker.RunWorkerAsync(inputClass);               
            //}
        }

        
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
         //  InputClass inputClass = (InputClass)e.Argument;
        //    double result = CalculateIntegral(inputClass, backgroundWorker);
        }
        private void ReportProgress(int progress)
        {
            Dispatcher.Invoke(() => pBar.Value = progress);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           // pBar.Value = e.ProgressPercentage;

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}