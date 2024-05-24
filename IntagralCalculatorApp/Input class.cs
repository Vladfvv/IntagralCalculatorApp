using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntagralCalculatorApp
{
    public class InputClass : INotifyPropertyChanged
    {


        //public double From { get; set; }
        //public double To { get; set; }
        //public int Step { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private double _from;
        private double _to;
        private int _step;
        public double From
        {
            get { return _from; }
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged();
                }
            }
        }
        public double To
        {
            get { return _to; }
            set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Step
        {
            get { return _step; }
            set
            {
                if (_step != value)
                {
                    _step = value;
                    OnPropertyChanged();
                }
            }
        }


        private double CalculateIntegral(double From, double To, int Step)
        {
            double h = (To - From) / Step;
            double sum = 0.0;

            for (int i = 0; i < Step; i++)
            {
                double x = From + h * i;
                sum += Math.Sqrt(x) * h;
               // reportProgress((i + 1) * 100 / n);
            }

            return sum;
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //public double Result { get; set; }

        /*public InputClass() {}

        public InputClass(double from, double to, int step)
        {
            From = from;
            To = to;
            Step = step;
        }*/
    }


}

