using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Dashboard
{
    public class DashboardValues : INotifyPropertyChanged
    {
        private Int64 _Consumption;
        public Int64 Consumption
        {
            get
            {
                return _Consumption;
            }
            set
            {
                if (value != _Consumption)
                {
                    _Consumption = value;
                    NotifyPropertyChanged("Consumption");
                }
            }
        }



        private Decimal _PaymentReceived;
        public Decimal PaymentReceived
        {
            get
            {
                return _PaymentReceived;
            }
            set
            {
                if (value != _PaymentReceived)
                {
                    _PaymentReceived = value;
                    NotifyPropertyChanged("PaymentReceived");
                }
            }
        }



        private Decimal _ChargesReceived;
        public Decimal ChargesReceived
        {
            get
            {
                return _ChargesReceived;
            }
            set
            {
                if (value != _ChargesReceived)
                {
                    _ChargesReceived = value;
                    NotifyPropertyChanged("ChargesReceived");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
