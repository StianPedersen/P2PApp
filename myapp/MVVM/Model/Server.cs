using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace myapp.MVVM.Model
{
     class Server
   {

        TcpClient _client;
        PacketBuilder _packetBuilder;
        public PacketReader PacketReader;

        

        public event Action connectedEvent;
        public event Action msgRecievedEvent;
        public event Action disconnectEvent;
        public event Action noListenerEvent;
        public event Action ShakeScreenEvent;
        public Server()
       {
            _client = new TcpClient();
       }
       public void ConnectToServer(string ip, int port, string username)
       {
            if (!_client.Connected)
            {
                try
                {
                    _client.Connect(ip, port);
                    PacketReader = new PacketReader(_client.GetStream());
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                    ReadPackets();
                }
                catch
                {
                    noListenerEvent?.Invoke();
                }
               

           }
       }
        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;

                        case 5:
                            msgRecievedEvent?.Invoke();
                            break;

                        case 10:
                            disconnectEvent?.Invoke();
                            break;
                        case 15:
                            //Debug.WriteLine("kom hit");
                            ShakeScreenEvent?.Invoke();
                            break;
                        

                        default:
                            Debug.WriteLine("XIAXIASASDASD");
                            break;
                    }
                }
            }
            );
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }

        public void SendBuzz()
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(15);
            messagePacket.WriteMessage("Buzz sent");
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}