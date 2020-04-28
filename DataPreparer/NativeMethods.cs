
using System;
using System.Runtime.InteropServices;

static class NativeMethods {

    const string KERNEL32 = "Kernel32.dll";

    [DllImport(KERNEL32)]
    public extern static void CopyMemory(IntPtr dest, IntPtr src, uint length);

}