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
        string txtWaitForEachURLTillURLisComplete, txtTimeDelayAfterOpeningURLinSeconds, txtNumberOfTimeToLoopWatching, txtNumberOfTimesToOpenURLAtOnce, txtShutdownAfterAllWorkIsDone, txtKillBrowserForEachURL, txtKillBrowserFirstTimeBeforeLoading, txtMuteSystem, txtAllowMediaAutoPlay, txtDisableMediaAutoPlay, txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, txtMouseClick, txtXPOS, txtYPOS, txtOpenExternalHelperApplications, txtNumberOfMouseClicksToPerform, txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd;
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        
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



        private void OpenYoutubeURLsAfterevery15secs()
        {
            int XPOS, YPOS = 0;

            youtubeURLsToOpen.Clear();

            //--------------STEP 1-----------Open External Helper Apps - NOTEPAD & XPOSYPOS -------------
            ExtractAppConfigurationValues();


            //--------------STEP 2-----------Open External Helper Apps - NOTEPAD & XPOSYPOS -------------
            OpenExternalHelperApplications();

            //--------------STEP 3-----------Set ALLOW MEDIA AUTO PLAY --- ENABLE  OR   DISABLE -------------
            SetAllowMediaAutoPlayRegisterEntries();

            //--------------STEP 4-----------Control Volume -------------
            ControlVolume();

            //--------------STEP 5-----------Process URLs--STARTED-----------
            AppendTextToFile("--------------STEP 5-----------Process URLs--STARTED-----------");
            ProcessCopy processCopy = new ProcessCopy();
            List<CopyDetails> copyDetailsList = processCopy.ReadCSVAndPopulateList(".\\HelperFiles\\YT_URLs.csv");

            //
            //RANJIT - Logic - This is the number of times to loop all the URLs from YT_URLs list
            //
            _ = int.TryParse(txtNumberOfTimeToLoopWatching, out int lintNumberOfTimeToLoopWatching);
            for (int i = 0; i < lintNumberOfTimeToLoopWatching; i++)
            {
                foreach (var item in copyDetailsList)
                {
                    LogCopyDetailsItemDetails(item);

                    //
                    //RANJIT - Logic - Extract cofiguration values for each URL being opened so that config values modified in between the URLs open will take effect.
                    //
                    ExtractAppConfigurationValues();
                    _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out int lintTimeDelayAfterOpeningURLinSeconds);

                    //
                    //RANJIT - Logic - Number of times to open the same URL simultaneously
                    //
                    _ = int.TryParse(txtNumberOfTimesToOpenURLAtOnce, out int lintNumberOfTimesToOpenURLAtOnce);
                    for (int y = 0; y < lintNumberOfTimesToOpenURLAtOnce; y++)
                    {

                        FirstTimeKillTheBrowserBeforeURLLaunch(lintTimeDelayAfterOpeningURLinSeconds);

                        ProcessURL(item, lintTimeDelayAfterOpeningURLinSeconds);

                        MouseClick(out XPOS, out YPOS, lintTimeDelayAfterOpeningURLinSeconds);
                    }

                    WaitForEachURLTillURLisComplete(item, lintTimeDelayAfterOpeningURLinSeconds);

                    //ExtraCodeToLoopThroughProcessesAndFindingTheProcessName();

                    KillTheBrowser(lintTimeDelayAfterOpeningURLinSeconds);
                }
            }
            AppendTextToFile("--------------STEP 5-----------Process URLs--ENDED-----------");

            ShutDownTheSystem();
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



        #region "Public Methods"

        #endregion

        #region "Private - Step Methods"

        #endregion

        #region "Private - Helper Methods"

        #endregion



        private void ShutDownTheSystem()
        {
            AppendTextToFile("--------------STEP 6-----------ShutDownTheSystem--Started----------");

            //
            //RANJIT - Logic - Shutdown the system
            //
            if (string.Compare(txtShutdownAfterAllWorkIsDone.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 6--txtShutdownAfterAllWorkIsDone--Configuration Value is ----TRUE--------------");

                //
                //RANJIT - Logic - If wait for each URL is complete is FALSE then we are waiting 20 minutes.  So that all the URLs can be watched
                //
                if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
                {
                    AppendTextToFile("-----------STEP 6--txtWaitForEachURLTillURLisComplete--Configuration Value is ----TRUE--------------");

                    System.Threading.Thread.Sleep(1 * 1000);
                }
                else
                {
                    AppendTextToFile("-----------STEP 6--txtWaitForEachURLTillURLisComplete--Configuration Value is ----FALSE--------------");

                    System.Threading.Thread.Sleep(20 * 60 * 1000);
                }

                var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }
            AppendTextToFile("--------------STEP 6-----------Process URLs---KillTheBrowser--Ended----------");

        }
        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        private void KillTheBrowser(int lintTimeDelayAfterOpeningURLinSeconds)
        {
            AppendTextToFile("--------------STEP 5-----------Process URLs---KillTheBrowser--Started----------");

            //
            //RANJIT - Logic - Kill all the instances of the edge browser
            //
            if (string.Compare(txtKillBrowserForEachURL.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 5--txtKillBrowserForEachURL--Configuration Value is ----TRUE--------------");

                KillEdgeBrowser();
            }
            System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
            AppendTextToFile("--------------STEP 5-----------Process URLs---KillTheBrowser--Ended----------");

        }

        private static void ExtraCodeToLoopThroughProcessesAndFindingTheProcessName()
        {
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
        }

        private void WaitForEachURLTillURLisComplete(CopyDetails item, int lintTimeDelayAfterOpeningURLinSeconds)
        {
            AppendTextToFile("--------------STEP 5-----------Process URLs---WaitForEachURLTillURLisComplete--Started----------");
            ExtractAppConfigurationValues();
            //
            //RANJIT - Logic - Wait for each URL is complete before opening the next URL
            //
            _ = int.TryParse(txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd, out int lintNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd);

            if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 5--txtWaitForEachURLTillURLisComplete--Configuration Value is ----TRUE--------------");

                List<string> llstTimeComponents = item.Time.Split(':').ToList();
                _ = int.TryParse(llstTimeComponents[0], out int lintTimeInMins);
                _ = int.TryParse(llstTimeComponents[1], out int lintTimeInSeconds);
                _ = int.TryParse(txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out int lintTimeDelayBeforeClosingURLAfterCompletioninSeconds);
                int lintNumberOfSecondsReglintTimeDelayAfterOpeningURLinSeconds = lintNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd * lintTimeDelayAfterOpeningURLinSeconds;


                int lintTotalTimeinSeconds = (lintTimeInMins) * 60 + lintTimeInSeconds + lintTimeDelayBeforeClosingURLAfterCompletioninSeconds + lintNumberOfSecondsReglintTimeDelayAfterOpeningURLinSeconds;
                AppendTextToFile(string.Concat("--------------STEP 5-----------Process URLs---WaitForEachURLTillURLisComplete--Number of seconds being weighted----------", lintTotalTimeinSeconds));

                System.Threading.Thread.Sleep(lintTotalTimeinSeconds * 1000);
            }

            AppendTextToFile("--------------STEP 5-----------Process URLs---WaitForEachURLTillURLisComplete--Ended----------");

        }

        private void MouseClick(out int XPOS, out int YPOS, int lintTimeDelayAfterOpeningURLinSeconds)
        {
            //lintTimeDelayAfterOpeningURLinSeconds = lintNumberOfMouseClicksToPerform
            AppendTextToFile("--------------STEP 5-----------Process URLs---MouseClick--Started----------");
            //
            //RANJIT - Logic - MAIN - Simulate USER clicks
            //  NOTE - for this MOUSE CLICK to work, i assume the remote session must be in focus.
            //
            _ = int.TryParse(txtXPOS, out XPOS);
            _ = int.TryParse(txtYPOS, out YPOS);
            if (string.Compare(txtMouseClick.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 5--txtMouseClick--Configuration Value is ----TRUE--------------");

                _ = int.TryParse(txtNumberOfMouseClicksToPerform, out int lintNumberOfMouseClicksToPerform);
                for (int i = 0; i < lintNumberOfMouseClicksToPerform; i++)
                {
                    LeftMouseClick(XPOS, YPOS);
                    System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
                }
            } 
            AppendTextToFile("--------------STEP 5-----------Process URLs---MouseClick--Ended----------");
        }

        private void ProcessURL(CopyDetails item, int lintTimeDelayAfterOpeningURLinSeconds)
        {
            //lintTimeDelayAfterOpeningURLinSeconds =  1 time
            AppendTextToFile("--------------STEP 5-----------Process URLs---ProcessURL--Started----------");
            //
            //RANJIT - Logic - MAIN - URL that will be opened and played
            //
            System.Diagnostics.Process.Start(item.URL);
            System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);
            AppendTextToFile("--------------STEP 5-----------Process URLs---ProcessURL--Ended----------");

        }

        private void FirstTimeKillTheBrowserBeforeURLLaunch(int lintTimeDelayAfterOpeningURLinSeconds)
        {
            //lintTimeDelayAfterOpeningURLinSeconds = 4 times
            AppendTextToFile("--------------STEP 5-----------Process URLs---FirstTimeKillTheBrowserBeforeURLLaunch--Started----------");
            //
            //RANJIT - Logic - This is to kill the edge browser to skip SETUP steps while opening EDGE browser first time in new system
            //
            if (mblnFirstTimeLaunchingTheBrowser && (string.Compare(txtKillBrowserFirstTimeBeforeLoading.ToUpper(), "TRUE") == 0))
            {
                AppendTextToFile("--------------STEP 5-----------Process URLs---FirstTimeKillTheBrowserBeforeURLLaunch--This is First time &&&----------");
                AppendTextToFile("-----------STEP 5--txtKillBrowserFirstTimeBeforeLoading--Configuration Value is ----TRUE--------------");

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
                AppendTextToFile("--------------STEP 5-----------Process URLs---FirstTimeKillTheBrowserBeforeURLLaunch--This is NOT First time ORRR----------");
                AppendTextToFile("-----------STEP 5--txtKillBrowserFirstTimeBeforeLoading--Configuration Value is ----FALSE--------------");

                mblnFirstTimeLaunchingTheBrowser = false;
            }
            AppendTextToFile("--------------STEP 5-----------Process URLs---FirstTimeKillTheBrowserBeforeURLLaunch--Ended----------");

        }

        private void LogCopyDetailsItemDetails(CopyDetails item)
        {
            AppendTextToFile("--------------STEP 5-----------Process URLs---LogCopyDetailsItemDetails--Started----------");
            //
            //RANJIT - Logic - Logging the URL being played
            //
            AppendTextToFile("------------------------");
            AppendTextToFile(item.Title);
            AppendTextToFile(item.URL);
            AppendTextToFile(item.Time);
            AppendTextToFile("------------------------");
            AppendTextToFile("--------------STEP 5-----------Process URLs---LogCopyDetailsItemDetails--Started----------");

        }

        private void ControlVolume()
        {
            AppendTextToFile("-----------STEP 4--ControlVolume--STARTED--------------");

            //
            //RANJIT - Logic - This is to mute the system.   Currently i am setting volume 1
            //
            if (string.Compare(txtMuteSystem.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 4--txtMuteSystem--Configuration Value is ----TRUE--------------");

                //Mute all audio devices
                SetVolume(1);
            }
            AppendTextToFile("-----------STEP 4--ControlVolume--ENDED--------------");

        }

        private void SetAllowMediaAutoPlayRegisterEntries()
        {
            AppendTextToFile("-----------STEP 3--SetAllowMediaAutoPlayRegisterEntries--STARTED--------------");
            //
            //RANJIT - Logic - This is to ALLOW / DISABLE - Media Auto Play option in the Edge
            //  NOTE - Once the YOUTUBE video is running then this option will not have any significance on the subsequence videos being opened in the new tab
            //              As they keep playing in the new tab though if you HAD DISABLED AUTO MEDIA PLAY as already video is being played in the previous tab page
            //
            if (string.Compare(txtAllowMediaAutoPlay.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 3--txtAllowMediaAutoPlay--Configuration Value is ----TRUE--------------");

                //THIS SEEMS TO BE AN ISSUE WITH AUTOMATIC PLAY OPTION.   LETS INVESTIGATE FURTHER
                string strRegPath = "Allow_media_autoplay_in_Microsoft_Edge.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
                strRegPath = "Enable_media_autoplay_in_Microsoft_Edge_for_all_users.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
            }

            if (string.Compare(txtDisableMediaAutoPlay.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 3--txtDisableMediaAutoPlay--Configuration Value is ----TRUE--------------");

                //THIS SEEMS TO BE AN ISSUE WITH AUTOMATIC PLAY OPTION.   LETS INVESTIGATE FURTHER
                string strRegPath = "Disable_media_autoplay_in_Microsoft_Edge.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
                strRegPath = "Disable_media_autoplay_in_Microsoft_Edge_for_all_users.reg";
                System.Diagnostics.Process.Start("regedit.exe", "/s \"" + strRegPath + "\"");
            }
            AppendTextToFile("-----------STEP 3--SetAllowMediaAutoPlayRegisterEntries--ENDED--------------");

        }

        private void ExtractAppConfigurationValues()
        {
            AppendTextToFile("-----------STEP 1--ExtractAppConfigurationValues--STARTED--------------");
            ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL, out txtKillBrowserFirstTimeBeforeLoading, out txtMuteSystem, out txtAllowMediaAutoPlay, out txtDisableMediaAutoPlay, out txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out txtMouseClick, out txtXPOS, out txtYPOS, out txtOpenExternalHelperApplications, out txtNumberOfMouseClicksToPerform, out txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd);
            AppendTextToFile("-----------STEP 1--ExtractAppConfigurationValues--ENDED--------------");
        }

        private void OpenExternalHelperApplications()
        {
            AppendTextToFile("-----------STEP 2--OpenExternalHelperApplications--STARTED--------------");
            //
            //RANJIT - Logic - This is to open Mofiki's Coordinate Finder.exe, so that we can find the coordinates of the play button
            //
            ProcessStartInfo processStartInfo1 = new ProcessStartInfo(@"C:\Users\admindemo\Desktop\Mofiki's Coordinate Finder.exe");
            Process.Start(processStartInfo1);
            AppendTextToFile("--Opened Coordinates Finder--");
            //
            //RANJIT - Logic - This is to open config file in the remote system so that it is easy to modify the configuration values
            //
            ProcessStartInfo processStartInfo = new ProcessStartInfo("notepad.exe");
            processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            processStartInfo.Arguments = @"C:\Users\admindemo\Desktop\1_OpenYTInVM.exe.config";
            Process.Start(processStartInfo);
            AppendTextToFile("--NOTEPAD--");
            AppendTextToFile("-----------STEP 2--OpenExternalHelperApplications--ENDED--------------");
        }

        private void ExtractAppConfigurationValues(out string txtWaitForEachURLTillURLisComplete, out string txtTimeDelayAfterOpeningURLinSeconds, out string txtNumberOfTimeToLoopWatching, out string txtNumberOfTimesToOpenURLAtOnce, out string txtShutdownAfterAllWorkIsDone, out string txtKillBrowserForEachURL, out string txtKillBrowserFirstTimeBeforeLoading, out string txtMuteSystem, out string txtAllowMediaAutoPlay, out string txtDisableMediaAutoPlay, out string txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out string txtMouseClick, out string txtXPOS, out string txtYPOS, out string txtOpenExternalHelperApplications, out string txtNumberOfMouseClicksToPerform, out string txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd)
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
            txtOpenExternalHelperApplications = ConfigurationManager.AppSettings["OpenExternalHelperApplications"];
            txtNumberOfMouseClicksToPerform = ConfigurationManager.AppSettings["NumberOfMouseClicksToPerform"];
            txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd = ConfigurationManager.AppSettings["NumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd"]; 
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
