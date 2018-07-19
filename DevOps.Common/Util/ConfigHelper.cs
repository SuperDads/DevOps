using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace DevOps.Common.Utils
{
    public class ConfigHelper : IDisposable
    {
        private static Configuration config;

        ConfigHelper(string path)
        {
            config = WebConfigurationManager.OpenWebConfiguration(path);
        }

        ConfigHelper() : this(AppDomain.CurrentDomain.BaseDirectory)
        { }

        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="configKey">节点名称</param>
        /// <returns></returns>
        public static string Read(string configKey)
        {
            return Read(configKey);
        }

        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="configKey">节点名称</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static string Read(string configKey, string defaultVal)
        {
            string val = default(string);

            if (string.IsNullOrWhiteSpace(defaultVal))
                val = defaultVal;

            try
            {
                val = ConfigurationManager.AppSettings[configKey];
            }
            catch (Exception)
            {
                val = "";
            }
            return val;

        }

        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="configKey">节点名称</param>
        /// <returns></returns>
        public static int ReadInt(string configKey)
        {
            int val = default(int);

            try
            {
                val = Convert.ToInt32(ConfigurationManager.AppSettings[configKey]);
            }
            catch (Exception)
            { }
            return val;

        }

        /// <summary>
        /// 读取节点值
        /// </summary>
        /// <param name="configKey">节点名称</param>
        /// <returns></returns>
        public static bool ReadBool(string configKey)
        {
            bool val = default(bool);

            try
            {
                val = Convert.ToBoolean(ConfigurationManager.AppSettings[configKey]);
            }
            catch (Exception)
            { }
            return val;
        }

        /// <summary>
        /// 设置应用程序配置节点，如果已经存在此节点，则会修改该节点的值，否则添加此节点
        /// </summary>
        /// <param name="configKey">节点名称</param>
        /// <param name="configValue">节点值</param>
        public static void SetAppSetting(string configKey, string configValue)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");

            if (appSetting.Settings[configKey] == null)//如果不存在此节点，则添加
            {
                appSetting.Settings.Add(configKey, configValue);
            }
            else// 如果存在此节点，则修改
            {
                appSetting.Settings[configKey].Value = configValue;
            }
        }

        /// <summary>
        /// 保存所作的修改
        /// </summary>
        void Save()
        {
            config.Save();
            config = null;
        }

        void IDisposable.Dispose()
        {
            if (config != null)
            {
                config.Save();
            }
        }

    }
}
