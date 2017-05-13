using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EpxViewer
{
    public class SpecialCommand
    {
        public static Command ExitAppCommand = new Command()
        {
            ExecuteCommand = o =>
            {
                Application.Current.Shutdown();
            }
        };
    }
}
