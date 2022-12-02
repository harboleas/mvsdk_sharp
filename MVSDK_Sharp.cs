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

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5*256)]
        public char[][] cameraReserved;






    }


}