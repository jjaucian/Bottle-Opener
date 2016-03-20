using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BottleOpener.DataAccess;

namespace BottleOpener.Models
{
    public class LogMessage
    {
        private string _message;
        private string _level;
        private string _time;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string message
        {
            get { return _message; }
            set
            {
                _message = value; NotifyPropertyChanged("message");
            }
        }

        public string level
        {
            get { return _level; }
            set
            {
                _level = value; NotifyPropertyChanged("level");
            }
        }

        public string time
        {
            get { return _time; }
            set { _time = value; NotifyPropertyChanged("time"); }
        }
    }
}
