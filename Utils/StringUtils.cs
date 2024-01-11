using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FolderBrowser.Utils
{
    public class StringUtils
    {              
        public static string removeCarriageReturnAndNewLine(string str)
        {
            return str.TrimEnd(new char[] { '\r', '\n' });
        }
        public static string removeQuotationMarks(string str)
        {
            return str.Replace("\"", "");
        }     
    }
}
