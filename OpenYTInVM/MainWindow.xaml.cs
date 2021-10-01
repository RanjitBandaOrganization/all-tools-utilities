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

            var txtWaitForEachURLTillURLisComplete = ConfigurationManager.AppSettings["WaitForEachURLTillURLisComplete"];
            var txtTimeDelayAfterOpeningURLinSeconds = ConfigurationManager.AppSettings["TimeDelayAfterOpeningURLinSeconds"];
            var txtNumberOfTimeToLoopWatching = ConfigurationManager.AppSettings["NumberOfTimeToLoopWatching"];
            var txtShutdownAfterAllWorkIsDone = ConfigurationManager.AppSettings["ShutdownAfterAllWorkIsDone"];
            //youtubeURLsToOpen.AddRange(System.IO.File.ReadAllLines(".\\HelperFiles\\YT_URLs.txt"));
            _ = int.TryParse(txtNumberOfTimeToLoopWatching, out int lintNumberOfTimeToLoopWatching);

            for (int i = 0; i < lintNumberOfTimeToLoopWatching; i++)
            {
                foreach (var item in copyDetailsList)
                {
                    Process.Start(string.Concat("microsoft-edge:", item.URL));

                    //var process = System.Diagnostics.Process.Start(item.URL);
                    if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
                    {
                        List<string> llstTimeComponents = item.Time.Split(':').ToList();
                        _ = int.TryParse(llstTimeComponents[0], out int lintTimeInMins);
                        _ = int.TryParse(llstTimeComponents[1], out int lintTimeInSeconds);
                        int lintTotalTimeinSeconds = lintTimeInMins * 60 + lintTimeInSeconds;
                        System.Threading.Thread.Sleep(lintTotalTimeinSeconds * 1000);
                    }
                    _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out int lintTimeDelayAfterOpeningURLinSeconds);
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
    }
}
