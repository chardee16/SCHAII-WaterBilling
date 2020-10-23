using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WaterBilling.Models.ClientMaster
{
    public class ClientStatusClass : INotifyPropertyChanged
    {
        private Int32 _ClientStatusID;
        public Int32 ClientStatusID
        {
            get
            {
                return _ClientStatusID;
            }
            set
            {
                if (value != _ClientStatusID)
                {
                    _ClientStatusID = value;
                    NotifyPropertyChanged("ClientStatusID");
                }
            }
        }


        private String _ClientStatusDesc;
        public String ClientStatusDesc
        {
            get
            {
                return _ClientStatusDesc;
            }
            set
            {
                if (value != _ClientStatusDesc)
                {
                    _ClientStatusDesc = value;
                    NotifyPropertyChanged("ClientStatusDesc");
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
