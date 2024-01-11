using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Runtime;

namespace FolderBrowser.Utils
{
    public class DirectoryUtils
    {

        public static List<string> getFilesNames(List<string> files)
        {
            List<string> names = new List<string>();
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                names.Add(name);
            }
            return names;

        }
        public static List<ImageSource> getFilesIcons(List<string> files)
        {

            List<ImageSource> icons = new List<ImageSource>();
            foreach (string file in files)
            {
                ImageSource icon = IconUtils.GetIcon(file, false);
                icons.Add(icon);
            }

            return icons;
        }
        public static string[] getFilesByExtensions(string path,string[] extensionPatterns)
        {
            return Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(file => extensionPatterns.Any(extension => file.EndsWith(extension))).ToArray();
        }        
        public static string[] getFiles(string path)
        {
            return Directory.GetFiles(path);
        }
        public static string[] getFolders(string path)
        {
            return Directory.GetDirectories(path);
        }
        public static void createNewFile(string path)
        {
            File.Create(path);
        }
        public static void createNewFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
        public static void deleteFile(string path)
        {
            File.Delete(path);
        }
        public static void deleteFolder(string path)
        {
            Directory.Delete(path, false);
        }
        public static void deleteFolderRecursively(string path)
        {
            Directory.Delete(path, true);
        }
        public static void renameFile(string oldPath,string newPath)
        {
            File.Move(oldPath, newPath);
        }
        public static void renameFolder(string oldPath, string newPath)
        {
            Directory.Move(oldPath, newPath);
        }

    }

}
