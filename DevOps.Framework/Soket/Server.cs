using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Framework.Soket
{
    public class Server

    {
        /// <summary>
        /// 默认服务器超时时间（秒）
        /// </summary>
        private const int DefaultServerTimeoutSec = 5;

        /// <summary>
        /// 服务器最大超时时间（秒）
        /// </summary>
        public const int MaxServerTimeoutSec = 20;

        /// <summary>
        /// 服务器默认端口
        /// </summary>
        public const int DefaultServerPort = 8788;

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string server;
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int server_port;
        /// <summary>
        /// 密码
        /// </summary>
        public string password;
        /// <summary>
        /// 加密方式
        /// </summary>
        public string method;
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int timeout;

        public Server()
        {
            server = "";
            server_port = DefaultServerPort;
            method = "aes-256-cfb";
            password = "";
            remarks = "";
            timeout = DefaultServerTimeoutSec;
        }

        public string Identifier()
        {
            return server + ':' + server_port;
        }

        public string FormatHostName(string hostName)
        {
            // CheckHostName() 不会执行真正的DNS查找
            switch (Uri.CheckHostName(hostName))
            {
                case UriHostNameType.IPv6: // Add square bracket when IPv6 (RFC3986)
                    return $"[{hostName}]";
                default: // IPv4 or domain name
                    return hostName;
            }
        }
    }
}
