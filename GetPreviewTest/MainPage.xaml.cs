using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GetPreviewTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaCapture _mediaCapture;
        bool _isPreviewing;
        DisplayRequest _displayRequest = new DisplayRequest();
        public MainPage()
        {
            this.InitializeComponent();
            // initMediaCapture();
            StartPreviewAsync();
        }
        private async Task StartPreviewAsync()
        {
            try
            {
                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();
                _displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {

            }
            try
            {
                PreviewControl.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (System.IO.FileLoadException)
            {

            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var previewProperties = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;
            var videoFrame = new VideoFrame(BitmapPixelFormat.Bgra8, (int)previewProperties.Width, (int)previewProperties.Height);
            using (var currentFrame = await _mediaCapture.GetPreviewFrameAsync(videoFrame))
            {
                SoftwareBitmap previewFrame = currentFrame.SoftwareBitmap;
                var sbSource = new SoftwareBitmapSource();
                await sbSource.SetBitmapAsync(previewFrame);
                PreviewFrameImage.Source = sbSource;
            }
        }
        //private async Task initMediaCapture()
        //{
        //    var cameraDevice = await FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel.Back);
        //    _mediaCapture = new MediaCapture();
        //    var settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id };
        //    try
        //    {
        //        await _mediaCapture.InitializeAsync(settings);

        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        Debug.WriteLine("The app was denied access to the camera");
        //    }
        //    if (cameraDevice.EnclosureLocation == null || cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
        //    {
        //        // No information on the location of the camera, assume it's an external camera, not integrated on the device
        //        _externalCamera = true;
        //    }
        //    else
        //    {
        //        // Camera is fixed on the device
        //        _externalCamera = false;

        //        // Only mirror the preview if the camera is on the front panel
        //        _mirroringPreview = (cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
        //    }

        //    await StartPreviewAsync();

        //}

        //private async Task StartPreviewAsync()
        //{
        //    _displayRequest.RequestActive();
        //    await  _mediaCapture.StartPreviewAsync();
        //}
        ////private async Task SetPreviewRotationAsync()
        ////{
        ////    // Only need to update the orientation if the camera is mounted on the device
        ////    if (_externalCamera) return;

        ////    // Calculate which way and how far to rotate the preview
        ////    int rotationDegrees = ConvertDisplayOrientationToDegrees(_displayOrientation);

        ////    // The rotation direction needs to be inverted if the preview is being mirrored
        ////    if (_mirroringPreview)
        ////    {
        ////        rotationDegrees = (360 - rotationDegrees) % 360;
        ////    }

        ////    // Add rotation metadata to the preview stream to make sure the aspect ratio / dimensions match when rendering and getting preview frames
        ////    var props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
        ////    props.Properties.Add(RotationKey, rotationDegrees);
        ////    await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null);
        ////}
        //private static async Task<DeviceInformation> FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        //{
        //    // Get available devices for capturing pictures
        //    var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

        //    // Get the desired camera by panel
        //    DeviceInformation desiredDevice = allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredPanel);

        //    // If there is no device mounted on the desired panel, return the first device found
        //    return desiredDevice ?? allVideoDevices.FirstOrDefault();
        //}

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    var previewProperties = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview) as VideoEncodingProperties;
        //    VideoFrame videoFrame = new VideoFrame(BitmapPixelFormat.Bgra8, (int)previewProperties.Width, (int)previewProperties.Height);
        //    var source = new SoftwareBitmapSource();

        //    var previewFrame = await _mediaCapture.GetPreviewFrameAsync(videoFrame);

        //    SoftwareBitmap previewBitmap = videoFrame.SoftwareBitmap;
        //    await source.SetBitmapAsync(previewBitmap);
        //    MyImg.Source = source;

        //}


    }
}
