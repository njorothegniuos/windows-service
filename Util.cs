using Bmat.Tools.DbManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINBANKRECONSERVICE
{
    public class Util
    {
        public static string GetDbConnString()
        {

            string connId = System.Configuration.ConfigurationManager.AppSettings["ConnId"];
            string connKey = System.Configuration.ConfigurationManager.AppSettings["ConnKey"];
            string connData = System.Configuration.ConfigurationManager.AppSettings["ConnData"];

            ConnectionManager conMan = new ConnectionManager();
            string connString = conMan.GetConnectionString(connData, connId, connKey);

            return connString;
        }


        public static void LogError(string userName, Exception ex, bool isError = true)
        {
            try
            {
                var appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string logDir = Path.Combine(appPath, "logs");

                //---- Create Directory if it does not exist              
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                string logFile = Path.Combine(logDir, "ErrorLog.log");
                //--- Delete log if it more than 500Kb
                if (File.Exists(logFile))
                {
                    FileInfo fi = new FileInfo(logFile);
                    if ((fi.Length / 1000) > 500)
                        fi.Delete();
                }
                //--- Create stream writter
                StreamWriter stream = new StreamWriter(logFile, true);
                stream.WriteLine(string.Format("{0}|{1:dd-MMM-yyyy HH:mm:ss}|{2}|{3}",
                    isError ? "ERROR" : "INFOR",
                    DateTime.Now,
                    userName,
                    isError ? ex.ToString() : ex.Message));
                stream.Close();
            }
            catch (Exception e) { }
        }
    }
}
