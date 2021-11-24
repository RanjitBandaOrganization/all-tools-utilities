using ReadCSVandCopy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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

namespace OpenYTPrll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> youtubeURLsToOpen = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.OpenYoutubeURLsAfterevery15secs();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.OpenYoutubeURLsAfterevery15secs();
        }

        private void OpenYoutubeURLsAfterevery15secs()
        {
            int num;
            this.youtubeURLsToOpen.Clear();
            List<CopyDetails> list = new ProcessCopy().ReadCSVAndPopulateList(@".\HelperFiles\YT_URLs.csv");
            string str = ConfigurationManager.AppSettings["WaitForEachURLTillURLisComplete"];
            string s = ConfigurationManager.AppSettings["TimeDelayAfterOpeningURLinSeconds"];
            string str4 = ConfigurationManager.AppSettings["ShutdownAfterAllWorkIsDone"];
            string str5 = ConfigurationManager.AppSettings["KillBrowserForEachURL"];
            int.TryParse(ConfigurationManager.AppSettings["NumberOfTimeToLoopWatching"], out num);
            int num2 = 0;
            while (true)
            {
                if (num2 >= num)
                {
                    if (string.Compare(str4.ToUpper(), "TRUE") == 0)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("shutdown", "/s /t 0")
                        {
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };
                        Process.Start(startInfo);
                    }
                    return;
                }
                foreach (CopyDetails details in list)
                {
                    int num3;
                    Process.Start(details.URL);
                    if (string.Compare(str.ToUpper(), "TRUE") == 0)
                    {
                        int num4;
                        int num5;
                        char[] separator = new char[] { ':' };
                        List<string> list2 = details.Time.Split(separator).ToList<string>();
                        int.TryParse(list2[0], out num4);
                        int.TryParse(list2[1], out num5);
                        Thread.Sleep((int)(((num4 * 60) + num5) * 0x3e8));
                    }
                    int.TryParse(s, out num3);
                    Thread.Sleep((int)(num3 * 0x3e8));
                    if (string.Compare(str5.ToUpper(), "TRUE") == 0)
                    {
                        foreach (Process process in Process.GetProcessesByName("msedge"))
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    Thread.Sleep((int)(num3 * 0x3e8));
                }
                num2++;
            }
        }

    }
}
