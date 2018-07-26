using System.Net;
using System.Net.Sockets;

namespace DevOps.Framework.Soket
{
    public class UDPState
    {
        public Socket socket;
        public byte[] buffer = new byte[4096];
        public EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }
}
