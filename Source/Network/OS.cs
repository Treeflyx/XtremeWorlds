using System;
using System.IO;

namespace Mirage.Sharp.Asfw
{
  public class Os
  {
    private static bool _initialized;
    private static bool _isMobile;
    private static bool _isXBox;
    private static bool _isWindows;
    private static bool _isMac;
    private static bool _isLinux;
    private static bool _isBit64;

    private static void Activate()
    {
      if (Os._initialized)
        return;
      if (IntPtr.Size == 8)
        Os._isBit64 = true;
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Win32S:
        case PlatformID.Win32Windows:
        case PlatformID.Win32NT:
        case PlatformID.WinCE:
          Os._isWindows = true;
          break;
        case PlatformID.Unix:
          if (Directory.Exists("/Applications") && Directory.Exists("/System") && Directory.Exists("/Users") && Directory.Exists("/Volumes"))
          {
            Os._isMac = true;
            break;
          }
          Os._isLinux = true;
          break;
        case PlatformID.Xbox:
          Os._isXBox = true;
          break;
        case PlatformID.MacOSX:
          Os._isMac = true;
          break;
        default:
          Os._isMobile = true;
          break;
      }
      Os._initialized = true;
    }

    public static bool Mobile
    {
      get
      {
        Os.Activate();
        return Os._isMobile;
      }
    }

    public static bool XBox
    {
      get
      {
        Os.Activate();
        return Os._isXBox;
      }
    }

    public static bool Windows
    {
      get
      {
        Os.Activate();
        return Os._isWindows;
      }
    }

    public static bool Mac
    {
      get
      {
        Os.Activate();
        return Os._isMac;
      }
    }

    public static bool Linux
    {
      get
      {
        Os.Activate();
        return Os._isLinux;
      }
    }

    public static bool X86
    {
      get
      {
        Os.Activate();
        return !Os._isBit64;
      }
    }

    public static bool X64
    {
      get
      {
        Os.Activate();
        return Os._isBit64;
      }
    }
  }
}
