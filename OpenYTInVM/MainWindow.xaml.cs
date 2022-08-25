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

        public MainWindow()
        {
            InitializeComponent();
            OpenYoutubeURLsAfterevery15secs();
        }

        string txtWaitForEachURLTillURLisComplete, txtTimeDelayAfterOpeningURLinSeconds, txtNumberOfTimeToLoopWatching, txtNumberOfTimesToOpenURLAtOnce, txtShutdownAfterAllWorkIsDone, txtKillBrowserForEachURL, txtKillBrowserFirstTimeBeforeLoading, txtMuteSystem, txtAllowMediaAutoPlay, txtDisableMediaAutoPlay, txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, txtMouseClick, txtXPOS, txtYPOS, txtOpenExternalHelperApplications, txtNumberOfMouseClicksToPerform, txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd, txtTimeInMins = "1", txtTimeInSeconds="1", txtFilePath;
        int lintNumberOfTimesToOpenURLAtOnce, lintNumberOfTimeToLoopWatching, lintTimeDelayAfterOpeningURLinSeconds, XPOS, YPOS, lintNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd, lintTimeInMins, lintTimeInSeconds, lintTimeDelayBeforeClosingURLAfterCompletioninSeconds, lintNumberOfMouseClicksToPerform;
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;


        public List<string> youtubeURLsToOpen = new List<string>();
        bool mblnFirstTimeLaunchingTheBrowser = true;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenYoutubeURLsAfterevery15secs();
        }




        #region "Public Methods"

        #endregion

        #region "Private - Step Methods"


        private void OpenYoutubeURLsAfterevery15secs()
        {
            ExtractAppConfigurationValuesActual();

            youtubeURLsToOpen.Clear();

            //--------------STEP 1-----------Open External Helper Apps - NOTEPAD & XPOSYPOS -------------
            ExtractAppConfigurationValuesActual();


            //--------------STEP 2-----------Open External Helper Apps - NOTEPAD & XPOSYPOS -------------
            //OpenExternalHelperApplications();

            //--------------STEP 3-----------Set ALLOW MEDIA AUTO PLAY --- ENABLE  OR   DISABLE -------------
            //SetAllowMediaAutoPlayRegisterEntries();

            //--------------STEP 4-----------Control Volume -------------
            ControlVolume();

            //--------------STEP 5-----------Process URLs--STARTED-----------
            AppendTextToFile("--------------STEP 5-----------Process URLs--STARTED-----------");
            ProcessCopy processCopy = new ProcessCopy();
            List<CopyDetails> copyDetailsList = null;

            if (File.Exists(".\\HelperFiles\\YT_URLs.csv"))
                copyDetailsList = processCopy.ReadCSVAndPopulateList(".\\HelperFiles\\YT_URLs.csv");
            else if (File.Exists(txtFilePath))
                copyDetailsList = processCopy.ReadCSVAndPopulateList(txtFilePath);
            else
                throw new Exception("YT_URLs.csv file doesn't found");

            //
            //RANJIT - Logic - This is the number of times to loop all the URLs from YT_URLs list
            //
            for (int i = 0; i < lintNumberOfTimeToLoopWatching; i++)
            {
                foreach (var item in copyDetailsList)
                {
                    LogCopyDetailsItemDetails(item);
                    //
                    //RANJIT - Logic - Extract cofiguration values for each URL being opened so that config values modified in between the URLs open will take effect.
                    //
                    ExtractAppConfigurationValuesActual();
                    //
                    //RANJIT - Logic - Number of times to open the same URL simultaneously
                    //
                    for (int y = 0; y < lintNumberOfTimesToOpenURLAtOnce; y++)
                    {

                        //FirstTimeKillTheBrowserBeforeURLLaunch(lintTimeDelayAfterOpeningURLinSeconds);

                        ProcessURL(item, lintTimeDelayAfterOpeningURLinSeconds);

                        MouseClick(XPOS, YPOS, lintTimeDelayAfterOpeningURLinSeconds);
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
                sw.WriteLine(String.Concat(DateTime.Now.ToString(), " -- ", pstrTextToAppend));
            }
        }


        private void OpenExternalHelperApplications()
        {
            ExtractAppConfigurationValuesActual();

            AppendTextToFile("-----------STEP 2--OpenExternalHelperApplications--STARTED--------------");
            //
            //RANJIT - Logic - This is to open Mofiki's Coordinate Finder.exe, so that we can find the coordinates of the play button
            //
            ProcessStartInfo processStartInfo1 = new ProcessStartInfo(@"D:\repos-m\all-tools-utilities\OpenYTInVM\Mofiki's Coordinate Finder.exe");
            Process.Start(processStartInfo1);
            AppendTextToFile("--Opened Coordinates Finder--");
            //
            //RANJIT - Logic - This is to open config file in the remote system so that it is easy to modify the configuration values
            //
            ProcessStartInfo processStartInfo = new ProcessStartInfo("notepad.exe");
            processStartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            processStartInfo.Arguments = @"D:\repos-m\all-tools-utilities\OpenYTInVM\bin\Debug\1_OpenYTInVM.exe.config";
            Process.Start(processStartInfo);
            AppendTextToFile("--NOTEPAD--");
            AppendTextToFile("-----------STEP 2--OpenExternalHelperApplications--ENDED--------------");
        }


        private void SetAllowMediaAutoPlayRegisterEntries()
        {
            ExtractAppConfigurationValuesActual();

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

        private void ShutDownTheSystem()
        {
            ExtractAppConfigurationValuesActual();

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
                    ApplyThreadSleepInSeconds(1);
                }
                else
                {
                    AppendTextToFile("-----------STEP 6--txtWaitForEachURLTillURLisComplete--Configuration Value is ----FALSE--------------");

                    ApplyThreadSleepInSeconds(20 * 60);
                }

                var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }
            AppendTextToFile("--------------STEP 6-----------Process URLs---KillTheBrowser--Ended----------");

        }

        private void ApplyThreadSleepInSeconds(int pstrSeconds)
        {
            AppendTextToFile(String.Concat("-----------SLEEP Applied--", pstrSeconds, " SECONDS--------------"));
            System.Threading.Thread.Sleep(pstrSeconds * 1000);
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


        #endregion

        #region "Private - Helper Methods"

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        private void KillTheBrowser(int lintTimeDelayAfterOpeningURLinSeconds)
        {
            ExtractAppConfigurationValuesActual();
            AppendTextToFile("--------------STEP 5-----------Process URLs---KillTheBrowser--Started----------");

            //
            //RANJIT - Logic - Kill all the instances of the edge browser
            //
            if (string.Compare(txtKillBrowserForEachURL.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 5--txtKillBrowserForEachURL--Configuration Value is ----TRUE--------------");

                KillEdgeBrowser();
            }
            ApplyThreadSleepInSeconds(lintTimeDelayAfterOpeningURLinSeconds);
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
            ExtractAppConfigurationValuesActual();
            //
            //RANJIT - Logic - Wait for each URL is complete before opening the next URL
            //

            if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 5--txtWaitForEachURLTillURLisComplete--Configuration Value is ----TRUE--------------");

                List<string> llstTimeComponents = item.Time.Split(':').ToList();
                txtTimeInMins = llstTimeComponents[0];
                txtTimeInSeconds = llstTimeComponents[1];
                ExtractAppConfigurationValuesActual();
                int lintNumberOfSecondsReglintTimeDelayAfterOpeningURLinSeconds = lintNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd * lintTimeDelayAfterOpeningURLinSeconds;


                int lintTotalTimeinSeconds = (lintTimeInMins) * 60 + lintTimeInSeconds + lintTimeDelayBeforeClosingURLAfterCompletioninSeconds + lintNumberOfSecondsReglintTimeDelayAfterOpeningURLinSeconds;
                AppendTextToFile(string.Concat("--------------STEP 5-----------Process URLs---WaitForEachURLTillURLisComplete--Number of seconds being weighted----------", lintTotalTimeinSeconds));

                ApplyThreadSleepInSeconds(lintTotalTimeinSeconds);
            }

            AppendTextToFile("--------------STEP 5-----------Process URLs---WaitForEachURLTillURLisComplete--Ended----------");

        }

        private void MouseClick(int XPOS, int YPOS, int lintTimeDelayAfterOpeningURLinSeconds)
        {
            //lintTimeDelayAfterOpeningURLinSeconds = lintNumberOfMouseClicksToPerform
            AppendTextToFile("--------------STEP 5-----------Process URLs---MouseClick--Started----------");
            //
            //RANJIT - Logic - MAIN - Simulate USER clicks
            //  NOTE - for this MOUSE CLICK to work, i assume the remote session must be in focus.
            //
            if (string.Compare(txtMouseClick.ToUpper(), "TRUE") == 0)
            {
                AppendTextToFile("-----------STEP 5--txtMouseClick--Configuration Value is ----TRUE--------------");

                for (int i = 0; i < lintNumberOfMouseClicksToPerform; i++)
                {
                    LeftMouseClick(XPOS, YPOS);
                    ApplyThreadSleepInSeconds(lintTimeDelayAfterOpeningURLinSeconds);
                }
            }
            AppendTextToFile("--------------STEP 5-----------Process URLs---MouseClick--Ended----------");
        }

        private void ProcessURL(CopyDetails item, int lintTimeDelayAfterOpeningURLinSeconds)
        {
            ExtractAppConfigurationValuesActual();
            //lintTimeDelayAfterOpeningURLinSeconds =  1 time
            AppendTextToFile("--------------STEP 5-----------Process URLs---ProcessURL--Started----------");
            //
            //RANJIT - Logic - MAIN - URL that will be opened and played
            //
            System.Diagnostics.Process.Start(item.URL);
            ApplyThreadSleepInSeconds(lintTimeDelayAfterOpeningURLinSeconds);
            AppendTextToFile("--------------STEP 5-----------Process URLs---ProcessURL--Ended----------");

        }

        private void FirstTimeKillTheBrowserBeforeURLLaunch(int lintTimeDelayAfterOpeningURLinSeconds)
        {
            ExtractAppConfigurationValuesActual();
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


                    ApplyThreadSleepInSeconds(lintTimeDelayAfterOpeningURLinSeconds);
                    KillEdgeBrowser();
                    ApplyThreadSleepInSeconds(lintTimeDelayAfterOpeningURLinSeconds);
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


        private void ExtractAppConfigurationValuesActual()
        {
            AppendTextToFile("-----------STEP 1--ExtractAppConfigurationValues--STARTED--------------");
            txtFilePath = ConfigurationManager.AppSettings["FilePath"];
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

            AppendTextToFile(txtXPOS);
            AppendTextToFile(txtYPOS);
            //Integer Values Parsing
            _ = int.TryParse(txtNumberOfTimesToOpenURLAtOnce, out lintNumberOfTimesToOpenURLAtOnce);
            _ = int.TryParse(txtNumberOfTimeToLoopWatching, out lintNumberOfTimeToLoopWatching);
            _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out lintTimeDelayAfterOpeningURLinSeconds);
            _ = int.TryParse(txtXPOS, out XPOS);
            _ = int.TryParse(txtYPOS, out YPOS);
            _ = int.TryParse(txtNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd, out lintNumberOfTimeDelayAfterOpeningURLinSecondsToWaitAtEnd);
            _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out lintTimeDelayAfterOpeningURLinSeconds);
            _ = int.TryParse(txtTimeInMins, out lintTimeInMins);
            _ = int.TryParse(txtTimeInSeconds, out lintTimeInSeconds);
            _ = int.TryParse(txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out lintTimeDelayBeforeClosingURLAfterCompletioninSeconds);
            _ = int.TryParse(txtNumberOfMouseClicksToPerform, out lintNumberOfMouseClicksToPerform);

            AppendTextToFile("-----------STEP 1--ExtractAppConfigurationValues--ENDED--------------");
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
                ExtractAppConfigurationValuesActual();

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
                    catch (Exception)
                    {
                        //Do something with exception when an audio endpoint could not be muted
                        //_log.Warn(dev.FriendlyName + " could not be muted with error " + ex);
                    }
                }
            }
            catch (Exception)
            {
                //When something happend that prevent us to iterate through the devices
                //_log.Warn("Could not enumerate devices due to an excepion: " + ex.Message);
            }
        }
        #endregion

    }
}