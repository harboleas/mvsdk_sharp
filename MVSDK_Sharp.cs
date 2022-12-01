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
    	interfaceTypeAll = 0x00000000,
    	interfaceTypeGige = 0x00000001,
    	interfaceTypeUsb3 = 0x00000002,
    	interfaceTypeCL = 0x00000004,
    	interfaceTypePCIe = 0x00000008,
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

}