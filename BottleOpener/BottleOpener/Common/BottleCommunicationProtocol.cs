using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottleOpener.Common
{
    public class BottleCommunicationProtocol
    {
        public struct BottleCommunicationMessage
        {
            public int ID;

            public BottleCommunicationMessage(int id)
            {
                ID = id;
            }
        };

        List<BottleCommunicationMessage> _messages;
        
        public BottleCommunicationProtocol()
        {
            _messages = new List<BottleCommunicationMessage>();

            _messages.Add(new BottleCommunicationMessage(100)); //Connect
            _messages.Add(new BottleCommunicationMessage(200)); //Disconnect

        } 

        public BottleCommunicationMessage Connect()
        {
            return new BottleCommunicationMessage(100);

        }

        
    }
}
