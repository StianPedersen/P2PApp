using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace myapp.MVVM.Model
{
    class Program
    {
         static List<Client> _users;
         TcpListener _listener;
        public event Func<bool> acceptEvent;
        public bool accept;
        
        public void Function( string ip, int port)
        {

            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse(ip), port);
            
                _listener.Start();

                while (true)
                {
                    var client = new Client(_listener.AcceptTcpClient());

                    _users.Add(client);
                    accept = (bool)acceptEvent?.Invoke();
                    Debug.WriteLine(accept);
                    if (accept)
                    {
                        BroadcastConnection();
                        BroadcastMessage($"{client.Username} Connected");
                    }
                    else
                    {
                    BroadcastMessage($"{client.Username} was rejected!");
                    BroadcastDisconnect(client.Username);

                    }

                }
            
          
        }

        public void BroadcastConnection()
        {
            foreach(var user in _users)
            {
                foreach(var usr in _users)
                {
                    var broadcastpacket = new PacketBuilder();
                    broadcastpacket.WriteOpCode(1);
                    broadcastpacket.WriteMessage(usr.Username);
                    user.Clientsocket.Client.Send(broadcastpacket.GetPacketBytes());
                }
            }
        }

       


        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                
                    var msgPacket = new PacketBuilder();
                    msgPacket.WriteOpCode(5);
                    msgPacket.WriteMessage(message);
                    user.Clientsocket.Client.Send(msgPacket.GetPacketBytes());
                
            }
        }

        public static void BroadcastDisconnect(string username)
        {
            var disconnect = _users.Where(x => x.Username == username).FirstOrDefault();
            _users.Remove(disconnect);
            
                foreach (var user in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(10);
                    broadcastPacket.WriteMessage(username);
                    user.Clientsocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
                BroadcastMessage($"{disconnect.Username} Disconnected!");
        }
        public static void BroadcastBuzz(string name)
        {
            BroadcastMessage($"{name} sent a buzz");
            foreach (var user in _users)
            {
                if (user.Username != name)
                {
                    var msgPacket = new PacketBuilder();
                    msgPacket.WriteOpCode(15);
                    user.Clientsocket.Client.Send(msgPacket.GetPacketBytes());
                }
            }
        }
            
    }
}
