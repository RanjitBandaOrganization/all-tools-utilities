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
using System.IO;

namespace OpenYTInVM
{

   

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }


        public List<string> youtubeURLsToOpen = new List<string>();
        bool mblnFirstTimeLaunchingTheBrowser = true;

        public MainWindow()
        {
            InitializeComponent();
            OpenYoutubeURLsAfterevery15secs();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenYoutubeURLsAfterevery15secs();
        }

        private void AppendTextToFile(string pstrTextToAppend)
        {
            // Creating a file
            string myfile = @"LogFile.txt";

            // Appending the given texts
            using (StreamWriter sw = File.AppendText(myfile))
            {
                sw.WriteLine(pstrTextToAppend);
            }
        }

        private void OpenYoutubeURLsAfterevery15secs()
        {
            int XPOS, YPOS = 0;
            
            youtubeURLsToOpen.Clear();
            //
            //RANJIT - Logic - This is to open Mofiki's Coordinate Finder.exe, so that we can find the coordinates of the play button
            //
            ProcessStartInfo processStartInfo1 = new ProcessStartInfo(@"C:\Users\admindemo\Desktop\Mofiki's Coordinate Finder.exe");
            Process.Start(processStartInfo1);
            //
            //RANJIT - Logic - This is to open config file in the remote system so that it is easy to modify the configuration values
            //
            ProcessStartInfo processStartInfo = new ProcessStartInfo("notepad.exe");
            processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            processStartInfo.Arguments = @"C:\Users\admindemo\Desktop\1_OpenYTInVM.exe.config";
            Process.Start(processStartInfo);
            

            string txtWaitForEachURLTillURLisComplete, txtTimeDelayAfterOpeningURLinSeconds, txtNumberOfTimeToLoopWatching, txtNumberOfTimesToOpenURLAtOnce, txtShutdownAfterAllWorkIsDone, txtKillBrowserForEachURL, txtKillBrowserFirstTimeBeforeLoading, txtMuteSystem, txtAllowMediaAutoPlay, txtDisableMediaAutoPlay, txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, txtMouseClick, txtXPOS, txtYPOS;
            ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL, out txtKillBrowserFirstTimeBeforeLoading, out txtMuteSystem, out txtAllowMediaAutoPlay, out txtDisableMediaAutoPlay, out txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out txtMouseClick, out txtXPOS, out txtYPOS);

            //
            //RANJIT - Logic - This is to ALLOW / DISABLE - Media Auto Play option in the Edge
            //  NOTE - Once the YOUTUBE video is running then this option will not have any significance on the subsequence videos being opened in the new tab
            //              As they keep playing in the new tab though if you HAD DISABLED AUTO MEDIA PLAY as already video is being played in the previous tab page
            //
            if (string.Compare(txtAllowMediaAutoPlay.ToUpper(), "TRUE") == 0)
            {
                //THIS SEEMS TO BE AN ISSUE WITH AUTOMATIC PLAY OPTION.   LETS INVESTIGATE FURTHER
                string strRegPath = "Allow_media_autoplay_in_Microsoft_Edge.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
                strRegPath = "Enable_media_autoplay_in_Microsoft_Edge_for_all_users.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
            }

            if (string.Compare(txtDisableMediaAutoPlay.ToUpper(), "TRUE") == 0)
            {
                //THIS SEEMS TO BE AN ISSUE WITH AUTOMATIC PLAY OPTION.   LETS INVESTIGATE FURTHER
                string strRegPath = "Disable_media_autoplay_in_Microsoft_Edge.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
                strRegPath = "Disable_media_autoplay_in_Microsoft_Edge_for_all_users.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
            }


            ProcessCopy processCopy = new ProcessCopy();
            List<CopyDetails> copyDetailsList = processCopy.ReadCSVAndPopulateList(".\\HelperFiles\\YT_URLs.csv");

            //
            //RANJIT - Logic - This is to mute the system.   Currently i am setting volume 1
            //
            if (string.Compare(txtMuteSystem.ToUpper(), "TRUE") == 0)
            {
                //Mute all audio devices
                SetVolume(1);
            }

            //
            //RANJIT - Logic - This is the number of times to loop all the URLs from YT_URLs list
            //
            _ = int.TryParse(txtNumberOfTimeToLoopWatching, out int lintNumberOfTimeToLoopWatching);
            for (int i = 0; i < lintNumberOfTimeToLoopWatching; i++)
            {
                foreach (var item in copyDetailsList)
                {
                    //
                    //RANJIT - Logic - Logging the URL being played
                    //
                    AppendTextToFile("------------------------");
                    AppendTextToFile(item.Title);
                    AppendTextToFile(item.URL);
                    AppendTextToFile(item.Time);
                    AppendTextToFile("------------------------");

                    //
                    //RANJIT - Logic - Extract cofiguration values for each URL being opened so that config values modified in between the URLs open will take effect.
                    //
                    ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL, out txtKillBrowserFirstTimeBeforeLoading, out txtMuteSystem, out txtAllowMediaAutoPlay, out txtDisableMediaAutoPlay, out txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out txtMouseClick, out txtXPOS, out txtYPOS);
                    _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out int lintTimeDelayAfterOpeningURLinSeconds);

                    //
                    //RANJIT - Logic - Number of times to open the same URL simultaneously
                    //
                    _ = int.TryParse(txtNumberOfTimesToOpenURLAtOnce, out int lintNumberOfTimesToOpenURLAtOnce);
                    for (int y = 0; y < lintNumberOfTimesToOpenURLAtOnce; y++)
                    {

                        //
                        //RANJIT - Logic - This is to kill the edge browser to skip SETUP steps while opening EDGE browser first time in new system
                        //
                        if (mblnFirstTimeLaunchingTheBrowser && (string.Compare(txtKillBrowserFirstTimeBeforeLoading.ToUpper(), "TRUE") == 0))
                        {
                            for (int z = 0; z < 2; z++)
                            {
                                System.Diagnostics.Process.Start("https://www.youtube.com"); 
                                
                                
                                System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
                                KillEdgeBrowser();
                                System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
                            }
                            mblnFirstTimeLaunchingTheBrowser = false;
                        }
                        else
                        {
                            mblnFirstTimeLaunchingTheBrowser = false;
                        }

                        //
                        //RANJIT - Logic - MAIN - URL that will be opened and played
                        //
                        System.Diagnostics.Process.Start(item.URL);
                        System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);

                        //
                        //RANJIT - Logic - MAIN - Simulate USER clicks
                        //  NOTE - for this MOUSE CLICK to work, i assume the remote session must be in focus.
                        //
                        _ = int.TryParse(txtXPOS, out XPOS);
                        _ = int.TryParse(txtYPOS, out YPOS);
                        if (string.Compare(txtMouseClick.ToUpper(), "TRUE") == 0)
                        {
                            LeftMouseClick(XPOS, YPOS);
                            System.Threading.Thread.Sleep(2 * 1000);
                            LeftMouseClick(XPOS, YPOS);
                            System.Threading.Thread.Sleep(2 * 1000);
                            LeftMouseClick(XPOS, YPOS);
                        }
                        else
                        {
                            LeftMouseClick(XPOS, YPOS);
                            System.Threading.Thread.Sleep(2 * 1000);
                            LeftMouseClick(XPOS, YPOS);
                        }
                    }

                    //
                    //RANJIT - Logic - Wait for each URL is complete before opening the next URL
                    //
                    if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
                    {
                        List<string> llstTimeComponents = item.Time.Split(':').ToList();
                        _ = int.TryParse(llstTimeComponents[0], out int lintTimeInMins);
                        _ = int.TryParse(llstTimeComponents[1], out int lintTimeInSeconds);
                        _ = int.TryParse(txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out int lintTimeDelayBeforeClosingURLAfterCompletioninSeconds);

                        int lintTotalTimeinSeconds = lintTimeInMins * 60 + lintTimeInSeconds + lintTimeDelayBeforeClosingURLAfterCompletioninSeconds;
                        System.Threading.Thread.Sleep(lintTotalTimeinSeconds * 1000);
                    }
                    
                    System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);

                    //
                    //RANJIT - Logic - This is to find out what is the process name of the edge browser.   May be useful in the future if they processname changes
                    //
                    //////Process[] edgeProcessListFull = Process.GetProcesses();
                    //////List<string> llstProcessNames = new List<string>();
                    //////foreach (var item2 in edgeProcessListFull)
                    //////{
                    //////    if(item2.ProcessName.ToUpper().Contains("MICRO") || item2.ProcessName.ToUpper().Contains("EDGE"))
                    //////    {
                    //////        llstProcessNames.Add(item2.ProcessName);
                    //////    }
                    //////}

                    //
                    //RANJIT - Logic - Kill all the instances of the edge browser
                    //
                    if (string.Compare(txtKillBrowserForEachURL.ToUpper(), "TRUE") == 0)
                    {
                        KillEdgeBrowser();
                    }
                    System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
                }
            }

            //
            //RANJIT - Logic - Shutdown the system
            //
            if (string.Compare(txtShutdownAfterAllWorkIsDone.ToUpper(), "TRUE") == 0)
            {
                //
                //RANJIT - Logic - If wait for each URL is complete is FALSE then we are waiting 20 minutes.  So that all the URLs can be watched
                //
                if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
                {
                    System.Threading.Thread.Sleep(1 * 1000);
                }
                else
                {
                    System.Threading.Thread.Sleep(20 * 60 * 1000);
                }

                var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }

        }

        private static void ExtractAppConfigurationValues(out string txtWaitForEachURLTillURLisComplete, out string txtTimeDelayAfterOpeningURLinSeconds, out string txtNumberOfTimeToLoopWatching, out string txtNumberOfTimesToOpenURLAtOnce, out string txtShutdownAfterAllWorkIsDone, out string txtKillBrowserForEachURL, out string txtKillBrowserFirstTimeBeforeLoading, out string txtMuteSystem, out string txtAllowMediaAutoPlay, out string txtDisableMediaAutoPlay, out string txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out string txtMouseClick, out string txtXPOS, out string txtYPOS)
        {
            txtWaitForEachURLTillURLisComplete = ConfigurationManager.AppSettings["WaitForEachURLTillURLisComplete"];
            txtTimeDelayAfterOpeningURLinSeconds = ConfigurationManager.AppSettings["TimeDelayAfterOpeningURLinSeconds"];
            txtNumberOfTimeToLoopWatching = ConfigurationManager.AppSettings["NumberOfTimeToLoopWatching"];
            txtNumberOfTimesToOpenURLAtOnce = ConfigurationManager.AppSettings["NumberOfTimesToOpenURLAtOnce"];
            txtShutdownAfterAllWorkIsDone = ConfigurationManager.AppSettings["ShutdownAfterAllWorkIsDone"];
            txtKillBrowserForEachURL = ConfigurationManager.AppSettings["KillBrowserForEachURL"];
            txtKillBrowserFirstTimeBeforeLoading = ConfigurationManager.AppSettings["KillBrowserFirstTimeBeforeLoading"];
            txtMuteSystem = ConfigurationManager.AppSettings["MuteSystem"];
            txtAllowMediaAutoPlay = ConfigurationManager.AppSettings["AllowMediaAutoPlay"];
            txtDisableMediaAutoPlay = ConfigurationManager.AppSettings["DisableMediaAutoPlay"];
            txtTimeDelayBeforeClosingURLAfterCompletioninSeconds = ConfigurationManager.AppSettings["TimeDelayBeforeClosingURLAfterCompletioninSeconds"];
            txtMouseClick = ConfigurationManager.AppSettings["MouseClick"];
            txtXPOS = ConfigurationManager.AppSettings["XPOS"];
            txtYPOS = ConfigurationManager.AppSettings["YPOS"];
            
        }

        private static void KillEdgeBrowser()
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


        public void SetVolume(int level)
        {
            try
            {
                //Instantiate an Enumerator to find audio devices
                NAudio.CoreAudioApi.MMDeviceEnumerator MMDE = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                //Get all the devices, no matter what condition or status
                NAudio.CoreAudioApi.MMDeviceCollection DevCol = MMDE.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
                //Loop through all devices
                foreach (NAudio.CoreAudioApi.MMDevice dev in DevCol)
                {
                    try
                    {
                        if (dev.State == NAudio.CoreAudioApi.DeviceState.Active)
                        {
                            var newVolume = (float)Math.Max(Math.Min(level, 100), 0) / (float)100;

                            //Set at maximum volume
                            dev.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

                            dev.AudioEndpointVolume.Mute = level == 0;

                            //Get its audio volume
                            //_log.Info("Volume of " + dev.FriendlyName + " is " + dev.AudioEndpointVolume.MasterVolumeLevelScalar.ToString());
                        }
                        else
                        {
                            //_log.Debug("Ignoring device " + dev.FriendlyName + " with state " + dev.State);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Do something with exception when an audio endpoint could not be muted
                        //_log.Warn(dev.FriendlyName + " could not be muted with error " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                //When something happend that prevent us to iterate through the devices
                //_log.Warn("Could not enumerate devices due to an excepion: " + ex.Message);
            }
        }
    }
}
