using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoShooter.Utils
{
    public class CameraUtils
    {
        private static FilterInfoCollection videoDevices;
        private static VideoCaptureDevice captureDevice;
        public static VideoCaptureDevice getCaptureDevice()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            if (videoDevices.Count > 0)
            {
                captureDevice = new VideoCaptureDevice(videoDevices[0].MonikerString);
                captureDevice.VideoResolution = captureDevice.VideoCapabilities[4];
            }
                
            return captureDevice;            
        }
    }
}
