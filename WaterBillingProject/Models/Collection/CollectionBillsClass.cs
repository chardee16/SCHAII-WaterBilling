using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class CollectionBillsClass : INotifyPropertyChanged
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


        public String _Consumption;
        public String Consumption
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



        public Decimal _CurrentDue;
        public Decimal CurrentDue
        {
            get
            {
                return _CurrentDue;
            }
            set
            {
                if (value != _CurrentDue)
                {
                    _CurrentDue = value;
                    NotifyPropertyChanged("CurrentDue");
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
