using System;
using System.Runtime.InteropServices;

namespace Camara_MARS
{
    // API
    public class IMVApi
    {
        [DllImport("MVSDK.so")]
        static extern string IMV_GetVersion();   

        [DllImport("MVSDK.so")]
        static extern int IMV_EnumDevices(out IMV_DeviceList DeviceList, IMV_EInterfaceType interfaceType);   
        
        public static IMV_DeviceInfo[] EnumDevices(IMV_EInterfaceType EInterfaceType)
        {
            IMV_DeviceList devlist;
            var ret = IMV_EnumDevices(out devlist, EInterfaceType);
            var info = new IMV_DeviceInfo[devlist.nDevNum];
            int size = Marshal.SizeOf<IMV_DeviceInfo>();
            for (int i = 0; i < devlist.nDevNum; i++)
                info[i] = Marshal.PtrToStructure<IMV_DeviceInfo>(devlist.pDevInfo + i*size);

            return info;      
        }

        static void Main(string[] args)
        {

            var info = EnumDevices(IMV_EInterfaceType.interfaceTypeUsb3);

            for (int i = 0; i < info.Length; i++)
            {
                Console.WriteLine(info[i].cameraName);
                Console.WriteLine(info[i].vendorName);
                Console.WriteLine(info[i].modelName);
                Console.WriteLine(info[i].manufactureInfo);
                Console.WriteLine(info[i].cameraKey);
                Console.WriteLine(info[i].deviceVersion);
                Console.WriteLine(info[i].DeviceSpecificInfo.usbDeviceInfo.maxPower);
                Console.WriteLine(info[i].InterfaceInfo.usbInterfaceInfo.description);
            }

        }
    }


    // Definicion de constantes
    public static class Const
    {
        // Error code
        public const int IMV_OK = 0;
        public const int IMV_ERROR = -101;
        public const int IMV_INVALID_HANDLE = -102;
        public const int IMV_INVALID_PARAM = -103;
        public const int IMV_INVALID_FRAME_HANDLE = -104;
        public const int IMV_INVALID_FRAME = -105;
        public const int IMV_INVALID_RESOURCE = -106;
        public const int IMV_INVALID_IP = -107;
        public const int IMV_NO_MEMORY = -108;
        public const int IMV_INSUFFICIENT_MEMORY = -109;
        public const int IMV_ERROR_PROPERTY_TYPE = -110;
        public const int IMV_INVALID_ACCESS = -111;
        public const int IMV_INVALID_RANGE = -112;
        public const int IMV_NOT_SUPPORT = -113;
        //
        public const int IMV_MAX_DEVICE_ENUM_NUM = 100;
        public const int IMV_MAX_STRING_LENTH = 256;
        public const int IMV_MAX_ERROR_LIST_NUM = 128;
        //
        public const int IMV_GVSP_PIX_MONO = 0x01000000;
        public const int IMV_GVSP_PIX_RGB = 0x02000000;
        public const int IMV_GVSP_PIX_COLOR = 0x02000000;
        public const uint IMV_GVSP_PIX_CUSTOM = 0x80000000;
        public const uint IMV_GVSP_PIX_COLOR_MASK = 0xFF000000;

        public const int IMV_GVSP_PIX_OCCUPY1BIT = 0x00010000;
        public const int IMV_GVSP_PIX_OCCUPY2BIT = 0x00020000;
        public const int IMV_GVSP_PIX_OCCUPY4BIT = 0x00040000;
        public const int IMV_GVSP_PIX_OCCUPY8BIT = 0x00080000;
        public const int IMV_GVSP_PIX_OCCUPY12BIT = 0x000C0000;
        public const int IMV_GVSP_PIX_OCCUPY16BIT = 0x00100000;
        public const int IMV_GVSP_PIX_OCCUPY24BIT = 0x00180000;
        public const int IMV_GVSP_PIX_OCCUPY32BIT = 0x00200000;
        public const int IMV_GVSP_PIX_OCCUPY36BIT = 0x00240000;
        public const int IMV_GVSP_PIX_OCCUPY48BIT = 0x00300000;
    }

    // Definicion de estructuras
    public enum IMV_EInterfaceType
    {
    	interfaceTypeAll = 0,
    	interfaceTypeGige = 1,
    	interfaceTypeUsb3 = 2,
    	interfaceTypeCL = 4,
    	interfaceTypePCIe = 8,
    }

