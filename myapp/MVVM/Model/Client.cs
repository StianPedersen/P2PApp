using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.IO;

namespace myapp.MVVM.Model
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient Clientsocket { get; set; }

        PacketReader _packetReader;
        PacketBuilder _packetBuilder;
        public Client(TcpClient client)
        {
            Clientsocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(Clientsocket.GetStream());        
            Task.Run(() => Process());
        }
        public async void Process()
        {
            var opcode = _packetReader.ReadByte();
            Username = await _packetReader.ReadMessage();
            while (true)
            {
                try
                {
                    opcode =  _packetReader.ReadByte();

                    switch (opcode)
                    {
                        case 2:
                            break;
                        case 5:
                            var msg = await _packetReader.ReadMessage();
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        case 15:
                            Program.BroadcastBuzz( UID.ToString());
                            break;
                        default:
                            break;
                    }
                }

                catch (Exception ex)
                {   
                    Program.BroadcastDisconnect(UID.ToString());
                    Clientsocket.Close();
                    Debug.WriteLine(ex.ToString());         
                    break;
                }
            }
        }
      
    }

}
