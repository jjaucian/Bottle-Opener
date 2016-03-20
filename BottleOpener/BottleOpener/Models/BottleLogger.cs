using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottleOpener.Models
{
    public class BottleLoggerEventArgs : EventArgs
    {
        public LogMessage message
        {
            get; set;
        }

        public BottleLoggerEventArgs(LogMessage data)
        {
            message = data;
        }
    }

    public class BottleLogger
    {
        private static BottleLogger _instance;
        private static List<LogMessage> _log = new List<LogMessage>();
        public event EventHandler<BottleLoggerEventArgs> Updated;

        private Object _lock = new Object();

        protected virtual void OnUpdated(BottleLoggerEventArgs e)
        {
            if (Updated != null)
            {
                Updated(this, e);
            }
        }

        /// <summary>
        /// A Bottle Logger singleton instance
        /// </summary>
        public static BottleLogger Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new BottleLogger();

                }
                return _instance;
            }
        }

        public void Write(string msg)
        {
            lock(_lock)
            {
                LogMessage newMessage = new LogMessage();
                newMessage.message = msg;
                newMessage.level = "1";
                newMessage.time = DateTime.Now.ToString();

                Console.WriteLine("{0}:[{1}]  {2}", newMessage.time, newMessage.level, newMessage.message);

                _log.Add(newMessage);

                OnUpdated(new BottleLoggerEventArgs(newMessage));
            }
        }

        public LogMessage GetLatest()
        {
            return _log.Last();
        }


    }


}
