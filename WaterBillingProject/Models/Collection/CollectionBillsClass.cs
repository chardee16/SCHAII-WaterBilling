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
        private Int32 _SLC_CODE;
        public Int32 SLC_CODE
        {
            get
            {
                return _SLC_CODE;
            }
            set
            {
                if (value != _SLC_CODE)
                {
                    _SLC_CODE = value;
                    NotifyPropertyChanged("SLC_CODE");
                }
            }
        }


        private Int32 _SLT_CODE;
        public Int32 SLT_CODE
        {
            get
            {
                return _SLT_CODE;
            }
            set
            {
                if (value != _SLT_CODE)
                {
                    _SLT_CODE = value;
                    NotifyPropertyChanged("SLT_CODE");
                }
            }
        }


        private String _BillMonth;
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

        private String _ReferenceNo;
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


        private String _Consumption;
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



        private Decimal _CurrentDue;
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


        private String _SL_Description;
        public String SL_Description
        {
            get
            {
                return _SL_Description;
            }
            set
            {
                if (value != _SL_Description)
                {
                    _SL_Description = value;
                    NotifyPropertyChanged("SL_Description");
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
