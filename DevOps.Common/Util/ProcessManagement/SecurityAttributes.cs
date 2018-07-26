using System;

namespace DevOps.Common.Util.ProcessManagement
{
    public struct SecurityAttributes
    {
        public UInt32 nLength;
        public IntPtr lpSecurityDescriptor;
        public Int32 bInheritHandle;
    }
}
