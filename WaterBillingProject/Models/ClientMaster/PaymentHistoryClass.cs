using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.ClientMaster
{
    public class PaymentHistoryClass : INotifyPropertyChanged
    {
        private String _ORNo;
        public String ORNo
        {
            get
            {
                return _ORNo;
            }
            set
            {
                if (value != _ORNo)
                {
                    _ORNo = value;
                    NotifyPropertyChanged("ORNo");
                }
            }
        }



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



        private Decimal _WaterBill;
        public Decimal WaterBill
        {
            get
            {
                return _WaterBill;
            }
            set
            {
                if (value != _WaterBill)
                {
                    _WaterBill = value;
                    NotifyPropertyChanged("WaterBill");
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




        private Decimal _MonthlyDues;
        public Decimal MonthlyDues
        {
            get
            {
                return _MonthlyDues;
            }
            set
            {
                if (value != _MonthlyDues)
                {
                    _MonthlyDues = value;
                    NotifyPropertyChanged("MonthlyDues");
                }
            }
        }



        private Decimal _GarbageCollection;
        public Decimal GarbageCollection
        {
            get
            {
                return _GarbageCollection;
            }
            set
            {
                if (value != _GarbageCollection)
                {
                    _GarbageCollection = value;
                    NotifyPropertyChanged("GarbageCollection");
                }
            }
        }


        private Decimal _Surcharge;
        public Decimal Surcharge
        {
            get
            {
                return _Surcharge;
            }
            set
            {
                if (value != _Surcharge)
                {
                    _Surcharge = value;
                    NotifyPropertyChanged("Surcharge");
                }
            }
        }


        private Decimal _CashReceived;
        public Decimal CashReceived
        {
            get
            {
                return _CashReceived;
            }
            set
            {
                if (value != _CashReceived)
                {
                    _CashReceived = value;
                    NotifyPropertyChanged("CashReceived");
                }
            }
        }



        private Int32 _UserID;
        public Int32 UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                if (value != _UserID)
                {
                    _UserID = value;
                    NotifyPropertyChanged("UserID");
                }
            }
        }


        private String _UserName;
        public String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                if (value != _UserName)
                {
                    _UserName = value;
                    NotifyPropertyChanged("UserName");
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
