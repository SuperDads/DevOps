using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DevOps.Common.Util.ProcessManagement
{
    public class Job : IDisposable
    {
        private IntPtr handle = IntPtr.Zero;

        public Job()
        {
            handle = CreateJobObject(IntPtr.Zero, null);
            var extendedInfoPtr = IntPtr.Zero;
            var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = 0x2000
            };

            var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = info
            };

            try
            {
                int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
                extendedInfoPtr = Marshal.AllocHGlobal(length);
                Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

                if (!SetInformationJobObject(handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr,
                        (uint)length))
                    throw new Exception(string.Format("Unable to set information.  Error: {0}",
                        Marshal.GetLastWin32Error()));
            }
            finally
            {
                if (extendedInfoPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(extendedInfoPtr);
                    extendedInfoPtr = IntPtr.Zero;
                }
            }
        }

        public bool AddProcess(IntPtr processHandle)
        {
            var succ = AssignProcessToJobObject(handle, processHandle);

            if (!succ)
            {
                //Logging.Error("Failed to call AssignProcessToJobObject! GetLastError=" + Marshal.GetLastWin32Error());
            }

            return succ;
        }

        public bool AddProcess(int processId)
        {
            return AddProcess(Process.GetProcessById(processId).Handle);
        }

        #region IDisposable

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            disposed = true;

            if (disposing)
            {
                // no managed objects to free
            }

            if (handle != IntPtr.Zero)
            {
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
        }

        ~Job()
        {
            Dispose(false);
        }

        #endregion

        #region Interop

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateJobObject(IntPtr a, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, UInt32 cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        #endregion
    }
}
