using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WaterBilling.Models.Billing
{
    public class PreviousBillClass : INotifyPropertyChanged
    {
        public String _BillMonth;
        public String BillMonth
        {
            get
            {
                return _BillMonth;
            }
            set
            {
                if (value != _BillMonth)
                {
                    _BillMonth = value;
                    NotifyPropertyChanged("BillMonth");
                }
            }
        }

        public String _ReferenceNo;
        public String ReferenceNo
        {
            get
            {
                return _ReferenceNo;
            }
            set
            {
                if (value != _ReferenceNo)
                {
                    _ReferenceNo = value;
                    NotifyPropertyChanged("ReferenceNo");
                }
            }
        }


        public Decimal _TotalDue;
        public Decimal TotalDue
        {
            get
            {
                return _TotalDue;
            }
            set
            {
                if (value != _TotalDue)
                {
                    _TotalDue = value;
                    NotifyPropertyChanged("TotalDue");
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
