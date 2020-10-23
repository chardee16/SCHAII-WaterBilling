using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WaterBilling.Models.Billing
{
    public class ChargesClass : INotifyPropertyChanged
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


        private String _Formula;
        public String Formula
        {
            get
            {
                return _Formula;
            }
            set
            {
                if (value != _Formula)
                {
                    _Formula = value;
                    NotifyPropertyChanged("Formula");
                }
            }
        }

        private Int64 _AccountCode;
        public Int64 AccountCode
        {
            get
            {
                return _AccountCode;
            }
            set
            {
                if (value != _AccountCode)
                {
                    _AccountCode = value;
                    NotifyPropertyChanged("AccountCode");
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
