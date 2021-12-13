using System;
using System.IO;
using System.Text;


namespace myapp.MVVM.Model
{
    class PacketBuilder
        {
             MemoryStream _ms;
            public PacketBuilder()
            {
                _ms = new MemoryStream();
            }
            public void WriteOpCode(byte opcode)
            {
                _ms.WriteByte(opcode);
                
            }
            public async void WriteMessage(string message)
            {
                var messageLength = message.Length;
                await _ms.WriteAsync(BitConverter.GetBytes(messageLength));
                await _ms.WriteAsync(Encoding.ASCII.GetBytes(message));
            }

            public byte[] GetPacketBytes()
            {
                return _ms.ToArray();   
            }
        }
}
