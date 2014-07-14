using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BionicLab10.Model;

namespace BionicLab10.ViewModel
{
    class FileViewModel: INotifyPropertyChanged
    {
        public MyFile MyFile;

        public FileViewModel(MyFile myFile)
        {
            this.MyFile = myFile;
        }

        public string FileName
        {
            get
            { return MyFile.FileName; }
            set
            {
                MyFile.FileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public DateTime FileDate1
        {
            get
            { return MyFile.FileDate1; }
            set
            { 
                MyFile.FileDate1 = value;
                OnPropertyChanged("FileDate1");
            }
        }

        public DateTime FileDate2
        {
            get
            { return MyFile.FileDate2; }
            set
            { 
                MyFile.FileDate2 = value;
                OnPropertyChanged("FileDate2");
            }
        }

        public string Status
        {
            get
            { return MyFile.Status; }
            set
            {
                MyFile.Status = value;
                OnPropertyChanged("Status");
            }
        }

        public bool Sync
        {
            get
            { return MyFile.Sync; }
            set
            {
                MyFile.Sync = value;
                OnPropertyChanged("Sync");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
