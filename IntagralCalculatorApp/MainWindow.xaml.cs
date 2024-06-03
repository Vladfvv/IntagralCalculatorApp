
using System.ComponentModel;
using System.Windows;

using System.Windows.Threading;

namespace IntagralCalculatorApp
{
    public partial class MainWindow : Window
    {
        private InputClass inputClass;
        BackgroundWorker backgroundWorker;
        private double lowerBound;
        private double upperBound;
        private int intervals;

        public MainWindow()
        {
            InitializeComponent();
            inputClass = new InputClass();

            // Инициализация BackgroundWorker
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            cancelButton.IsEnabled = false;


        }

        private bool ShowInputDialog()
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

        private bool ShowInputDialog(InputClass inputClass)
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
            cancelButton.IsEnabled = false;
            pBar.Value = 0;
            ResultTextBlock.Text = "";
            ResultTextBlock2.Text = "";
               
            if (ShowInputDialog())
            {
                Thread t = new Thread(CalculateIntegral1);
                t.Start();
               
            }
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
        }

        private void CalculateIntegral1()
        {
            double a = inputClass.From;
            double b = inputClass.To;
            int n = inputClass.Step;

            double h = (b - a) / n;
            double sum = 0.0;

            for (int i = 0; i < n; i++)
            {
                double x = a + h * i;
                sum += Math.Sqrt(x) * h;
               
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    // double result = CalculateIntegral(lowerBound, upperBound, intervals, ReportProgress);
                     ResultTextBlock.Text = $"Result: {sum}";
                    //reportProgress((i + 1) * 100 / n);
                    pBar.Value = i;
                }));
            }

           // return sum;
        }
        /*
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
        }*/


        private double CalculateIntegral(InputClass inputClass, BackgroundWorker backgroundWorker)
        {
            double h = (inputClass.To - inputClass.From) / inputClass.Step;
            double sum = 0.0;

            for (int i = 0; i < inputClass.Step; i++)
            {
                double x = inputClass.From + h * i;
                sum += Math.Sqrt(x) * h;
                //reportProgress((i + 1) * 100 / inputClass.Step);
                if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
                {
                    backgroundWorker.ReportProgress(100);
                }


            }
            return sum;
        }




        private void backgroundWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherButton.IsEnabled = false;
            backgroundWorkerButton.IsEnabled = false;
            pBar.Value = 0;
            cancelButton.IsEnabled = true;
            ResultTextBlock.Text = "";
            ResultTextBlock2.Text = "";
            if (ShowInputDialog(inputClass))
            {
                
                backgroundWorker.RunWorkerAsync(inputClass);
            }
        }

        private void ReportProgress(int progress)
        {
            Dispatcher.Invoke(() => pBar.Value = progress);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
        }

        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
            MessageBox.Show("Вы прервали операцию вычисления");
            //pBar.Value = 0;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InputClass input = (InputClass)e.Argument;
            double res = CalculateIntegral(input, backgroundWorker);
            
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
               // backgroundWorker.ReportProgress(progress);
            

            e.Result = res;



        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Operation cancelled");
            }
            else if (e.Error != null)
            {
                MessageBox.Show($"Error: {e.Error.Message}");
            }
            else
            {
                double result = (double)e.Result;
                ResultTextBlock2.Text = $"Result: {result}";
                
            }

            dispatcherButton.IsEnabled = true;
            backgroundWorkerButton.IsEnabled = true;
            cancelButton.IsEnabled = false;
        }
    }
}