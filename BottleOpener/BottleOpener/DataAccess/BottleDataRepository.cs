using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BottleOpener.Models;
using BottleOpener.Common;
using System.Threading;

namespace BottleOpener.DataAccess
{
    public class BottleDataEventArgs : EventArgs
    {
        public int _bottlevalue
        {
            get; set;
        }

        public BottleDataEventArgs(int data)
        {
            _bottlevalue = data;
        }
    }

    public class BottleDataRepository
    {
        #region Singleton
        private static BottleDataRepository _instance;
        #endregion

        public event EventHandler<BottleDataEventArgs> Updated;
        protected virtual void OnUpdated(BottleDataEventArgs e)
        {
            if (Updated != null)
            {
                Updated(this, e);
            }
        }

        #region Fields

        private List<BottleData> _bottleData;
        private BottleDevice _device;
        private StringBuilder _csvLogBuffer;
        bool _isThreadStarted = false;
        Thread _readThread;

        #endregion

        #region Constructor

        private BottleDataRepository()
        {
            _bottleData = new List<BottleData>();
            _csvLogBuffer = new StringBuilder();

            _readThread = new Thread(new ThreadStart(AddData));
            _readThread.Name = "BottleDataThread";
            _isThreadStarted = true;
            _readThread.Start();
        }

        public static BottleDataRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BottleDataRepository();
                }
                return _instance;
            }
        }

        #endregion

        #region Public Interface

        private void AddData()
        {
            while (_isThreadStarted)
            {
                while (_device != null && _device.Data.Count > 0)
                {
                    Console.WriteLine("Recording Data");
                    BottleData _latestData = new BottleData(_device.RetrieveData());
                    _bottleData.Add(_latestData);
                    OnUpdated(new BottleDataEventArgs(_latestData.AvgData));

                    //Write CSV string
                    var csvString = new StringBuilder();

                    foreach (int value in _latestData.RawData)
                    {
                        csvString.AppendFormat("{0},", value);
                    }

                    csvString.AppendFormat("{0}", _latestData.Time.ToShortTimeString());
                    _csvLogBuffer.AppendLine(csvString.ToString());

                    Thread.Sleep(1);
                }
            }
        }

        public List<BottleData> GetSessionData()
        {
            return new List<BottleData>(_bottleData);
        }

        public void ConnectBottle(string port)
        {
            try
            {
                _device = new BottleDevice(port, 28800);
                _device.Connect();
            } catch (Exception)
            {
                BottleLogger.Instance.Write("Error occurred while connecting to bottle.");
            }
        }

        public void StartDataCollection()
        {
            try
            {
                _device.StartData();
            }
            catch (Exception)
            {
                BottleLogger.Instance.Write("Bottle not connected. Please establish a connection.");
            }
        }
        public void StopDataCollection()
        {
            try
            {
                _device.StopData();

                //Dump to the log file if there was anything to write
                if (_csvLogBuffer.Length > 0)
                {
                    string filePath = System.IO.Directory.GetCurrentDirectory();
                    string fileName = string.Format(@"{0}\log{1}.csv", filePath, DateTime.Now.ToString("yyMdHHmm"));

                    System.IO.File.AppendAllText(fileName, _csvLogBuffer.ToString());
                }
            } catch (Exception)
            {
                BottleLogger.Instance.Write("Bottle not connected. Please establish a connection.");
            }
        }

        public void DisconnectBottle()
        {
            try
            {
                _device.Disconnect();
            }
            catch (Exception)
            {
                BottleLogger.Instance.Write("Bottle not connected. Please establish a connection.");
            }
        }
        #endregion     

    }
}
