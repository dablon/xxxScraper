using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xxx.Data
{
    public class Logs
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static string ToMessageAndCompleteStacktrace(Exception exception)
        {
            Exception e = exception;
            StringBuilder s = new StringBuilder();
            while (e != null)
            {
                s.AppendLine("Exception type: " + e.GetType().FullName);
                s.AppendLine("Message       : " + e.Message);
                s.AppendLine("Stacktrace:" + e.StackTrace);
                s.AppendLine();
                e = e.InnerException;
            }
            return s.ToString();
        }

    }
}