using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class TransactionListClass : INotifyPropertyChanged
    {
        private Int32 _TransactionCode;
        public Int32 TransactionCode
        {
            get
            {
                return _TransactionCode;
            }
            set
            {
                if (value != _TransactionCode)
                {
                    _TransactionCode = value;
                    NotifyPropertyChanged("TransactionCode");
                }
            }
        }


        private Int32 _TransYear;
        public Int32 TransYear
        {
            get
            {
                return _TransYear;
            }
            set
            {
                if (value != _TransYear)
                {
                    _TransYear = value;
                    NotifyPropertyChanged("TransYear");
                }
            }
        }


        private Int64 _CTLNo;
        public Int64 CTLNo
        {
            get
            {
                return _CTLNo;
            }
            set
            {
                if (value != _CTLNo)
                {
                    _CTLNo = value;
                    NotifyPropertyChanged("CTLNo");
                }
            }
        }



        private String _TransactionDate;
        public String TransactionDate
        {
            get
            {
                return _TransactionDate;
            }
            set
            {
                if (value != _TransactionDate)
                {
                    _TransactionDate = value;
                    NotifyPropertyChanged("TransactionDate");
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


        private Decimal _Amt;
        public Decimal Amt
        {
            get
            {
                return _Amt;
            }
            set
            {
                if (value != _Amt)
                {
                    _Amt = value;
                    NotifyPropertyChanged("Amt");
                }
            }
        }



        private Int32 _UPDTag;
        public Int32 UPDTag
        {
            get
            {
                return _UPDTag              ;
            }
            set
            {
                if (value != _UPDTag)
                {
                    _UPDTag = value;
                    NotifyPropertyChanged("UPDTag");
                }
            }
        }




        private String _ClientName;
        public String ClientName
        {
            get
            {
                return _ClientName;
            }
            set
            {
                if (value != _ClientName)
                {
                    _ClientName = value;
                    NotifyPropertyChanged("ClientName");
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
