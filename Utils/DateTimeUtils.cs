using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoShooter.Utils
{
    public class DateTimeUtils
    {
        public static string getLatestDateTimeString()
        {            
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("ddMMMMyyyy_HHmmss");
            return formattedDateTime;
        }
    }
}
