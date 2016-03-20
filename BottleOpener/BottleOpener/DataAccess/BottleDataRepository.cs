using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BottleOpener.Models;
using BottleOpener.Common;

namespace BottleOpener.DataAccess
{
    public class BottleDataRepository
    {
        #region Fields

        private List<BottleData> _bottleData;
        private BottleDevice _device;

        #endregion

        #region Constructor

        public BottleDataRepository()
        {
            _device = new BottleDevice("COM3", 19200);
            _bottleData = new List<BottleData>();
        }

        #endregion

        #region Public Interface

        public void AddData()
        {

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
