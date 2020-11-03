using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Reports
{
    public class ReportClientListClass : INotifyPropertyChanged
    {

        private Int64 _ClientID;
        public Int64 ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                if (value != _ClientID)
                {
                    _ClientID = value;
                    NotifyPropertyChanged("ClientID");
                }
            }
        }





        private String _FullName;
        public String FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                if (value != _FullName)
                {
                    _FullName = value;
                    NotifyPropertyChanged("FullName");
                }
            }
        }


        private String _FullAddress;
        public String FullAddress
        {
            get
            {
                return _FullAddress;
            }
            set
            {
                if (value != _FullAddress)
                {
                    _FullAddress = value;
                    NotifyPropertyChanged("FullAddress");
                }
            }
        }



        private Int32 _Consumption;
        public Int32 Consumption
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


        private Decimal _TotalDue;
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


        private Int32 _TotalUnpaidBill;
        public Int32 TotalUnpaidBill
        {
            get
            {
                return _TotalUnpaidBill;
            }
            set
            {
                if (value != _TotalUnpaidBill)
                {
                    _TotalUnpaidBill = value;
                    NotifyPropertyChanged("TotalUnpaidBill");
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
