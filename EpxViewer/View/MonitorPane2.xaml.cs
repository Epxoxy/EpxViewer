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
    /// Interaction logic for MonitorPane2.xaml
    /// </summary>
    public partial class MonitorPane2 : UserControl
    {

        public MonitorPane2()
        {
            InitializeComponent();
        }
        
        #region Changed monitor state

        public void updateMonitorState()
        {
            if (isMonitoring) StopMonitoring();
            else OpenMonitoring();
        }

        private void OpenMonitoring()
        {
            isMonitoring = true;
            GetCpuUsage();
            GetMemoryUsage();
            GetRemainRunTime(Environment.TickCount);

            //GetDriverUsage("C:");
            //GetNetworkIp();
        }

        private void StopMonitoring()
        {
            isMonitoring = false;
        }

        private bool isMonitoring;
        #endregion

        private void GetCpuUsage()
        {
            Thread tc = new Thread(new ThreadStart(delegate
            {
                PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                while (isMonitoring)
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
            cpupb.Value = Math.Round(100f - value, 1);
            hpstate.Text = string.Format("{0}/255", (int)((1f - value / 100) * 255));
        }

        private void GetMemoryUsage()
        {
            Thread tm = new Thread(new ThreadStart(delegate
            {
                ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("Select TotalPhysicalMemory from Win32_ComputerSystem");
                ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("Select Availablebytes from Win32_PerfRawData_PerfOS_Memory");
                while (isMonitoring)
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
            rampb.Value = Math.Round(100 * avail / total, 1);
        }
        
        private void GetRemainRunTime(int tickCount)
        {
            Thread tn = new Thread(new ThreadStart(delegate
            {
                int plantotal = 43200000;
                double radio = 0;
                while (isMonitoring)
                {
                    radio = (plantotal - tickCount) / (double)plantotal;
                    this.Dispatcher.Invoke(new PlanTimeHandler(ShowPlanTime), radio);

                    Thread.Sleep(2000);
                }
            }));
            tn.IsBackground = true;
            tn.Start();
        }

        private void ShowPlanTime(double radio)
        {
            procpb.Value = radio * 100;
            if (IsOnBattery) chargeState.Visibility = Visibility.Visible;
            else chargeState.Visibility = Visibility.Collapsed;
            //System.Diagnostics.Debug.WriteLine(radio);
        }
        
        private void OnBarMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == addCanvas0 || sender == barCanvas0)
            {
                statePath.Opacity = 0.8;
                bg0.Opacity = 0.8;
            }
            else (sender == barGrid1 ? bg1 : bg2).Opacity = 0.8;
        }

        private void OnBarMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender == addCanvas0 || sender == barCanvas0)
            {
                statePath.Opacity = 0.6;
                bg0.Opacity = 0.6;
            }
            else (sender == barGrid1 ? bg1 : bg2).Opacity = 0.6;
        }
        
        #region Not useable now

        private void GetDriverUsage(string diskLetter)
        {
            Thread td = new Thread(new ThreadStart(delegate
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("select * from win32_logicalDisk where driveType=3 and deviceid='{0}'", diskLetter));
                while (isMonitoring)
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
                    Thread.Sleep(10000);
                }
            }));
            td.IsBackground = true;
            td.Start();
        }

        private void ShowDriverUsage(string name, double size, double free)
        {
            double gs = Math.Round(free / 1024 / 1024 / 1024, 1);
            double fs = Math.Round(size / 1024 / 1024 / 1024, 1);
            //procpb.Value = gs / fs * 100;
        }

        private void GetNetworkIp()
        {
            Thread tn = new Thread(new ThreadStart(delegate
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_NetworkAdapterConfiguration where IPEnabled=TRUE");
                do
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        string[] arr = mo["IPAddress"] as string[];
                        this.Dispatcher.Invoke(new NetworkIpHandler(ShowNetworkIp), arr[0]);
                    }
                    Thread.Sleep(1000);
                } while (false);
            }));
            tn.IsBackground = true;
            tn.Start();
        }

        private void ShowNetworkIp(string address)
        {
        }

        private string GetBatteryStatus()
        {
            ManagementScope scope = new ManagementScope("//./root/cimv2");
            SelectQuery query = new SelectQuery("Select BatteryStatus From Win32_Battery");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        PropertyData pData = mObj.Properties["BatteryStatus"];
                        switch ((Int16)pData.Value)
                        {
                            //...
                            case 2: return "Not Charging";
                            case 3: return "Fully Charged";
                            case 4: return "Low";
                            case 5: return "Critical";
                                //...
                        }
                    }
                }
            }
            return string.Empty;
        }

        #endregion

        #region Delegate Member

        public delegate void CpuUsageHandler(float value);
        public delegate void MemoryUsageHandler(double total, double avail);
        public delegate void DriverUsageHandler(string name, double size, double free);
        public delegate void NetworkIpHandler(string address);
        public delegate void PlanTimeHandler(double radio);

        #endregion

        #region Member

        private bool IsOnBattery => System.Windows.Forms.SystemInformation.PowerStatus.PowerLineStatus == System.Windows.Forms.PowerLineStatus.Online;

        #endregion

    }
}
