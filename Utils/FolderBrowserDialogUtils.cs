using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderBrowser.Utils
{
    public class FolderBrowserDialogUtils
    {
        /*
         * TO DO: Consider refactoring the second return case from NULL 
         * to something else. (Special Case Pattern)
         */
        public static string getPath()
        {            
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            string result= null!;
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                result= folderBrowserDialog.SelectedPath;                
            }
            return result;
        }
    }
}
