using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TricorpMigraFacturasTask
{
    public class Log
    {
        public static void LogMessage(string aMessage)
        {
            string lFile = AppDomain.CurrentDomain.BaseDirectory + "\\Log" + DateTime.Now.ToString("ddMMyy") + ".txt";
            FileInfo lFileInfo = new FileInfo(lFile);

            if (File.Exists(lFile))
            {
                do
                {
                    System.Threading.Thread.Sleep(50);
                }
                while (ArchivoEnUso(lFileInfo));
            }

            using (StreamWriter sw = File.AppendText(lFile))
            {
                sw.WriteLine(DateTime.Now.ToString() + " <" + aMessage + ">");
            }
        }

        public static bool ArchivoEnUso(FileInfo aArchivo)
        {
            try
            {
                using (FileStream stream = aArchivo.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
