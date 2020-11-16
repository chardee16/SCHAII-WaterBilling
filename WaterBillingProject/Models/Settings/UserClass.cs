using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterBillingProject.Models.Settings
{
    public class UserClass : INotifyPropertyChanged
    {
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


        private String _Username;
        public String Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (value != _Username)
                {
                    _Username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }


        private String _Password;
        public String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                if (value != _Password)
                {
                    _Password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }



        private String _FirstName;
        public String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (value != _FirstName)
                {
                    _FirstName = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }



        private String _MiddleName;
        public String MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (value != _MiddleName)
                {
                    _MiddleName = value;
                    NotifyPropertyChanged("MiddleName");
                }
            }
        }


        private String _LastName;
        public String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                if (value != _LastName)
                {
                    _LastName = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }

        private String _IsAdministrator;
        public String IsAdministrator
        {
            get
            {
                return _IsAdministrator;
            }
            set
            {
                if (value != _IsAdministrator)
                {
                    _IsAdministrator = value;
                    NotifyPropertyChanged("IsAdministrator");
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
