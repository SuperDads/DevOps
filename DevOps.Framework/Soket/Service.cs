using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Framework.Soket
{
    public abstract class Service : IService
    {
        public abstract bool Handle(byte[] firstPacket, int length, Socket socket, object state);

        public virtual void Stop() { }
    }
}
