using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Collection
{
    public class CollectionDiscountClass : INotifyPropertyChanged
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

        private Int32 _SLE_CODE;
        public Int32 SLE_CODE
        {
            get
            {
                return _SLE_CODE;
            }
            set
            {
                if (value != _SLE_CODE)
                {
                    _SLE_CODE = value;
                    NotifyPropertyChanged("SLE_CODE");
                }
            }
        }

        private Int32 _StatusID;
        public Int32 StatusID
        {
            get
            {
                return _StatusID;
            }
            set
            {
                if (value != _StatusID)
                {
                    _StatusID = value;
                    NotifyPropertyChanged("StatusID");
                }
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }


        private Int64 _COAID;
        public Int64 COAID
        {
            get
            {
                return _COAID;
            }
            set
            {
                if (value != _COAID)
                {
                    _COAID = value;
                    NotifyPropertyChanged("COAID");
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


        private Decimal _Amount;
        public Decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                if (value != _Amount)
                {
                    _Amount = value;
                    NotifyPropertyChanged("Amount");
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
