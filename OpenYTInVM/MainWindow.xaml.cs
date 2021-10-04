using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;
using ReadCSVandCopy;
using System.Configuration;

namespace OpenYTInVM
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
            OpenYoutubeURLsAfterevery15secs();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenYoutubeURLsAfterevery15secs();
        }

        private void OpenYoutubeURLsAfterevery15secs()
        {
            youtubeURLsToOpen.Clear();
            ProcessCopy processCopy = new ProcessCopy();
            List<CopyDetails> copyDetailsList = processCopy.ReadCSVAndPopulateList(".\\HelperFiles\\YT_URLs.csv");
            string txtWaitForEachURLTillURLisComplete, txtTimeDelayAfterOpeningURLinSeconds, txtNumberOfTimeToLoopWatching, txtNumberOfTimesToOpenURLAtOnce, txtShutdownAfterAllWorkIsDone, txtKillBrowserForEachURL;
            ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL);
            //youtubeURLsToOpen.AddRange(System.IO.File.ReadAllLines(".\\HelperFiles\\YT_URLs.txt"));
            _ = int.TryParse(txtNumberOfTimeToLoopWatching, out int lintNumberOfTimeToLoopWatching);

            for (int i = 0; i < lintNumberOfTimeToLoopWatching; i++)
            {
                foreach (var item in copyDetailsList)
                {
                    ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL);
                    _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out int lintTimeDelayAfterOpeningURLinSeconds);

                    //Process.Start(string.Concat("microsoft-edge:", item.URL));
                    _ = int.TryParse(txtNumberOfTimesToOpenURLAtOnce, out int lintNumberOfTimesToOpenURLAtOnce);
                    for (int y = 0; y < lintNumberOfTimesToOpenURLAtOnce; y++)
                    {
                        System.Diagnostics.Process.Start(item.URL);
                        System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
                    }

                    if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
                    {
                        List<string> llstTimeComponents = item.Time.Split(':').ToList();
                        _ = int.TryParse(llstTimeComponents[0], out int lintTimeInMins);
                        _ = int.TryParse(llstTimeComponents[1], out int lintTimeInSeconds);
                        int lintTotalTimeinSeconds = lintTimeInMins * 60 + lintTimeInSeconds;
                        System.Threading.Thread.Sleep(lintTotalTimeinSeconds * 1000);
                    }
                    //_ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out int lintTimeDelayAfterOpeningURLinSeconds);
                    System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);

                    //////Process[] edgeProcessListFull = Process.GetProcesses();
                    //////List<string> llstProcessNames = new List<string>();
                    //////foreach (var item2 in edgeProcessListFull)
                    //////{
                    //////    if(item2.ProcessName.ToUpper().Contains("MICRO") || item2.ProcessName.ToUpper().Contains("EDGE"))
                    //////    {
                    //////        llstProcessNames.Add(item2.ProcessName);
                    //////    }
                    //////}

                    if (string.Compare(txtKillBrowserForEachURL.ToUpper(), "TRUE") == 0)
                    {
                        Process[] edgeProcessList = Process.GetProcessesByName("msedge");

                        foreach (Process theprocess in edgeProcessList)
                        {
                            try
                            {
                                theprocess.Kill();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
                }


            }

            if (string.Compare(txtShutdownAfterAllWorkIsDone.ToUpper(), "TRUE") == 0)
            {
                var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }

        }

        private static void ExtractAppConfigurationValues(out string txtWaitForEachURLTillURLisComplete, out string txtTimeDelayAfterOpeningURLinSeconds, out string txtNumberOfTimeToLoopWatching, out string txtNumberOfTimesToOpenURLAtOnce, out string txtShutdownAfterAllWorkIsDone, out string txtKillBrowserForEachURL)
        {
            txtWaitForEachURLTillURLisComplete = ConfigurationManager.AppSettings["WaitForEachURLTillURLisComplete"];
            txtTimeDelayAfterOpeningURLinSeconds = ConfigurationManager.AppSettings["TimeDelayAfterOpeningURLinSeconds"];
            txtNumberOfTimeToLoopWatching = ConfigurationManager.AppSettings["NumberOfTimeToLoopWatching"];
            txtNumberOfTimesToOpenURLAtOnce = ConfigurationManager.AppSettings["NumberOfTimesToOpenURLAtOnce"];
            txtShutdownAfterAllWorkIsDone = ConfigurationManager.AppSettings["ShutdownAfterAllWorkIsDone"];
            txtKillBrowserForEachURL = ConfigurationManager.AppSettings["KillBrowserForEachURL"];
        }
    }
}
