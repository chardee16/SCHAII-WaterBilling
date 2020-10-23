using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WaterBilling.Models.Billing
{
    public class MonthsClass : INotifyPropertyChanged
    {
        private Int32 _MonthID;
        public Int32 MonthID
        {
            get
            {
                return _MonthID;
            }
            set
            {
                if (value != _MonthID)
                {
                    _MonthID = value;
                    NotifyPropertyChanged("MonthID");
                }
            }
        }


        private String _MonthDescription;
        public String MonthDescription
        {
            get
            {
                return _MonthDescription;
            }
            set
            {
                if (value != _MonthDescription)
                {
                    _MonthDescription = value;
                    NotifyPropertyChanged("MonthDescription");
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
