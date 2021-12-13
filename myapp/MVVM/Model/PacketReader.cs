using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace myapp.MVVM.Model
{
     class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
      public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }
        public async Task<string> ReadMessage()
        {
            byte[] msgbuffer;
            var length =  ReadInt32();
            msgbuffer = new byte[length];
            await _ns.ReadAsync(msgbuffer, 0, length); 
            return Encoding.ASCII.GetString(msgbuffer);
        }
        
    }
}
