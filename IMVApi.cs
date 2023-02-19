using System;
using System.Runtime.InteropServices;

namespace MVSDK_Sharp 
{
    // API
    public static class IMVApi
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

        [DllImport("MVSDK.so")]
        static extern int IMV_CreateHandle(out IntPtr handle, IMV_ECreateHandleMode mode, IntPtr pIdentifier);   
        
        [StructLayout(LayoutKind.Sequential)]
        struct Id
        {
            public uint index;
        }

        [DllImport("MVSDK.so")]
        static extern int IMV_DestroyHandle(IntPtr handle);   
 
        public class VideoCam : IDisposable
        {
            IntPtr handle;
            public IMV_DeviceInfo info;

            public VideoCam(uint index)
            {
                info = EnumDevices(IMV_EInterfaceType.interfaceTypeUsb3)[index];

                var id = new Id();
                id.index = index;
                IntPtr pid = Marshal.AllocHGlobal(Marshal.SizeOf(id));
                Marshal.StructureToPtr<Id>(id, pid, true);
                IMV_CreateHandle(out handle, IMV_ECreateHandleMode.modeByIndex, pid);
                Marshal.FreeHGlobal(pid);
            }

            public void Dispose()
            {
                IMV_DestroyHandle(handle);
            }

        }
    }

    //////

    public static class Prueba
    {
        static void Main(string[] args)
        {
        
            var info = IMVApi.EnumDevices(IMV_EInterfaceType.interfaceTypeUsb3);

            for (int i = 0; i < info.Length; i++)
            {
                Console.WriteLine(info[i].cameraKey);
            }

            var cam = new IMVApi.VideoCam(0);
            Console.WriteLine(cam.info.cameraKey);
        }
    }

}