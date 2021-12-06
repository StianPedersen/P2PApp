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
        public TcpClient Clientsocket { get; set; }

        PacketReader _packetReader;
        PacketBuilder _packetBuilder;
        public Client(TcpClient client)
        {
   
            Clientsocket = client;
            _packetReader = new PacketReader(Clientsocket.GetStream());
            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();            
            Debug.WriteLine($"Client: {Username} {opcode.ToString()}");
            Task.Run(() => Process());
        }

        void Process()
        {
            while(true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    
                    switch (opcode)
                    {
                        case 2:
                            //send to MAINWINDOW A DECLINE OR ACCEPT
                            Debug.WriteLine("Hej du kom till 2");
                            
                            break;
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Debug.WriteLine($"Msgrecieved: {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        case 15:
                            Debug.WriteLine("Buzz sent");
                            Program.BroadcastBuzz( $"{Username}");
                            break;
                        default:
                            break;
                    }
                }

                catch (Exception ex)
                {
                    if (ex is IOException)
                    {
                        Program.BroadcastDisconnect(Username);
                    }
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine($"{Username} Disconnected!");              
                    break;
                }
            }
        }
      
    }

}
