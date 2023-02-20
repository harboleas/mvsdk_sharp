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
        static extern int IMV_CreateHandle(out IntPtr handle, IMV_ECreateHandleMode mode, in uint pIdentifier);   

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
        static extern int IMV_StartGrabbing(IntPtr handle);
        
        [DllImport("MVSDK.so")]
        static extern bool IMV_IsGrabbing(IntPtr handle);

        [DllImport("MVSDK.so")]
        static extern int IMV_StopGrabbing(IntPtr handle);
 
        [DllImport("MVSDK.so")]
        static extern int IMV_GetFrame(IntPtr handle, out IMV_Frame Frame, uint timeoutMS);

        [DllImport("MVSDK.so")]
        static extern int IMV_ReleaseFrame(IntPtr handle, in IMV_Frame Frame);


 
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

                IMV_CreateHandle(out handle, IMV_ECreateHandleMode.modeByIndex, in index);
           
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

            public int StartGrabbing()
            {
                var ret = IMV_StartGrabbing(handle);
                return ret;
            }
            public bool IsGrabbing()
            {
                var ret = IMV_IsGrabbing(handle);
                return ret;
            }
            public int StopGrabbing()
            {
                var ret = IMV_StopGrabbing(handle);
                return ret;
            }
 

            public int ReadFrame(out Mat frame, uint timeoutMS)
            {
                IMV_Frame imv_frame;
                var ret = IMV_GetFrame(handle, out imv_frame, timeoutMS);
                var ret2 = IMV_ReleaseFrame(handle, in imv_frame);
                var rows = (int) imv_frame.frameInfo.height;
                var cols = (int) imv_frame.frameInfo.width;
                Console.WriteLine(imv_frame.frameInfo.recvFrameTime);
                
                switch(imv_frame.frameInfo.pixelFormat)
                {
                    case IMV_EPixelType.gvspPixelMono8:
                        frame = new Mat(rows, cols, MatType.CV_8UC1, imv_frame.pData);
                        break;
                    default:
                        // Cualquier cosa que no sea blanco y negro   
                        frame = new Mat(rows, cols, MatType.CV_8UC3, imv_frame.pData);
                        break;
                }

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

            var cam = new IMVApi.VideoCam(1);
            Console.WriteLine(cam.devInfo.cameraKey);
            Console.WriteLine(cam.devInfo.DeviceSpecificInfo.usbDeviceInfo.maxPower);
            Console.WriteLine(cam.IsOpen());
            Console.WriteLine(cam.Open());
            Console.WriteLine(cam.IsOpen());
            Console.WriteLine(cam.SaveCfg("pepe.xml"));

            Mat frame;
            cam.StartGrabbing();
            Console.WriteLine(cam.IsGrabbing());
            cam.ReadFrame(out frame, 50);
            frame.ImWrite("prueba.jpg");
            frame.Dispose();
            cam.StopGrabbing();
            cam.Close();
            cam.Dispose();

        }
    }

}