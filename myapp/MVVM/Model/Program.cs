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
                    if (accept)
                    {
                        BroadcastConnection(client.Username);
                        BroadcastMessage($"{client.Username} connected");
                    }
                    else
                    {
                        BroadcastMessage($"{client.Username} was rejected!");
                        BroadcastRejection(client.Username);
                    
                    
                }

                }
            
          
        }

        public void BroadcastConnection(string name)
        {
            foreach(var user in _users)
            {
                
                    foreach (var usr in _users)
                    {

                        var broadcastpacket = new PacketBuilder();
                        broadcastpacket.WriteOpCode(1);
                        broadcastpacket.WriteMessage(usr.Username);
                        broadcastpacket.WriteMessage(usr.UID.ToString());
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

        public static void BroadcastDisconnect(string uid)
        {
            var disconnect = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
               
 
                _users.Remove(disconnect);
          
                foreach (var user in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(10);
                    broadcastPacket.WriteMessage(uid);
                    user.Clientsocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
           
                BroadcastMessage($"{disconnect.Username} disconnected!");
            


        }

        public static void BroadcastRejection(string username)
        {
            var disconnect = _users.Where(x => x.Username == username).FirstOrDefault();
            _users.Remove(disconnect);
            var broadcastPacket = new PacketBuilder();
            broadcastPacket.WriteOpCode(20);
            disconnect.Clientsocket.Client.Send(broadcastPacket.GetPacketBytes());
        }

        public static void BroadcastBuzz(string uid)
        {
            var name = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            if(name != null)
            {
                BroadcastMessage($"{name.Username} sent a buzz!");
            }
            foreach (var user in _users)
            {
                if (user.UID.ToString() != uid)
                {
                    var msgPacket = new PacketBuilder();
                    msgPacket.WriteOpCode(15);
                    user.Clientsocket.Client.Send(msgPacket.GetPacketBytes());
                }
            }
        }
            
    }
}
