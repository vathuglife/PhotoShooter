using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FolderBrowser.Models
{
    public class ListViewEntry
    {                
        public BitmapImage Image { get; set; }
        public string PathName{ get; set; }
        public string FileName{ get; set; }

    }
}
