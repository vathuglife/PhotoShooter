using FolderBrowser.Models;
using FolderBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoShooter.Popups.RenameImagePopup
{
    /// <summary>
    /// Interaction logic for RenameImagePopup.xaml
    /// </summary>
    public partial class RenameImagePopup : Window
    {
        private ListViewEntry selectedImage;
        private MainWindow mainWindow;
        public RenameImagePopup(MainWindow mainWindow,ListViewEntry selectedImage)
        {
            InitializeComponent();
            this.selectedImage = selectedImage;
            this.mainWindow = mainWindow;
        }
        private void RenameImage(object sender, RoutedEventArgs e)
        {
            string oldPath = selectedImage.PathName;
            string newPath = GetNewPath();
            DirectoryUtils.renameFile(oldPath, newPath);
            finalize();
        }       
        private string GetNewPath()
        {
            string newImageName = StringUtils.removeCarriageReturnAndNewLine(NewImageNameTextbox.Text);
            string path = selectedImage.PathName;
            string oldImageName = selectedImage.FileName;
            string newPath = path.Replace(oldImageName, newImageName);
            return newPath;
        }
        private void finalize()
        {
            mainWindow.RefreshListView();
            this.Close();
        }
    }
}

