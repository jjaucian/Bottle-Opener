﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using BottleOpener.Models;


namespace BottleOpener.Common
{
    public class BottleDevice
    {
        private SerialPort _socket;
        private BottleCommunicationProtocol _comm;
        private Queue<List<int>> _data;

        bool _isThreadStarted = false;
        Thread _readThread;
        

        public BottleDevice(string portname, int baudrate)
        {
            _socket = new SerialPort();
            _comm = new BottleCommunicationProtocol();          

            _socket.PortName = portname;
            _socket.BaudRate = baudrate;
            _socket.DataBits = 8;
            _socket.Parity = Parity.None;
            _socket.StopBits = StopBits.One;

            _readThread = new Thread(new ThreadStart(Read));
            _readThread.Name = "BottleReadThread";
            _readThread.IsBackground = true;


            _data = new Queue<List<int>>();
        }    

        public void Connect()
        {
            try
            {
                if (!_socket.IsOpen)
                {
                    _socket.Open();
                    _isThreadStarted = true;
                    _readThread.Start();
                    _socket.Write("c100");
                }
            } catch (Exception)
            {
                BottleLogger.Instance.Write("Unable to connect to bottle.");
            }

        }

        public void StartData()
        {
            SendMessage("s100");
        }

        public void StopData()
        {
            SendMessage("t100");
        }

        public void Disconnect()
        {
            SendMessage("d100");
            try
            {
                _isThreadStarted = false;
                _readThread.Join();
                _socket.Close();
            }
            catch (Exception)
            {
                BottleLogger.Instance.Write("Error occurred while disconnecting");
            }
        }

        private void SendMessage(string msg)
        {
            if (_socket.IsOpen)
            {
                try
                {
                    _socket.Write(msg);
                }
                catch (Exception)
                {
                    BottleLogger.Instance.Write("Unable to send " + msg + "command to bottle.");
                }
            }
            else
            {
                BottleLogger.Instance.Write("Bottle is not connected.");
            }
        }
        
        private void Read()
        {
            while (_isThreadStarted)
            {
                try
                {
                    if (_socket.IsOpen)
                    {

                        string message = _socket.ReadLine();
                        if (message != "")
                        {
                            //strip the newline
                            message = message.Replace("\r", "");
                            EnqueueData(message);
                            BottleLogger.Instance.Write(message);
                        }
                    }
                }
                catch (Exception)
                {
                    BottleLogger.Instance.Write("Bottle Read Thread Crashed.");
                }
            }

        }

        public Queue<List<int>> Data
        {
            get { return _data; }            
        }

        private void EnqueueData(string data)
        {
            char[] delimiters = { 'x' };

            List<string> _sData = data.Split(delimiters).ToList();

            List<int> _iData = new List<int>();

            foreach (string item in _sData)
            {
                int datavalue;
                bool parsed = Int32.TryParse(item, out datavalue);
                if (parsed)
                {
                    _iData.Add(datavalue);
                }
            }

            if (_iData.Count > 0)
            {
                _data.Enqueue(_iData);
            }
        }

        public List<int> RetrieveData()
        {
            return _data.Dequeue();
        }

        public void Dispose()
        {
            _isThreadStarted = false;
            Disconnect();
            GC.SuppressFinalize(this);
        }
    }
}
