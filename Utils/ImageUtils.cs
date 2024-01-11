using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Media;

namespace PhotoShooter.Utils
{
    public class ImageUtils
    {
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public static BitmapImage ToBitmapImage(string path)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(path);
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public static void saveBitmapImageToFile(Object[] args)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();           
            BitmapImage image = (BitmapImage)(args[0]);
            string filePath = Convert.ToString(args[1]);            
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
        public static ImageSource ConvertToImageSource(string imagePath)
        {
            try
            {                
                BitmapImage bitmapImage = new BitmapImage();                
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                
                ImageSource imageSource = bitmapImage as ImageSource;

                return imageSource;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error converting image to ImageSource: " + ex.Message);
                return null;
            }
        }
        public static string getLatestImageName()
        {
            
            return string.Concat("PhotoShooter_", DateTimeUtils.getLatestDateTimeString(),".jpeg");
        }
        

    }
}
