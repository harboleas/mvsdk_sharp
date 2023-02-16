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
        
        public static int EnumDevices()
        {
            return 0;    
        }

        static void Main(string[] args)
        {

            IMV_DeviceList devlist;
            //Console.WriteLine(IMV_GetVersion());
            var ret = IMV_EnumDevices(out devlist, IMV_EInterfaceType.interfaceTypeUsb3);
            Console.WriteLine(ret);
            Console.WriteLine(devlist.nDevNum);
            var info = Marshal.PtrToStructure<IMV_DeviceInfo>(devlist.pDevInfo);
            Console.WriteLine(info.cameraName);
            Console.WriteLine(info.vendorName);
            Console.WriteLine(info.modelName);
            Console.WriteLine(info.manufactureInfo);
            Console.WriteLine(info.cameraKey);
            Console.WriteLine(info.deviceVersion);
            Console.WriteLine(info.DeviceSpecificInfo.usbDeviceInfo.maxPower);

        }
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
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_GigEInterfaceInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string macAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ipAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string subnetMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string defaultGateWay;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public _string_256[] chReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_UsbInterfaceInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string description;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string vendorID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string deviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string subsystemID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string revision;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string macAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ipAddress;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string subnetMask;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string defaultGateWay;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string protocolVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string configurationValid;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string genCPVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string u3vVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string deviceGUID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string familyName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string u3vSerialNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string speed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string maxPower;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public _string_256[] chReserved;
    }

    [StructLayout(LayoutKind.Explicit)]
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
//        [FieldOffset(0)]
//        public IMV_GigEInterfaceInfo gigeInterfaceInfo;
        [FieldOffset(0)]
        public IMV_UsbInterfaceInfo usbInterfaceInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMV_DeviceInfo
    {
    	public IMV_ECameraType nCameraType;		

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] nCameraReserved;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string cameraKey;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string cameraName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string serialName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string vendorName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string modelName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string manufactureInfo;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string deviceVersion;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public _string_256[] cameraReserved;

        public _DeviceSpecificInfo DeviceSpecificInfo;

        public IMV_EInterfaceType nInterfaceType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public int[] nInterfaceReserved;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InterfaceName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public _string_256[] interfaceReserved;

        public _InterfaceInfo InterfaceInfo;

    }


}