    public enum IMV_ECameraType
    {
        typeGigeCamera = 0,
        typeU3vCamera = 1,
        typeCLCamera = 2,
        typePCIeCamera = 3,
        typeUndefinedCamera = 255	
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_DeviceList
    {
    	public uint nDevNum;		
        public IntPtr pDevInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _string_256 
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_GigEInterfaceInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string macAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string ipAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string subnetMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string defaultGateWay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public _string_256[] chReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_UsbInterfaceInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string vendorID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string deviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string subsystemID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string revision;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string speed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public _string_256[] chReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_GigEDeviceInfo
    {
      	/// Supported IP configuration options of device\n
    	/// value:4 Device supports LLA \n
    	/// value:5 Device supports LLA and Persistent IP\n
    	/// value:6 Device supports LLA and DHCP\n
    	/// value:7 Device supports LLA, DHCP and Persistent IP\n
    	/// value:0 Get fail
        public uint nIpConfigOptions;
    	/// Current IP Configuration options of device\n
    	/// value:4 LLA is active\n
    	/// value:5 LLA and Persistent IP are active\n
    	/// value:6 LLA and DHCP are active\n
    	/// value:7 LLA, DHCP and Persistent IP are active\n
    	/// value:0 Get fail
	    public uint nIpConfigCurrent;
	    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] nReserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string macAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string ipAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string subnetMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string defaultGateWay;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string protocolVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string ipConfiguration;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public _string_256[] chReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _bool
    {
        [MarshalAs(UnmanagedType.U1)]
        public bool value;
    }
 
    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_UsbDeviceInfo
    {

        [MarshalAs(UnmanagedType.U1)]
        public bool bLowSpeedSupported;
        [MarshalAs(UnmanagedType.U1)]
        public bool bFullSpeedSupported;
        [MarshalAs(UnmanagedType.U1)]
        public bool bHighSpeedSupported;
        [MarshalAs(UnmanagedType.U1)]
        public bool bSuperSpeedSupported;
        [MarshalAs(UnmanagedType.U1)]
        public bool bDriverInstalled;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public _bool[] boolReserved;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] Reserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string configurationValid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string genCPVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string u3vVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string deviceGUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string familyName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string u3vSerialNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string speed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string maxPower;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public _string_256[] chReserved;
    }

    [StructLayout(LayoutKind.Explicit, Size = 3096)]
    public struct _DeviceSpecificInfo
    {
//        [FieldOffset(0)]
//        public IMV_GigEDeviceInfo gigeDeviceInfo;
        [FieldOffset(0)]
        public IMV_UsbDeviceInfo usbDeviceInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct _InterfaceInfo
    {
        [FieldOffset(0)]
        public IMV_GigEInterfaceInfo gigeInterfaceInfo;
        [FieldOffset(0)]
        public IMV_UsbInterfaceInfo usbInterfaceInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_DeviceInfo
    {
    	public IMV_ECameraType nCameraType;		

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] nCameraReserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string cameraKey;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string cameraName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string serialName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string vendorName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string modelName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string manufactureInfo;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string deviceVersion;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public _string_256[] cameraReserved;

        public _DeviceSpecificInfo DeviceSpecificInfo;

        public IMV_EInterfaceType nInterfaceType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] nInterfaceReserved;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.IMV_MAX_STRING_LENTH)]
        public string InterfaceName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public _string_256[] interfaceReserved;

