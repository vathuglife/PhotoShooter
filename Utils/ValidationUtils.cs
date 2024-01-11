using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BMICalculator.Utils
{
    public class ValidationUtils
    {
        public static bool isNumeric(string input)
        {
            var pattern = @"^-?\d+(\.\d+)?$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        public static bool isValidFileName(string fileName)
        {
            var pattern = @"^[a-zA-Z0-9_\-]+\.[a-zA-Z0-9]{1,5}$";
            var regex = new Regex(pattern);
            return regex.IsMatch(fileName);
        }
        public static bool isValidFolderName(string folderName)
        {
            var pattern = @"^[a-zA-Z0-9_\-]+$ ";
            var regex = new Regex(pattern);
            return regex.IsMatch(folderName);
        }
    
    }
}
