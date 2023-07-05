using System.Runtime.InteropServices;

namespace netch_process.libs;

public static class Redirector
{
    public enum NameList
    {
        AIO_FILTERLOOPBACK,
        AIO_FILTERINTRANET, // LAN
        AIO_FILTERPARENT,
        AIO_FILTERICMP,
        AIO_FILTERTCP,
        AIO_FILTERUDP,
        AIO_FILTERDNS,

        AIO_ICMPING,

        AIO_DNSONLY,
        AIO_DNSPROX,
        AIO_DNSHOST,
        AIO_DNSPORT,

        AIO_TGTHOST,
        AIO_TGTPORT,
        AIO_TGTUSER,
        AIO_TGTPASS,

        AIO_CLRNAME,
        AIO_ADDNAME,
        AIO_BYPNAME
    }

    public static bool Dial(NameList name, bool value)
    {
        return aio_dial(name, value.ToString().ToLower());
    }

    public static bool Dial(NameList name, string value)
    {
        return aio_dial(name, value);
    }

    public static Task<bool> InitAsync()
    {
        return Task.Run(aio_init);
    }

    public static Task<bool> FreeAsync()
    {
        return Task.Run(aio_free);
    }

    private const string Redirector_bin = "redirector.bin";

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool aio_register([MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool aio_unregister([MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_dial(NameList name, [MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_init();

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_free();

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern ulong aio_getUP();

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern ulong aio_getDL();
}