        public _InterfaceInfo InterfaceInfo;

    }

    public enum IMV_EPixelType
    {
    	// Undefined pixel type
    	gvspPixelTypeUndefined = -1,

    	// Mono Format
    	gvspPixelMono1p = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY1BIT | 0x0037),
    	gvspPixelMono2p = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY2BIT | 0x0038),
    	gvspPixelMono4p = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY4BIT | 0x0039),
    	gvspPixelMono8 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY8BIT | 0x0001),
    	gvspPixelMono8S = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY8BIT | 0x0002),
    	gvspPixelMono10 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0003),
    	gvspPixelMono10Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0004),
    	gvspPixelMono12 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0005),
    	gvspPixelMono12Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0006),
    	gvspPixelMono14 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0025),
    	gvspPixelMono16 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0007),

    	// Bayer Format
    	gvspPixelBayGR8 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY8BIT | 0x0008),
    	gvspPixelBayRG8 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY8BIT | 0x0009),
    	gvspPixelBayGB8 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY8BIT | 0x000A),
    	gvspPixelBayBG8 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY8BIT | 0x000B),
    	gvspPixelBayGR10 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x000C),
    	gvspPixelBayRG10 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x000D),
    	gvspPixelBayGB10 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x000E),
    	gvspPixelBayBG10 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x000F),
    	gvspPixelBayGR12 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0010),
    	gvspPixelBayRG12 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0011),
    	gvspPixelBayGB12 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0012),
    	gvspPixelBayBG12 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0013),
    	gvspPixelBayGR10Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0026),
    	gvspPixelBayRG10Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0027),
    	gvspPixelBayGB10Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0028),
    	gvspPixelBayBG10Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0029),
    	gvspPixelBayGR12Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x002A),
    	gvspPixelBayRG12Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x002B),
    	gvspPixelBayGB12Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x002C),
    	gvspPixelBayBG12Packed = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x002D),
    	gvspPixelBayGR16 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x002E),
    	gvspPixelBayRG16 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x002F),
    	gvspPixelBayGB16 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0030),
    	gvspPixelBayBG16 = (Const.IMV_GVSP_PIX_MONO | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0031),

    	// RGB Format
    	gvspPixelRGB8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x0014),
    	gvspPixelBGR8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x0015),
    	gvspPixelRGBA8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY32BIT | 0x0016),
    	gvspPixelBGRA8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY32BIT | 0x0017),
    	gvspPixelRGB10 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x0018),
    	gvspPixelBGR10 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x0019),
    	gvspPixelRGB12 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x001A),
    	gvspPixelBGR12 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x001B),
    	gvspPixelRGB16 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x0033),
    	gvspPixelRGB10V1Packed = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY32BIT | 0x001C),
    	gvspPixelRGB10P32 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY32BIT | 0x001D),
    	gvspPixelRGB12V1Packed = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY36BIT | 0X0034),
    	gvspPixelRGB565P = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0035),
    	gvspPixelBGR565P = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0X0036),

    	// YVR Format
    	gvspPixelYUV411_8_UYYVYY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x001E),
    	gvspPixelYUV422_8_UYVY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x001F),
    	gvspPixelYUV422_8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0032),
    	gvspPixelYUV8_UYV = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x0020),
    	gvspPixelYCbCr8CbYCr = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x003A),
    	gvspPixelYCbCr422_8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x003B),
    	gvspPixelYCbCr422_8_CbYCrY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0043),
    	gvspPixelYCbCr411_8_CbYYCrYY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x003C),
    	gvspPixelYCbCr601_8_CbYCr = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x003D),
    	gvspPixelYCbCr601_422_8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x003E),
    	gvspPixelYCbCr601_422_8_CbYCrY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0044),
    	gvspPixelYCbCr601_411_8_CbYYCrYY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x003F),
    	gvspPixelYCbCr709_8_CbYCr = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x0040),
    	gvspPixelYCbCr709_422_8 = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0041),
    	gvspPixelYCbCr709_422_8_CbYCrY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY16BIT | 0x0045),
    	gvspPixelYCbCr709_411_8_CbYYCrYY = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY12BIT | 0x0042),

    	// RGB Planar
    	gvspPixelRGB8Planar = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY24BIT | 0x0021),
    	gvspPixelRGB10Planar = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x0022),
    	gvspPixelRGB12Planar = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x0023),
    	gvspPixelRGB16Planar = (Const.IMV_GVSP_PIX_COLOR | Const.IMV_GVSP_PIX_OCCUPY48BIT | 0x0024),

    	//BayerRG10p和BayerRG12p格式，针对特定项目临时添加,请不要使用
    	//BayerRG10p and BayerRG12p, currently used for specific project, please do not use them
    	gvspPixelBayRG10p = 0x010A0058,
    	gvspPixelBayRG12p = 0x010c0059,

    	//mono1c格式，自定义格式
    	//mono1c, customized image format, used for binary output
    	gvspPixelMono1c = 0x012000FF,

    	//mono1e格式，自定义格式，用来显示连通域
    	//mono1e, customized image format, used for displaying connected domain
    	gvspPixelMono1e = 0x01080FFF
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_FrameInfo
    {
    	public UInt64 blockId;				
    	public uint	status;		
    	public uint	width;		
    	public uint	height;
    	public uint	size;	
    	IMV_EPixelType pixelFormat;	
    	public UInt64 timeStamp;				
    	public uint chunkCount;
    	public uint	paddingX;	
    	public uint	paddingY;
    	public uint	recvFrameTime;			
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
    	public uint[] nReserved;		
    }

}