using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.ClientMaster
{
    public class BillingHistoryClass : INotifyPropertyChanged
    {
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


        private Int32 _BillStatus;
        public Int32 BillStatus
        {
            get
            {
                return _BillStatus;
            }
            set
            {
                if (value != _BillStatus)
                {
                    _BillStatus = value;
                    NotifyPropertyChanged("BillStatus");
                }
            }
        }



        private String _BillStatusDescription;
        public String BillStatusDescription
        {
            get
            {
                return _BillStatusDescription;
            }
            set
            {
                if (value != _BillStatusDescription)
                {
                    _BillStatusDescription = value;
                    NotifyPropertyChanged("BillStatusDescription");
                }
            }
        }


        private Int32 _TR_CODE;
        public Int32 TR_CODE
        {
            get
            {
                return _TR_CODE;
            }
            set
            {
                if (value != _TR_CODE)
                {
                    _TR_CODE = value;
                    NotifyPropertyChanged("TR_CODE");
                }
            }
        }



        private String _TransactionDescription;
        public String TransactionDescription
        {
            get
            {
                return _TransactionDescription;
            }
            set
            {
                if (value != _TransactionDescription)
                {
                    _TransactionDescription = value;
                    NotifyPropertyChanged("TransactionDescription");
                }
            }
        }



        private String _TR_Date;
        public String TR_Date
        {
            get
            {
                return _TR_Date;
            }
            set
            {
                if (value != _TR_Date)
                {
                    _TR_Date = value;
                    NotifyPropertyChanged("TR_Date");
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


        private Int32 _CurrentReading;
        public Int32 CurrentReading
        {
            get
            {
                return _CurrentReading;
            }
            set
            {
                if (value != _CurrentReading)
                {
                    _CurrentReading = value;
                    NotifyPropertyChanged("CurrentReading");
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
