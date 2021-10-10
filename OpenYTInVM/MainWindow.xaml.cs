﻿using System;
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
            youtubeURLsToOpen.Clear();

            string txtWaitForEachURLTillURLisComplete, txtTimeDelayAfterOpeningURLinSeconds, txtNumberOfTimeToLoopWatching, txtNumberOfTimesToOpenURLAtOnce, txtShutdownAfterAllWorkIsDone, txtKillBrowserForEachURL, txtKillBrowserFirstTimeBeforeLoading, txtMuteSystem, txtAllowMediaAutoPlay, txtDisableMediaAutoPlay, txtTimeDelayBeforeClosingURLAfterCompletioninSeconds;
            ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL, out txtKillBrowserFirstTimeBeforeLoading, out txtMuteSystem, out txtAllowMediaAutoPlay, out txtDisableMediaAutoPlay, out txtTimeDelayBeforeClosingURLAfterCompletioninSeconds);

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
            

            if (string.Compare(txtMuteSystem.ToUpper(), "TRUE") == 0)
            {
                //Mute all audio devices
                SetVolume(0);
            }
            _ = int.TryParse(txtNumberOfTimeToLoopWatching, out int lintNumberOfTimeToLoopWatching);

            for (int i = 0; i < lintNumberOfTimeToLoopWatching; i++)
            {
                foreach (var item in copyDetailsList)
                {
                    //Logging
                    AppendTextToFile("------------------------");
                    AppendTextToFile(item.Title);
                    AppendTextToFile(item.URL);
                    AppendTextToFile(item.Time);
                    AppendTextToFile("------------------------");

                    ExtractAppConfigurationValues(out txtWaitForEachURLTillURLisComplete, out txtTimeDelayAfterOpeningURLinSeconds, out txtNumberOfTimeToLoopWatching, out txtNumberOfTimesToOpenURLAtOnce, out txtShutdownAfterAllWorkIsDone, out txtKillBrowserForEachURL, out txtKillBrowserFirstTimeBeforeLoading, out txtMuteSystem, out txtAllowMediaAutoPlay, out txtDisableMediaAutoPlay, out txtTimeDelayBeforeClosingURLAfterCompletioninSeconds);
                    _ = int.TryParse(txtTimeDelayAfterOpeningURLinSeconds, out int lintTimeDelayAfterOpeningURLinSeconds);

                    //Process.Start(string.Concat("microsoft-edge:", item.URL));
                    _ = int.TryParse(txtNumberOfTimesToOpenURLAtOnce, out int lintNumberOfTimesToOpenURLAtOnce);
                    for (int y = 0; y < lintNumberOfTimesToOpenURLAtOnce; y++)
                    {
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
                        System.Diagnostics.Process.Start(item.URL);
                        System.Threading.Thread.Sleep(lintTimeDelayAfterOpeningURLinSeconds * 1000);

                        LeftMouseClick(384, 384);
                    }

                    if (string.Compare(txtWaitForEachURLTillURLisComplete.ToUpper(), "TRUE") == 0)
                    {
                        List<string> llstTimeComponents = item.Time.Split(':').ToList();
                        _ = int.TryParse(llstTimeComponents[0], out int lintTimeInMins);
                        _ = int.TryParse(llstTimeComponents[1], out int lintTimeInSeconds);
                        _ = int.TryParse(txtTimeDelayBeforeClosingURLAfterCompletioninSeconds, out int lintTimeDelayBeforeClosingURLAfterCompletioninSeconds);

                        int lintTotalTimeinSeconds = lintTimeInMins * 60 + lintTimeInSeconds + lintTimeDelayBeforeClosingURLAfterCompletioninSeconds;
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
                        KillEdgeBrowser();
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

        private static void ExtractAppConfigurationValues(out string txtWaitForEachURLTillURLisComplete, out string txtTimeDelayAfterOpeningURLinSeconds, out string txtNumberOfTimeToLoopWatching, out string txtNumberOfTimesToOpenURLAtOnce, out string txtShutdownAfterAllWorkIsDone, out string txtKillBrowserForEachURL, out string txtKillBrowserFirstTimeBeforeLoading, out string txtMuteSystem, out string txtAllowMediaAutoPlay, out string txtDisableMediaAutoPlay, out string txtTimeDelayBeforeClosingURLAfterCompletioninSeconds)
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
