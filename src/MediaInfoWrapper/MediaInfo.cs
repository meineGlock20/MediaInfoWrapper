// SPDX-FileCopyrightText: 2025 meineGlock20
// SPDX-License-Identifier: MIT
// ----------------------------------------------------------------------------------------------
// This wrapper (MediaInfoWrapper) is MIT‐licensed.
// This product uses MediaInfo library, Copyright (c) 2002-2025 MediaArea.net SARL.
// ----------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Security;
using MediaInfoWrapper.Core;

namespace MediaInfoWrapper;

/// <summary>
/// A SafeHandle wrapper around the native MediaInfo handle.
/// </summary>
internal sealed class SafeMediaInfoHandle : SafeHandle
{
    // Add to SafeMediaInfoHandle class
    public void Initialize(IntPtr handle)
    {
        SetHandle(handle);
    }

    public SafeMediaInfoHandle() : base(IntPtr.Zero, ownsHandle: true) { }

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            NativeMethods.MediaInfo_Delete(handle);
            handle = IntPtr.Zero;
        }
        return true;
    }
}

/// <summary>
/// Thin interop surface for loading and invoking the native MediaInfo library.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static partial class NativeMethods
{
    // Change these constants if you ship native binaries for Linux/macOS:
#if WINDOWS
    public const string LibName = "MediaInfo.dll";
#elif LINUX
    public const string LibName = "libmediainfo.so";
#elif OSX
    public const string LibName = "libmediainfo.dylib";
#else
    public const string LibName = "MediaInfo.dll";
#endif

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_New();

    [LibraryImport(LibName)]
    public static partial void MediaInfo_Delete(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Open(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Open(IntPtr handle, IntPtr fileName);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Open_Buffer_Init(IntPtr handle, long fileSize, long fileOffset);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Open_Buffer_Init(IntPtr handle, long fileSize, long fileOffset);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Open_Buffer_Continue(IntPtr handle, IntPtr buffer, IntPtr bufferSize);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Open_Buffer_Continue(IntPtr handle, long fileSize, [In] byte[] buffer, IntPtr bufferSize);

    [LibraryImport(LibName)]
    public static partial long MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial long MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial void MediaInfo_Close(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Inform(IntPtr handle, IntPtr reserved);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Inform(IntPtr handle, IntPtr reserved);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Option(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string option, [MarshalAs(UnmanagedType.LPWStr)] string value);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfoA_Option(IntPtr handle, IntPtr option, IntPtr value);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_State_Get(IntPtr handle);

    [LibraryImport(LibName)]
    public static partial IntPtr MediaInfo_Count_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber);
}

/// <summary>
/// A managed wrapper around MediaInfoLib’s lifetime and P/Invoke calls.
/// Implements IDisposable so callers can dispose explicitly.
/// </summary>
public partial class MediaInfo : IDisposable
{
    private readonly SafeMediaInfoHandle _handle;
    private readonly bool _mustUseAnsi;
    private bool _disposed;

    /// <summary>
    /// Creates a new MediaInfo instance. Call <see cref="Dispose"/> when done.
    /// </summary>
    public MediaInfo()
    {
        IntPtr raw = IntPtr.Zero;
        try
        {
            raw = NativeMethods.MediaInfo_New();
        }
        catch
        {
            // If the native library cannot be loaded, raw remains IntPtr.Zero.
        }

        _handle = new SafeMediaInfoHandle();
        _handle.Initialize(raw);

        // On non‐Windows, assume ANSI entrypoints:
        _mustUseAnsi = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _handle.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    ~MediaInfo()
    {
        Dispose();
    }

    private IntPtr Handle
    {
        get
        {
            if (_disposed || _handle.IsInvalid)
                return IntPtr.Zero;
            return _handle.DangerousGetHandle();
        }
    }

    /// <summary>
    /// Opens the specified file. Throws if the native library isn’t loaded or the open fails.
    /// </summary>
    public void Open(string fileName)
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(MediaInfo), "Native MediaInfo handle is invalid.");

        int result;
        if (_mustUseAnsi)
        {
            IntPtr ansiPtr = Marshal.StringToHGlobalAnsi(fileName);
            try
            {
                result = (int)NativeMethods.MediaInfoA_Open(Handle, ansiPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(ansiPtr);
            }
        }
        else
        {
            result = (int)NativeMethods.MediaInfo_Open(Handle, fileName);
        }

        if (result == 0)
            throw new InvalidOperationException($"MediaInfo failed to open: {fileName}.");
    }

    /// <summary>
    /// Gets a metadata value for the given stream kind/number/parameter.  
    /// If not found, returns null (never returns an error string).
    /// </summary>
    public string? Get(
        StreamKind streamKind,
        int streamNumber,
        string parameter,
        InfoKind infoKind = InfoKind.Text,
        InfoKind kindOfSearch = InfoKind.Name)
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(MediaInfo), "Native MediaInfo handle is invalid.");

        if (string.IsNullOrEmpty(parameter))
            throw new ArgumentException("Parameter must be non-empty.", nameof(parameter));

        if (_mustUseAnsi)
        {
            IntPtr parmAnsi = Marshal.StringToHGlobalAnsi(parameter);
            try
            {
                IntPtr ptr = NativeMethods.MediaInfoA_Get(
                    Handle,
                    (IntPtr)streamKind,
                    streamNumber,
                    parmAnsi,
                    (IntPtr)infoKind,
                    (IntPtr)kindOfSearch);
                return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(parmAnsi);
            }
        }
        else
        {
            IntPtr ptr = NativeMethods.MediaInfo_Get(
                Handle,
                (IntPtr)streamKind,
                streamNumber,
                parameter,
                (IntPtr)infoKind,
                (IntPtr)kindOfSearch);
            return ptr == IntPtr.Zero ? null : Marshal.PtrToStringUni(ptr);
        }
    }

    /// <summary>
    /// Sets a MediaInfo option (e.g. "ParseSpeed", "Complete"). Returns the native response or null.
    /// </summary>
    public string? Option(string option, string value = "")
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(MediaInfo), "Native MediaInfo handle is invalid.");

        if (string.IsNullOrEmpty(option))
            throw new ArgumentException("Option must be non-empty.", nameof(option));

        if (_mustUseAnsi)
        {
            IntPtr optAnsi = Marshal.StringToHGlobalAnsi(option);
            IntPtr valAnsi = Marshal.StringToHGlobalAnsi(value);
            try
            {
                IntPtr ptr = NativeMethods.MediaInfoA_Option(Handle, optAnsi, valAnsi);
                return ptr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(optAnsi);
                Marshal.FreeHGlobal(valAnsi);
            }
        }
        else
        {
            IntPtr ptr = NativeMethods.MediaInfo_Option(Handle, option, value);
            return ptr == IntPtr.Zero ? null : Marshal.PtrToStringUni(ptr);
        }
    }

    /// <summary>
    /// Returns how many streams of a given kind (e.g. Audio, Text) exist.
    /// </summary>
    public int Count(StreamKind streamKind)
    {
        if (Handle == IntPtr.Zero)
            throw new ObjectDisposedException(nameof(MediaInfo), "Native MediaInfo handle is invalid.");
        return (int)NativeMethods.MediaInfo_Count_Get(Handle, (IntPtr)streamKind, -1);
    }
}