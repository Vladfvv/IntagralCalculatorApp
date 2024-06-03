using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntagralCalculatorApp
{
    class Worker
    {
        /* private double CalculateIntegral(double a, double b, int n)
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
         }*/


        private double CalculateIntegral(InputClass inputClass)
        {
            //double h = (b - a) / n;
            //double sum = 0.0;

            //for (int i = 0; i < n; i++)
            //{
            //    double x = a + h * i;
            //    sum += Math.Sqrt(x) * h;
            //reportProgress((i + 1) * 100 / n);
            //}

            return CalculateIntegral(inputClass, null);
        }


        public static double CalculateIntegral(InputClass inputClass, BackgroundWorker backgroundWorker)
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
                //reportProgress((i + 1) * 100 / inputClass.Step);

                if ((i % n == 0) && (backgroundWorker != null))
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        // Возврат без какой-либо дополнительной работы
                        return 0;
                    }
                    if (backgroundWorker.WorkerReportsProgress)
                    {
                        backgroundWorker.ReportProgress(i / n);
                    }
                }
            }
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(100);
            }

            return sum;
        }


        //public static int[] Find(int fromNumber, int toNumber)
        //{
        //    return Find(fromNumber, toNumber, null);
        //}

        
        /*
        public static int[] Find(int fromNumber, int toNumber, BackgroundWorker backgroundWorker)
        {




            double a = a_helper.From;
            double b = a_helper.To;
            int n = a_helper.Step;
            double result = 0;

            double h = (b - a) / n;
            double integral = 0;
            var step = Math.Round((double)n / 100);
            for (int i = 0; i <= n; i++)
            {
                double xi = a + i * h;
                integral += Function(xi);

                if (i % step == 0 && (backgroundWorker != null))
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        return 0;
                    }
                    if (backgroundWorker.WorkerReportsProgress)
                    {
                        backgroundWorker.ReportProgress((int)Math.Round(i / step));
                    }
                }
            }
            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(100);
            }

            a_helper.Result = (integral *= h);

            result = a_helper.Result;

            // return result;









            int[] list = new int[toNumber - fromNumber];

            // Создать массив, содержащий все целые числа
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = fromNumber;
                fromNumber += 1;
            }

            // Числа, кратные всем простым числам, меньшим или равным квадратному 
            // корню из максимального числа отмечаем цифрой 0 - это обычные числа.
            // Все остальные отмечаем 1 - это простые числа
            int maxDiv = (int)Math.Floor(Math.Sqrt(toNumber));

            int[] mark = new int[list.Length];


            for (int i = 0; i < list.Length; i++)
            {
                for (int j = 2; j <= maxDiv; j++)
                {

                    if ((list[i] != j) && (list[i] % j == 0))
                    {
                        mark[i] = 1;
                    }

                }

                int iteration = list.Length / 100;
                if ((i % iteration == 0) && (backgroundWorker != null))
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        // Возврат без какой-либо дополнительной работы
                        return null;
                    }

                    if (backgroundWorker.WorkerReportsProgress)
                    {

                        backgroundWorker.ReportProgress(i / iteration);

                    }
                }

            }

            // Cоздать новый массив, который содержит только простые числа, и вернуть этот массив
            int primes = 0;
            for (int i = 0; i < mark.Length; i++)
            {
                if (mark[i] == 0) primes += 1;

            }

            int[] ret = new int[primes];
            int curs = 0;
            for (int i = 0; i < mark.Length; i++)
            {
                if (mark[i] == 0)
                {
                    ret[curs] = list[i];
                    curs += 1;
                }
            }

            if (backgroundWorker != null && backgroundWorker.WorkerReportsProgress)
            {
                backgroundWorker.ReportProgress(100);
            }

            return ret;
        }*/
    }
}
