using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BottleOpener.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BottleOpener.ViewModels
{
    public class LogMessageViewModel : INotifyPropertyChanged
    {
        BottleLogger _log = BottleLogger.Instance;
        ObservableCollection<LogMessage> _messages;

        object _key = new object();

        public LogMessageViewModel()
        {
            _log.Updated += UpdateMessages;
            _messages = new ObservableCollection<LogMessage>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<LogMessage> Messages
        {
            get { return _messages; }
            set { _messages = value;}
        }

        private void UpdateMessages(object sender, BottleLoggerEventArgs e)
        {
            lock(_key)
            {
                //TODO change to BGW async later to fix crash
                App.Current.Dispatcher.InvokeAsync((Action)(() =>
                {
                    _messages.Add(e.message);
                }));
            }
        }

        public void Dispose()
        {
            _log.Updated -= UpdateMessages;
            GC.SuppressFinalize(this);
        }
    }
}
