using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Dashboard
{
    public class DashboardMonthlyConsumption : INotifyPropertyChanged
    {
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
