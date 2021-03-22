using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Reports
{
    public class BillingReportClass : INotifyPropertyChanged
    {

        private Int32 _BlockNo;
        public Int32 BlockNo
        {
            get
            {
                return _BlockNo;
            }
            set
            {
                if (value != _BlockNo)
                {
                    _BlockNo = value;
                    NotifyPropertyChanged("BlockNo");
                }
            }
        }



        private Int32 _LotNo;
        public Int32 LotNo
        {
            get
            {
                return _LotNo;
            }
            set
            {
                if (value != _LotNo)
                {
                    _LotNo = value;
                    NotifyPropertyChanged("LotNo");
                }
            }
        }






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



        private Int64 _PresentReading;
        public Int64 PresentReading
        {
            get
            {
                return _PresentReading;
            }
            set
            {
                if (value != _PresentReading)
                {
                    _PresentReading = value;
                    NotifyPropertyChanged("PresentReading");
                }
            }
        }



        private Int64 _PreviousReading;
        public Int64 PreviousReading
        {
            get
            {
                return _PreviousReading;
            }
            set
            {
                if (value != _PreviousReading)
                {
                    _PreviousReading = value;
                    NotifyPropertyChanged("PreviousReading");
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


        private Int32 _Minimum;
        public Int32 Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (value != _Minimum)
                {
                    _Minimum = value;
                    NotifyPropertyChanged("Minimum");
                }
            }
        }


        private Int32 _Excess;
        public Int32 Excess
        {
            get
            {
                return _Excess;
            }
            set
            {
                if (value != _Excess)
                {
                    _Excess = value;
                    NotifyPropertyChanged("Excess");
                }
            }
        }



        private Decimal _AmountDue;
        public Decimal AmountDue
        {
            get
            {
                return _AmountDue;
            }
            set
            {
                if (value != _AmountDue)
                {
                    _AmountDue = value;
                    NotifyPropertyChanged("AmountDue");
                }
            }
        }



        private Decimal _Discount;
        public Decimal Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                if (value != _Discount)
                {
                    _Discount = value;
                    NotifyPropertyChanged("Discount");
                }
            }
        }


        private Decimal _NetDue;
        public Decimal NetDue
        {
            get
            {
                return _NetDue;
            }
            set
            {
                if (value != _NetDue)
                {
                    _NetDue = value;
                    NotifyPropertyChanged("NetDue");
                }
            }
        }



        private Decimal _MonthlyDue;
        public Decimal MonthlyDue
        {
            get
            {
                return _MonthlyDue;
            }
            set
            {
                if (value != _MonthlyDue)
                {
                    _MonthlyDue = value;
                    NotifyPropertyChanged("MonthlyDue");
                }
            }
        }



        private Decimal _GarbageDue;
        public Decimal GarbageDue
        {
            get
            {
                return _GarbageDue;
            }
            set
            {
                if (value != _GarbageDue)
                {
                    _GarbageDue = value;
                    NotifyPropertyChanged("GarbageDue");
                }
            }
        }


        private Decimal _PreviousBalance;
        public Decimal PreviousBalance
        {
            get
            {
                return _PreviousBalance;
            }
            set
            {
                if (value != _PreviousBalance)
                {
                    _PreviousBalance = value;
                    NotifyPropertyChanged("PreviousBalance");
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
