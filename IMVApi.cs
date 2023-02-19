using System;
using System.Runtime.InteropServices;
using OpenCvSharp;

namespace MVSDK_Sharp 
{
    // API
    public static class IMVApi
   {
        [DllImport("MVSDK.so")]
        static extern int IMV_EnumDevices(out IMV_DeviceList DeviceList, IMV_EInterfaceType interfaceType);   

        [DllImport("MVSDK.so")]
        static extern int IMV_CreateHandle(out IntPtr handle, IMV_ECreateHandleMode mode, IntPtr pIdentifier);   

        [DllImport("MVSDK.so")]
        static extern int IMV_DestroyHandle(IntPtr handle);   

        [DllImport("MVSDK.so")]
        static extern int IMV_GetDeviceInfo(IntPtr handle, out IMV_DeviceInfo DevInfo);

        [DllImport("MVSDK.so")]
        static extern int IMV_Open(IntPtr handle);

        [DllImport("MVSDK.so")]
        static extern int IMV_OpenEx(IntPtr handle, IMV_ECameraAccessPermission accessPermission);

        [DllImport("MVSDK.so")]
        static extern bool IMV_IsOpen(IntPtr handle);

        [DllImport("MVSDK.so")]
        static extern int IMV_Close(IntPtr handle);

        [DllImport("MVSDK.so")]
        static extern int IMV_SaveDeviceCfg(IntPtr handle, string fileName);

        [DllImport("MVSDK.so")]
        static extern int IMV_LoadDeviceCfg(IntPtr handle, string fileName, out IMV_ErrorList errorList);


        [DllImport("MVSDK.so")]
        static extern int IMV_GetFrame(IntPtr handle, out IMV_Frame Frame, uint timeoutMS);

 
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

        public class VideoCam : IDisposable
        {
            IntPtr handle;
            public IMV_DeviceInfo devInfo;

            public VideoCam(uint index)
            {
                IntPtr pindex = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
                var index_b = BitConverter.GetBytes(index);
                for (int i = 0; i < index_b.Length; i++)
                    Marshal.WriteByte(pindex, i, index_b[i]);

                IMV_CreateHandle(out handle, IMV_ECreateHandleMode.modeByIndex, pindex);
                Marshal.FreeHGlobal(pindex);
           
                IMV_GetDeviceInfo(handle, out devInfo);
            
            }
            public int Open()
            {
                return IMV_Open(handle);
            }
            public int Open(IMV_ECameraAccessPermission accessPermission)
            {
                return IMV_OpenEx(handle, accessPermission);
            }

            public bool IsOpen()
            {
                return IMV_IsOpen(handle);
            }
            public int Close()
            {
                return IMV_Close(handle);
            }
 
            public int SaveCfg(string fileName)
            {
                var ret = IMV_SaveDeviceCfg(handle, fileName);
                return ret;
            }

            public int ReadFrame(uint timeoutMS)
            {
                IMV_Frame imv_frame;
                var ret = IMV_GetFrame(handle, out imv_frame, timeoutMS);
                Console.WriteLine(ret);
                Console.WriteLine(imv_frame.frameInfo.height);
                return ret;
            }

            public void Dispose()
            {
                IMV_DestroyHandle(handle);
            }

        }
    }

    //////

    public static class Test
    {
        static void Main(string[] args)
        {
        
            var info = IMVApi.EnumDevices(IMV_EInterfaceType.interfaceTypeUsb3);

            Console.WriteLine(info.Length);
            for (int i = 0; i < info.Length; i++)
            {
                Console.WriteLine(info[i].cameraKey);
            }

            var cam = new IMVApi.VideoCam(0);
            Console.WriteLine(cam.devInfo.cameraKey);
            Console.WriteLine(cam.devInfo.DeviceSpecificInfo.usbDeviceInfo.maxPower);
            Console.WriteLine(cam.IsOpen());
            Console.WriteLine(cam.Open(IMV_ECameraAccessPermission.accessPermissionControl));
            Console.WriteLine(cam.IsOpen());
            Console.WriteLine(cam.SaveCfg("pepe.xml"));

            cam.ReadFrame(50);
            cam.Close();
            cam.Dispose();

        }
    }

}