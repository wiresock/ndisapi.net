using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NdisApiDotNet;

int failures = 0;
int tests = 0;

void Run(string name, Action test)
{
    tests++;
    NativeStub.Reset();
    try
    {
        test();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception exception)
    {
        failures++;
        Console.WriteLine($"FAIL {name}: {exception.Message}");
    }
}

void CheckName(string name, Func<NdisApi> open, string expectedHex)
{
    Run(name, () =>
    {
        byte[] expected = Convert.FromHexString(expectedHex);
        using NdisApi api = open();
        Assert(!api.Handle.IsInvalid, "Open returned an invalid handle.");
        Assert(NativeStub.OpenCount() == 1, "Open did not call the native entry point once.");

        // OpenFilterDriver takes a pointer without a length. Inspect the saved
        // managed buffer first, so a failing test never reads past its allocation
        // looking for a missing NUL. Reopen then exercises these exact bytes
        // through P/Invoke against the native comparison below.
        var field = typeof(NdisApi).GetField("_driverNameBytes", BindingFlags.Instance | BindingFlags.NonPublic);
        byte[] actual = (byte[])field.GetValue(api);
        Assert(actual.SequenceEqual(expected),
            $"Expected encoded name {expectedHex}, got {Convert.ToHexString(actual)}.");

        NativeStub.ExpectName(expected, expected.Length);
        api.Reopen();
        Assert(!api.Handle.IsInvalid, "Reopen did not pass the terminated encoded name to native code.");
        Assert(NativeStub.OpenCount() == 2, "Reopen did not call the native entry point.");
        Assert(NativeStub.CloseCount() == 1, "Reopen did not close the original handle.");
        api.Close();
        Assert(NativeStub.CloseCount() == 2, "Close did not release the reopened handle.");
    });
}

CheckName("default Unicode", () => NdisApi.Open(), "4E00440049005300520044000000");
CheckName("explicit Unicode", () => NdisApi.Open("N\u00C9", Encoding.Unicode), "4E00C9000000");
CheckName("terminated Unicode", () => NdisApi.Open("NDISRD\0"), "4E00440049005300520044000000");
CheckName("empty Unicode", () => NdisApi.Open(""), "0000");
CheckName("Unicode bool overload", () => NdisApi.Open("NDISRD", false), "4E00440049005300520044000000");
CheckName("legacy bool overload", () => NdisApi.Open("NDISRD", true), "4E444953524400");
CheckName("explicit Latin-1", () => NdisApi.Open("N\u00C9", Encoding.GetEncoding("ISO-8859-1")), "4EC900");
CheckName("terminated Latin-1", () => NdisApi.Open("N\u00C9\0", Encoding.GetEncoding("ISO-8859-1")), "4EC900");
CheckName("empty Latin-1", () => NdisApi.Open("", Encoding.GetEncoding("ISO-8859-1")), "00");
CheckName("explicit UTF-8", () => NdisApi.Open("N\u00C9", Encoding.UTF8), "4EC38900");
CheckName("explicit big-endian Unicode", () => NdisApi.Open("A", Encoding.BigEndianUnicode), "00410000");

foreach (bool legacy in new[] { false, true })
{
    Run($"null name (legacy={legacy})", () =>
    {
        try
        {
            using NdisApi api = NdisApi.Open(null, legacy);
            throw new Exception("Expected ArgumentNullException.");
        }
        catch (ArgumentNullException exception)
        {
            Assert(exception.ParamName == "s", "The existing Encoding.GetBytes null exception changed.");
            Assert(NativeStub.OpenCount() == 0, "A null name reached native code.");
        }
    });
}

foreach (long invalidHandle in new long[] { 0, -1 })
{
    Run($"invalid native handle {invalidHandle}", () =>
    {
        NativeStub.ReturnHandle(new IntPtr(invalidHandle));
        try
        {
            using NdisApi api = NdisApi.Open();
            throw new Exception("Expected Win32Exception.");
        }
        catch (Win32Exception exception)
        {
            Assert(exception.NativeErrorCode == 110, "The native open error was not preserved.");
        }
    });
}

Console.WriteLine($"{tests - failures}/{tests} tests passed.");
return failures == 0 ? 0 : 1;

static void Assert(bool condition, string message)
{
    if (!condition)
        throw new Exception(message);
}

internal static class NativeStub
{
    [DllImport("ndisapi.dll", EntryPoint = "TestReset")]
    internal static extern void Reset();

    [DllImport("ndisapi.dll", EntryPoint = "TestExpectName")]
    internal static extern void ExpectName(byte[] expected, int length);

    [DllImport("ndisapi.dll", EntryPoint = "TestReturnHandle")]
    internal static extern void ReturnHandle(IntPtr handle);

    [DllImport("ndisapi.dll", EntryPoint = "TestOpenCount")]
    internal static extern int OpenCount();

    [DllImport("ndisapi.dll", EntryPoint = "TestCloseCount")]
    internal static extern int CloseCount();
}
