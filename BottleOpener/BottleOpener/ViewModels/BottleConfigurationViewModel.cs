using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BottleOpener.Models;
using BottleOpener.Common;
using BottleOpener.DataAccess;

namespace BottleOpener.ViewModels
{
    public class BottleConfigurationViewModel
    {

        private BottleDataRepository _bottle;
        private List<string> _commPort;

        public BottleConfigurationViewModel()
        {
            _bottle = new BottleDataRepository();
            

        }

        private ICommand _connectBottleCommand;
        private ICommand _startSessionCommand;
        private ICommand _stopSessionCommand;
        private ICommand _disconnectBottleCommand;


        public ICommand ConnectBottleCommand
        {
            get
            {
                if (_connectBottleCommand == null)
                {
                    _connectBottleCommand = new RelayCommand(
                        param => {
                            BottleLogger.Instance.Write("Connecting to bottle.");
                            _bottle.ConnectBottle();
                            
                        }
                        );
                }
                return _connectBottleCommand;
            }            
        }
        public ICommand StartSessionCommand
        {
            get
            {
                if (_startSessionCommand == null)
                {
                    _startSessionCommand = new RelayCommand(
                        param => {
                            BottleLogger.Instance.Write("Starting bottle session.");
                            _bottle.StartDataCollection();
                        }
                        );
                }
                return _startSessionCommand;
            }

        }
        public ICommand StopSessionCommand
        {
            get
            {
                if (_stopSessionCommand == null)
                {
                    _stopSessionCommand = new RelayCommand(
                        param => {
                            BottleLogger.Instance.Write("Stopping bottle session.");
                            _bottle.StopDataCollection();
                             }
                        );
                }
                return _stopSessionCommand;
            }

        }
        public ICommand DisconnectBottleCommand
        {
            get
            {
                if (_disconnectBottleCommand == null)
                {
                    _disconnectBottleCommand = new RelayCommand(
                        param => {
                            BottleLogger.Instance.Write("Disconnecting from bottle.");
                            _bottle.DisconnectBottle();

                        }
                        );
                }
                return _disconnectBottleCommand;
            }
        }
    }
}
