using DevOps.Common.Util;
using System;
using System.Collections.Generic;

namespace DevOps.Framework.Soket
{
    [Serializable]
    public class Configuration
    {
        public int index;
        public List<Server> configs;

        private static string CONFIG_FILE = "config.json";

        public Server GetCurrentServer()
        {
            if (index >= 0 && index < configs.Count)
                return configs[index];
            else
                return GetDefaultServer();
        }

        public static void CheckServer(Server server)
        {
            CheckPort(server.server_port);
            CheckPassword(server.password);
            CheckServer(server.server);
            CheckTimeout(server.timeout, Server.MaxServerTimeoutSec);
        }

        public static Server GetDefaultServer()
        {
            return new Server();
        }

        public static void CheckPort(int port)
        {
            if (port <= 0 || port > 65535)
            { }
            // throw new ArgumentException(I18N.GetString("Port out of range"));
        }

        public static void CheckLocalPort(int port)
        {
            CheckPort(port);
            if (port == 8123)
            { }
            // throw new ArgumentException(I18N.GetString("Port can't be 8123"));
        }

        private static void CheckPassword(string password)
        {
            if (password.IsNullOrEmpty())
            { }
            // throw new ArgumentException(I18N.GetString("Password can not be blank"));
        }

        public static void CheckServer(string server)
        {
            if (server.IsNullOrEmpty())
            { }
            //throw new ArgumentException(I18N.GetString("Server IP can not be blank"));
        }

        public static void CheckTimeout(int timeout, int maxTimeout)
        {
            if (timeout <= 0 || timeout > maxTimeout)
            { }
            // throw new ArgumentException(string.Format(I18N.GetString("Timeout is invalid, it should not exceed {0}"), maxTimeout));
        }

    }
}
