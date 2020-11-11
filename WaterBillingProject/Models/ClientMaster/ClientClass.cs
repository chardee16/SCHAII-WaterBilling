using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WaterBilling.Models.ClientMaster
{
    public class ClientClass : INotifyPropertyChanged
    {
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



        private Int32 _ClientAccountStatusID;
        public Int32 ClientAccountStatusID
        {
            get
            {
                return _ClientAccountStatusID;
            }
            set
            {
                if (value != _ClientAccountStatusID)
                {
                    _ClientAccountStatusID = value;
                    NotifyPropertyChanged("ClientAccountStatusID");
                }
            }
        }



        private Int32 _BlockNo;
        public Int32 BlockNo
        {
            get
            {
                return _BlockNo;
            }
            set
            {
                if (value != _BlockNo)
                {
                    _BlockNo = value;
                    NotifyPropertyChanged("BlockNo");
                }
            }
        }


        private Int32 _LotNo;
        public Int32 LotNo
        {
            get
            {
                return _LotNo;
            }
            set
            {
                if (value != _LotNo)
                {
                    _LotNo = value;
                    NotifyPropertyChanged("LotNo");
                }
            }
        }



        private Int32 _Occupants;
        public Int32 Occupants
        {
            get
            {
                return _Occupants;
            }
            set
            {
                if (value != _Occupants)
                {
                    _Occupants = value;
                    NotifyPropertyChanged("Occupants");
                }
            }
        }


        private Boolean _IsSenior;
        public Boolean IsSenior
        {
            get
            {
                return _IsSenior;
            }
            set
            {
                if (value != _IsSenior)
                {
                    _IsSenior = value;
                    NotifyPropertyChanged("IsSenior");
                }
            }
        }



        private Int32 _SeniorCount;
        public Int32 SeniorCount
        {
            get
            {
                return _SeniorCount;
            }
            set
            {
                if (value != _SeniorCount)
                {
                    _SeniorCount = value;
                    NotifyPropertyChanged("SeniorCount");
                }
            }
        }


        private String _FullName;
        public String FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                if (value != _FullName)
                {
                    _FullName = value;
                    NotifyPropertyChanged("FullName");
                }
            }
        }


        private String _FullAddress;
        public String FullAddress
        {
            get
            {
                return _FullAddress;
            }
            set
            {
                if (value != _FullAddress)
                {
                    _FullAddress = value;
                    NotifyPropertyChanged("FullAddress");
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



        private Int64 _PreviousReading;
        public Int64 PreviousReading
        {
            get
            {
                return _PreviousReading;
            }
            set
            {
                if (value != _PreviousReading)
                {
                    _PreviousReading = value;
                    NotifyPropertyChanged("PreviousReading");
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
