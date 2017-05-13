using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpxViewer
{
    public class StartInfoVerb
    {
        public const string Default = "";
        public const string RunAs = "runas";
        public const string Open = "open";
        public const string Edit = "edit";
        public const string OpenAsReadOnly = "OpenAsReadOnly";
        public const string Printto = "Printto";
        public const string Print = "Print";
    }
    public class CreateProcess : IDisposable
    {

        System.Diagnostics.Process exep;
        private string errorMessage;

        ~CreateProcess()
        {
            Dispose();
        }

        public void startWindow(string addr, string verb = "")
        {
            if (string.IsNullOrEmpty(addr)) return;
            bool isRightAddr = true;
            if (!System.IO.Directory.Exists(addr))
            {
                if (System.IO.File.Exists(addr))
                {
                    exep.StartInfo.Verb = string.IsNullOrEmpty(verb) ? StartInfoVerb.Open : verb;
                }
                else
                {
                    isRightAddr = false;
                    System.Diagnostics.Debug.WriteLine("Error addr");
                }
            }
            if (!isRightAddr) return;
            //Start program
            exep = new System.Diagnostics.Process();
            exep.StartInfo.FileName = addr;
            exep.EnableRaisingEvents = true;
            try
            {
                exep.Start();
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                errorMessage += "\n" + DateTime.Now.ToString() + "\n" + e.ToString();
            }
            catch (System.InvalidOperationException e)
            {
                errorMessage += "\n" + DateTime.Now.ToString() + "\n" + e.ToString();
            }
            finally
            {
                exep.Dispose();
            }
        }

        public void Dispose()
        {
            if(exep != null) exep.Dispose();
        }
    }
}
