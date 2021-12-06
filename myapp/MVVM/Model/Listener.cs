//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Sockets;
//using System.Net;
//using System.Diagnostics;
//using System.Windows.Input;


//namespace myapp.MVVM.Model
//{
//    class Listener
//    {
//        List<Client> _users;
//        TcpListener _listener = null;
//        public PacketReader PacketReader;

//        public event Action connectedEvent;

//        public async Task Create_Listener(string ip, int port)
//        {
//            _users = new List<Client>();
//            _listener = new TcpListener(IPAddress.Parse(ip), port);
//            _listener.Start();
            

//            while(true)
//            {

//                using (var client = await _listener.AcceptTcpClientAsync())
//                {
//                    //adds first Client, which is ourselves to the server
//                    Client ourClient = new Client(client);    
//                    _users.Add(ourClient);

//                    BroadcastConnection();
//                }
                
                
//            }
//            //_users.Add(client);
//            /*BROADCAST CONNECTION*/

//        }

//        private void BroadcastConnection()
//        {
//            foreach (var user in _users)
//            {
//                foreach (var usr in _users)
//                {
//                    Debug.WriteLine(usr.Username);
//                    var broadcastPacket = new PacketBuilder();
//                    broadcastPacket.WriteOpCode(1);
//                    broadcastPacket.WriteMessage(usr.Username);
//                    //user.Clientsocket.Send(broadcastPacket.GetPacketBytes());

//                }
//            }
//        }


//        private void ReadPackets()
//        {
//            Task.Run(() =>
//                {
//                    while (true)
//                    {
//                        var opcode = PacketReader.ReadByte();
//                        switch (opcode)
//                        {
//                            case 1:
//                                connectedEvent?.Invoke();
//                                break;
//                            default:
//                                Debug.WriteLine("XIAXIASASDASD");
//                                break;
//                        }
//                    }
//                }
//            );
//        }
//    }
//}
