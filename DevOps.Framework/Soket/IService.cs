using System.Net.Sockets;

namespace DevOps.Framework.Soket
{
    public interface IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstPacket">第一个包</param>
        /// <param name="length">长度</param>
        /// <param name="socket">Socket对象</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        bool Handle(byte[] firstPacket, int length, Socket socket, object state);

        void Stop();
    }
}
