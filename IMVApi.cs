using System;
using System.Runtime.InteropServices;
using OpenCvSharp;

namespace MVSDK_Sharp 
{
    // API
    public static class IMVApi
    {
        // Ver IMVApi.h del SDK para una descripcion de las funciones 

        [DllImport("MVSDK.so")]
        static extern int IMV_EnumDevices(out IMV_DeviceList deviceList, IMV_EInterfaceType interfaceType);   

        [DllImport("MVSDK.so")]
        static extern int IMV_CreateHandle(out IntPtr handle, IMV_ECreateHandleMode mode, in uint pIdentifier);   

        [DllImport("MVSDK.so")]
        static extern int IMV_DestroyHandle(IntPtr handle);   

        [DllImport("MVSDK.so")]
        static extern int IMV_GetDeviceInfo(IntPtr handle, out IMV_DeviceInfo devInfo);

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
        static extern int IMV_GetFrame(IntPtr handle, out IMV_Frame frame, uint timeoutMS);

        [DllImport("MVSDK.so")]
        static extern int IMV_ReleaseFrame(IntPtr handle, in IMV_Frame frame);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetBufferCount(IntPtr handle, uint nSize);

        [DllImport("MVSDK.so")]
        static extern int IMV_ClearFrameBuffer(IntPtr handle);

        [DllImport("MVSDK.so")]
        static extern bool IMV_FeatureIsAvailable(IntPtr handle, string featureName);

        [DllImport("MVSDK.so")]
        static extern bool IMV_FeatureIsReadable(IntPtr handle, string featureName);

        [DllImport("MVSDK.so")]
        static extern bool IMV_FeatureIsWriteable(IntPtr handle, string featureName);
        
