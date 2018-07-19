using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Common.Util
{
    public class Util
    {
        #region 检查系统和.NET Framework版本

        /// <summary>
        /// 检查当前此操作系统是否为Vista或更高版本
        /// </summary>
        /// 参考："https://docs.microsoft.com/zh-cn/windows/desktop/SysInfo/operating-system-version"
        /// <returns>返回：是否符合最低要求</returns>
        public static bool IsWinVistaOrHigher()
        {
            return Environment.OSVersion.Version.Major > 5;
        }

        /// <summary>
        /// 检查运行时版本
        /// </summary>
        /// <remarks>
        /// 参考："https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed"
        /// </remarks>
        /// <returns>返回：是否符合要求</returns>
        public static bool IsSupportedRuntimeVersion()
        {
            /*
             * +-----------------------------------------------------------------+----------------------------+
             * | Version                                                         | 发布 DWORD 的值            |
             * +-----------------------------------------------------------------+----------------------------+
             * | .NET Framework 4.5 installed on Windows                         | 378389                     |
             * | .NET Framework 4.6.2 installed on Windows 10 Anniversary Update | 394802                     |
             * | .NET Framework 4.6.2 installed on all other Windows OS versions | 394806                     |
             * +-----------------------------------------------------------------+----------------------------+
             */
            // 支持最小的Framework版本4.5
            const int minSupportedRelease = 378389;
            // 默认注册表路径
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = OpenRegKey(subkey, false, RegistryHive.LocalMachine))
            {
                if (ndpKey?.GetValue("Release") != null)
                {
                    var releaseKey = (int)ndpKey.GetValue("Release");

                    if (releaseKey >= minSupportedRelease)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 打开注册表
        /// </summary>
        /// <param name="name">注册表路径</param>
        /// <param name="writable">是否可写</param>
        /// <param name="hive">注册表注册项</param>
        /// <returns>返回：RegistryKey类</returns>
        public static RegistryKey OpenRegKey(string name, bool writable, RegistryHive hive = RegistryHive.CurrentUser)
        {
            // 我们正在为x86和x64构建x86二进制文件
            // 打开注册表时出现问题
            // 检测操作系统而不是CPU
            if (name.IsNullOrEmpty())
                throw new ArgumentException(nameof(name));
            try
            {
                RegistryKey userKey = RegistryKey.OpenBaseKey(hive,
                        Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32)
                    .OpenSubKey(name, writable);
                return userKey;
            }
            catch (ArgumentException ae)
            {
                //MessageBox.Show("OpenRegKey: " + ae.ToString());
                return null;
            }
            catch (Exception e)
            {
                //Logging.LogUsefulException(e);
                return null;
            }
        }

        #endregion

        #region 内存控制

        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="removePages"></param>
        public static void ReleaseMemory(bool removePages)
        {
            // release any unused pages
            // making the numbers look good in task manager
            // this is totally nonsense in programming
            // but good for those users who care
            // making them happier with their everyday life
            // which is part of user experience
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            // 进程使用的内存交换到虚拟内存
            if (removePages)
            {
                // as some users have pointed out
                // removing pages from working set will cause some IO
                // which lowered user experience for another group of users
                //
                // so we do 2 more things here to satisfy them:
                // 1. only remove pages once when configuration is changed
                // 2. add more comments here to tell users that calling
                //    this function will not be more frequent than
                //    IM apps writing chat logs, or web browsers writing cache files
                //    if they're so concerned about their disk, they should
                //    uninstall all IM apps and web browsers
                //
                // please open an issue if you're worried about anything else in your computer
                // no matter it's GPU performance, monitor contrast, audio fidelity
                // or anything else in the task manager
                // we'll do as much as we can to help you
                //
                // just kidding
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle,
                                         (UIntPtr)0xFFFFFFFF, // 0xFFFFFFFF 为 -1
                                         (UIntPtr)0xFFFFFFFF);
            }
        }

        /// <summary>
        /// 设置指定进程的最小和最大工作集大小
        /// </summary>
        /// <param name="process">进程句柄</param>
        /// <param name="minimumWorkingSetSize"></param>
        /// <param name="maximumWorkingSetSize"></param>
        /// <remarks>
        /// 参考："https://docs.microsoft.com/zh-cn/windows/desktop/api/winbase/nf-winbase-setprocessworkingsetsize"
        /// </remarks>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetProcessWorkingSetSize(IntPtr process,
    UIntPtr minimumWorkingSetSize, UIntPtr maximumWorkingSetSize);

        #endregion

        #region 扩展带宽

        /// <summary>
        /// 返回扩展带宽
        /// </summary>
        /// <param name="rawBandWidth">原始带宽</param>
        /// <returns>返回：BandwidthScaleInfo struct</returns>
        public static BandwidthScaleInfo GetBandwidthScale(long rawBandWidth)
        {
            long scale = 1;
            float f = rawBandWidth;
            string unit = "B";

            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "KiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "MiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "GiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "TiB";
            }
            return new BandwidthScaleInfo(f, unit, scale);
        }

        #endregion

    }
}
