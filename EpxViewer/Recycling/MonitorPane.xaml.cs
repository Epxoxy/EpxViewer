using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpxViewer
{
    /// <summary>
    /// Interaction logic for MonitorPane.xaml
    /// </summary>
    public partial class MonitorPane : UserControl
    {
        public MonitorPane()
        {
            InitializeComponent();
        }
        
        #region Monitor

        public delegate void CpuUsageHandler(float value);
        public delegate void MemoryUsageHandler(double total, double avail);
        public delegate void DriverUsageHandler(string name, double size, double free);
        public delegate void NetworkIpHandler(string address);

        public void StartMonitor()
        {
            GetCpuUsage();
            GetMemoryUsage();
            GetDriverUsage("C:");
        }

        private void GetCpuUsage()
        {
            Thread tc = new Thread(new ThreadStart(delegate
            {
                PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                while (true)
                {
                    pc.NextValue();
                    Thread.Sleep(1000);

                    this.Dispatcher.Invoke(new CpuUsageHandler(ShowCpuUsage), pc.NextValue());
                }
            }));
            tc.IsBackground = true;
            tc.Start();
        }

        private void ShowCpuUsage(float value)
        {
            cpupro.Value = Math.Round(100f - value, 1);
            hpstate.Text = string.Format("{0}/255", (int)((1f - value / 100 )* 255));
        }

        private void GetMemoryUsage()
        {
            Thread tm = new Thread(new ThreadStart(delegate
            {
                ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("Select TotalPhysicalMemory from Win32_ComputerSystem");
                ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("Select Availablebytes from Win32_PerfRawData_PerfOS_Memory");
                while (true)
                {
                    double total = 0.0;
                    double avail = 0.0;

                    foreach (ManagementObject mo in searcher1.Get())
                    {
                        total += Convert.ToDouble(mo["TotalPhysicalMemory"].ToString());
                    }

                    foreach (ManagementObject mo in searcher2.Get())
                    {
                        avail += Convert.ToDouble(mo["Availablebytes"].ToString());
                    }

                    this.Dispatcher.Invoke(new MemoryUsageHandler(ShowMemoryUsage), total, avail);
                    Thread.Sleep(1000);
                }
            }));
            tm.IsBackground = true;
            tm.Start();
        }

        private void ShowMemoryUsage(double total, double avail)
        {
            rampro.Value = Math.Round(100 * avail / total, 1);
        }

        private void GetDriverUsage(string diskLetter)
        {
            Thread td = new Thread(new ThreadStart(delegate
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("select * from win32_logicalDisk where driveType=3 and deviceid='{0}'", diskLetter));
                while (true)
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        if (mo["Name"].ToString().Trim() == diskLetter)
                        {
                            double size = Convert.ToDouble(mo["Size"].ToString());
                            double free = Convert.ToDouble(mo["FreeSpace"].ToString());
                            this.Dispatcher.Invoke(new DriverUsageHandler(ShowDriverUsage), mo["Name"].ToString(), size, free);
                        }
                    }
                    Thread.Sleep(1000000);
                }/*
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    //There are more attributes you can use.
                    //Check the MSDN link for a complete example.
                    Console.WriteLine(drive.Name);
                    if (drive.IsReady) Console.WriteLine(drive.TotalSize + drive.AvailableFreeSpace + drive.TotalFreeSpace);
                }*/
            }));
            td.IsBackground = true;
            td.Start();
        }

        private void ShowDriverUsage(string name, double size, double free)
        {
            double gs = Math.Round(free / 1024 / 1024 / 1024, 1);
            double fs = Math.Round(size / 1024 / 1024 / 1024, 1);
            diskpro.Value = gs / fs * 100;
        }

        private void monitorMouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void monitorMouseLeave(object sender, MouseEventArgs e)
        {
        }

        #endregion
    }
}