        [DllImport("MVSDK.so")]
        static extern int IMV_GetIntFeatureValue(IntPtr handle, string featureName, out UInt64 intValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetIntFeatureMin(IntPtr handle, string featureName, out UInt64 intValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetIntFeatureMax(IntPtr handle, string featureName, out UInt64 intValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetIntFeatureInc(IntPtr handle, string featureName, out UInt64 intValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetIntFeatureValue(IntPtr handle, string featureName, UInt64 intValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetDoubleFeatureValue(IntPtr handle, string featureName, out double doubleValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetDoubleFeatureMin(IntPtr handle, string featureName, out double doubleValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetDoubleFeatureMax(IntPtr handle, string featureName, out double doubleValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetDoubleFeatureValue(IntPtr handle, string featureName, double doubleValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetBoolFeatureValue(IntPtr handle, string featureName, out bool boolValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetBoolFeatureValue(IntPtr handle, string featureName, bool boolValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetEnumFeatureValue(IntPtr handle, string featureName, out UInt64 enumValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetEnumFeatureValue(IntPtr handle, string featureName, UInt64 enumValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetStringFeatureValue(IntPtr handle, string featureName, out IMV_String stringValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetStringFeatureValue(IntPtr handle, string featureName, string stringValue);

        [DllImport("MVSDK.so")]
        static extern int IMV_ExecuteCommandFeature(IntPtr handle, string featureName);

        [DllImport("MVSDK.so")]
        static extern int IMV_GetEnumFeatureSymbol(IntPtr handle, string featureName, out IMV_String enumSymbol);

        [DllImport("MVSDK.so")]
        static extern int IMV_SetEnumFeatureSymbol(IntPtr handle, string featureName, string enumSymbol);



        // Enumera los dispositivos conectados
        public static IMV_DeviceInfo[] EnumDevices(IMV_EInterfaceType eInterfaceType)
        {
            IMV_DeviceList devlist;
            var ret = IMV_EnumDevices(out devlist, eInterfaceType);
            var info = new IMV_DeviceInfo[devlist.nDevNum];
            int size = Marshal.SizeOf<IMV_DeviceInfo>();
            for (int i = 0; i < devlist.nDevNum; i++)
                info[i] = Marshal.PtrToStructure<IMV_DeviceInfo>(devlist.pDevInfo + i*size);

            return info;      
        }

        public class VideoCapture : IDisposable
        {
            IntPtr handle;
            public IMV_DeviceInfo devInfo;

            public VideoCapture(uint index)
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
 
            // Guarda la configuracion de la camara en un archivo
            public int SaveCfg(string fileName)
            {
                var ret = IMV_SaveDeviceCfg(handle, fileName);
                return ret;

            }
            // Carga en la camara el archivo de configuracion
            public int LoadCfg(string fileName, out IMV_ErrorList errorList)
            {
                var ret = IMV_LoadDeviceCfg(handle, fileName, out errorList);
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
                var rows = (int) imv_frame.frameInfo.height;
                var cols = (int) imv_frame.frameInfo.width;
               // Console.WriteLine(imv_frame.frameInfo.recvFrameTime);
                
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

                var ret2 = IMV_ReleaseFrame(handle, in imv_frame);
                return ret;
            }

            public bool FpsEnable
            {
                get 
                {
                    bool val;
                    IMV_GetBoolFeatureValue(handle, "AcquisitionFrameRateEnable", out val);
                    return val;
                }
                set
                {
                    IMV_SetBoolFeatureValue(handle, "AcquisitionFrameRateEnable", value);
                }
            }

            public double Fps
            {
                get 
                {
                    double val;
                    IMV_GetDoubleFeatureValue(handle, "AcquisitionFrameRate", out val);
                    return val;
                }
                set
                {
                    IMV_SetDoubleFeatureValue(handle, "AcquisitionFrameRate", value);
                }
            }

            public UInt64 ExposureAuto
            {
                get 
                {
                    UInt64 val;
                    IMV_GetEnumFeatureValue(handle, "ExposureAuto", out val);
                    return val;
                }
                set
                {
                    IMV_SetEnumFeatureValue(handle, "ExposureAuto", value);
                }
            }

            public double ExposureTime
            {
                get 
                {
                    double val;
                    IMV_GetDoubleFeatureValue(handle, "ExposureTime", out val);
                    return val;
                }
                set
                {
                    IMV_SetDoubleFeatureValue(handle, "ExposureTime", value);
                }
            }

            public int SetSoftTriggerConf()
            {
                int ret = Const.IMV_OK;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerSource", "Software");
                if (ret != Const.IMV_OK)
                    return ret;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerSelector", "FrameStart");
                if (ret != Const.IMV_OK)
                    return ret;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerMode", "On");
                if (ret != Const.IMV_OK)
                    return ret;

                return ret;                
            }

            public int SetLineTriggerConf(int n_line)
            {
                int ret = Const.IMV_OK;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerSource", "Line"+n_line);
                if (ret != Const.IMV_OK)
                    return ret;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerSelector", "FrameStart");
                if (ret != Const.IMV_OK)
                    return ret;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerActivation", "RisingEdge");
                if (ret != Const.IMV_OK)
                    return ret;
                ret = IMV_SetEnumFeatureSymbol(handle, "TriggerMode", "On");
                if (ret != Const.IMV_OK)
                    return ret;

                return ret;                
            }

            public int TriggerOff()
            {
                var ret = IMV_SetEnumFeatureSymbol(handle, "TriggerMode", "Off");
                return ret;
            }

            public int SoftTrigger()
            {
                var ret = IMV_ExecuteCommandFeature(handle, "TriggerSoftware");
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

            var cam = new IMVApi.VideoCapture(0);
            Console.WriteLine(cam.devInfo.cameraKey);
            Console.WriteLine(cam.devInfo.DeviceSpecificInfo.usbDeviceInfo.maxPower);
            Console.WriteLine(cam.IsOpen());
            Console.WriteLine(cam.Open());
            Console.WriteLine(cam.IsOpen());

            cam.ExposureAuto = 1;
            Console.WriteLine("Exposure Auto " + cam.ExposureAuto.ToString());
            Console.WriteLine("Exposure Time " + cam.ExposureTime.ToString());
            Console.WriteLine("Fps enable " + cam.FpsEnable.ToString());
            Console.WriteLine("Fps " + cam.Fps.ToString());
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