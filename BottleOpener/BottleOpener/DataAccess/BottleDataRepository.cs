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
    public class BottleDataRepository
    {
        #region Singleton
        private static BottleDataRepository _instance;
        #endregion

        #region Fields

        private List<BottleData> _bottleData;
        private BottleDevice _device;

        bool _isThreadStarted = false;
        Thread _readThread;

        #endregion

        #region Constructor

        private BottleDataRepository()
        {
            _device = new BottleDevice("COM3", 19200);
            _bottleData = new List<BottleData>();

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
                    _bottleData.Add(new BottleData(_device.RetrieveData()));
                    Thread.Sleep(1);
                }
            }
        }

        public List<BottleData> GetSessionData()
        {
            return new List<BottleData>(_bottleData);
        }

        public void ConnectBottle()
        {
            _device.Connect();
        }

        public void StartDataCollection()
        {
            _device.StartData();
        }

        public void StopDataCollection()
        {
            _device.StopData();
        }

        public void DisconnectBottle()
        {
            _device.Disconnect();
        }
        #endregion


    }
}
