using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace kn.web.core.Utils
{
    public class Utils
    {
        private string envVarName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "USERPROFILE" : "HOME";

        public void logError(string fileName, Exception e) {
            string homeDir = Environment.GetEnvironmentVariable(envVarName);
            string tempDir = Path.Combine(homeDir, "temp");

            if (!Directory.Exists(tempDir)) { Directory.CreateDirectory(tempDir); }

            using (FileStream fs = new FileStream(Path.Combine(tempDir, fileName), FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8) { AutoFlush = true })
            {
                string dateError = DateTime.Now.ToString();

                sw.WriteLine("Erro ini!!! --- {0} ---", dateError);
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine("ErrorMessage: {0}", e.Message);
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine("ErrorStackTrace: {0}", e.StackTrace);
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine("ErrorInnerException: {0}", e.InnerException);
                sw.WriteLine("Erro end!!! --- {0} ---", dateError);
                sw.Close();
            }
        }
    }
}
