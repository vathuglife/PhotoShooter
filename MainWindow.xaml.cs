using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AForge.Video.DirectShow;
using AForge.Video;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel;
using PhotoShooter.Utils;
using BMICalculator.Utils;
using FolderBrowser.Utils;
using System.Diagnostics;
using FolderBrowser.Models;
using System.Reflection;
using PhotoShooter.Popups.RenameImagePopup;
using PhotoShooter.Services;
using PhotoShooter.Services.Implementation;
using PhotoShooter.Constants;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using PhotoShooter.Enums;

namespace PhotoShooter
{
    public partial class MainWindow : Window
    {
        private VideoCaptureDevice videoCaptureDevice;
        private Bitmap bitmap;
        private string path;
        private bool isWebcamOn;
        private FirebaseService firebaseService;
        private ObservableCollection<ListViewEntry> imageObservableCollection;
        public MainWindow()
        {
            InitializeComponent();
            InitializeAppState();

        }
        public void InitializeAppState()
        {
            firebaseService = new FirebaseServiceImpl();
            imageObservableCollection = new ObservableCollection<ListViewEntry>();
            LoadNoCameraImage();
            Closing += OnMainWindowClose;
            isWebcamOn = false;
        }
        public void RefreshListView()
        {
            imageObservableCollection.Clear();
            string[] imagePaths = GetImagePaths();
            if (imagePaths.Length == 0)
            {
                ShowNoImagesFoundMessage();
                return;
            }

            foreach (string imagePath in imagePaths)
            {
                //Disposes of the image immediately after being added to the imageObservableCollectionView.               

                ListViewEntry listViewEntry = new ListViewEntry()
                {
                    //Image = null,
                    Image = ImageUtils.ToBitmapImage(imagePath),
                    PathName = imagePath,
                    FileName = System.IO.Path.GetFileName(imagePath),
                };
                imageObservableCollection.Add(listViewEntry);
            }
            ImageListView.ItemsSource = imageObservableCollection;

        }
        private void HandleWebcam(object sender, RoutedEventArgs e)
        {
            if (!isWebcamOn) { OpenWebcam(); }
            else CloseWebcam();
        }

        private void CaptureImage(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                ShowPathNotLoadedMessage();
                return;
            }
            if (!isWebcamOn)
            {
                ShowWebcamNotEnabledMessage();
                return;
            }
            string fullPath = GetFullImagePath();
            BitmapImage bitmapImage = ImageUtils.ToBitmapImage(bitmap);
            ImageUtils.saveBitmapImageToFile(new object[] { bitmapImage, fullPath });
            RefreshListView();

        }
        private void LoadImages(object sender, RoutedEventArgs e)
        {
            path = FolderBrowserDialogUtils.getPath();
            if (path == null) { return; }
            DirectoryTextbox.Text = path;
            RefreshListView();
        }
        private void RenameImage(object sender, RoutedEventArgs e)
        {
            ListViewEntry selectedEntry = (ListViewEntry)ImageListView.SelectedItem;
            RenameImagePopup RenameImagePopup = new RenameImagePopup(this, selectedEntry);
            RenameImagePopup.Show();
        }

        private void DeleteImage(object sender, RoutedEventArgs e)
        {

            if (!isDeleteImage()) return;
            ListViewEntry selectedEntry = (ListViewEntry)ImageListView.SelectedItem;
            selectedEntry.Image = null;
            ImageListView.ItemsSource = null;                        
            GC.Collect();
            DirectoryUtils.deleteFile(selectedEntry.PathName);
            RefreshListView();
        }
    
        private void SyncToFirebase(object sender, RoutedEventArgs e)
        {

            if (path == null)
            {
                ShowPathNotLoadedMessage();
                return;
            };
            FirebaseUploadResult result = firebaseService.uploadToFirebaseBucket(path);
            if (result == FirebaseUploadResult.SUCCESS)
            {
                ShowSuccessfulUploadMessage();
            }

        }

        private void UpdateFrame(object sender, NewFrameEventArgs e)
        {
            PreviewImage.Dispatcher.Invoke(() =>
            {
                PreviewImage.Source = ImageUtils.ToBitmapImage(e.Frame);

            });
            bitmap = (Bitmap)e.Frame.Clone();
        }
        private void OpenWebcam()
        {
            isWebcamOn = true;
            videoCaptureDevice = CameraUtils.getCaptureDevice();

            videoCaptureDevice.NewFrame += UpdateFrame;
            videoCaptureDevice.Start();
        }
        private void CloseWebcam()
        {
            isWebcamOn = false;
            LoadNoCameraImage();
            videoCaptureDevice.SignalToStop();
        }
        private void LoadNoCameraImage()
        {
            string noCameraImgPath = "E:\\FPT STUFFS\\CHUYEN_NGANH\\Semester 7\\PRN221\\Local_Backups\\PhotoShooter\\Resources\\Images\\no-camera.png";
            System.Drawing.Image noCameraImg = System.Drawing.Image.FromFile(noCameraImgPath);
            PreviewImage.Source = ImageUtils.ToBitmapImage((Bitmap)noCameraImg);
        }
        private string[] GetImagePaths()
        {
            return DirectoryUtils.getFilesByExtensions(path, ExtensionConstants.ImageExtensions);
        }

        private void ShowPathNotLoadedMessage()
        {
            DefaultMessageBoxArguments defaultMessageBoxArguments =
               new DefaultMessageBoxArguments(
                   "You haven't selected a Directory. Try again.", "Warning",
                   MessageBoxButton.OK, MessageBoxImage.Error
                );
            MessageBoxUtils.showDefaultMessageBox(defaultMessageBoxArguments);
        }
        private void ShowNoImagesFoundMessage()
        {
            DefaultMessageBoxArguments defaultMessageBoxArguments =
              new DefaultMessageBoxArguments(
                  "No Image is found in this folder.", "Info",
                  MessageBoxButton.OK, MessageBoxImage.Information
               );
            MessageBoxUtils.showDefaultMessageBox(defaultMessageBoxArguments);
        }
        private void ShowWebcamNotEnabledMessage()
        {
            DefaultMessageBoxArguments defaultMessageBoxArguments =
              new DefaultMessageBoxArguments(
                  "The Camera is not turned on.", "Info",
                  MessageBoxButton.OK, MessageBoxImage.Information
               );
            MessageBoxUtils.showDefaultMessageBox(defaultMessageBoxArguments);
        }
        private void ShowSuccessfulUploadMessage()
        {
            DefaultMessageBoxArguments defaultMessageBoxArguments =
             new DefaultMessageBoxArguments(
                 "Successfully synchronized with Firebase.", "Info",
                 MessageBoxButton.OK, MessageBoxImage.Information
              );
            MessageBoxUtils.showDefaultMessageBox(defaultMessageBoxArguments);
        }
        private string GetFullImagePath()
        {
            string bitmapImageName = ImageUtils.getLatestImageName();
            return string.Concat(path, "\\", bitmapImageName);
        }
        private bool isDeleteImage()
        {
            DefaultMessageBoxArguments defaultMessageBoxArguments =
              new DefaultMessageBoxArguments(
                  "You are about to permanently delete this picture. " +
                    "Do you really want to continue?.",
                  "Warning",
                  MessageBoxButton.OKCancel, MessageBoxImage.Warning
               );
            MessageBoxResult result = MessageBoxUtils.getYesNoMessageBoxResult(defaultMessageBoxArguments);
            if (result == MessageBoxResult.Yes) { return true; }
            return false;
        }
        private void OnMainWindowClose(object sender, CancelEventArgs e)
        {
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.SignalToStop();
            }

        }


    }
}
