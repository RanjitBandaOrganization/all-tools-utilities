using System.Windows;
using System.IO;
using System;
//using System.Windows.Shapes;

namespace UI.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            dtpSelectedDate.SelectedDate = DateTime.Now.AddDays(-5);

            MoveFiles(firefoxDownloadsPath, DOWNLOADSPATH);
            MoveFiles(operaDownloadsPath, DOWNLOADSPATH);
            CheckNoFileExistsInAFolder(firefoxDownloadsPath);
            CheckNoFileExistsInAFolder(operaDownloadsPath);




        }

        public string DateTimePickerSelectedDate
        {
            get
            {
                DateTime test = (DateTime)dtpSelectedDate.SelectedDate;
                //MessageBox.Show(test.ToString("yyyy-MM-dd"));
                if (test != null)
                {
                    return test.ToString("yyyy-MM-dd");
                }
                else
                {
                    return "No Date is Set";
                }
            }

        }

        public string SelectedDate
        {
            get
            {
                return DateTimePickerSelectedDate;
            }
        }


        #region "BROWSER RELATED INFO"

        string operaExecutablePath = @"C:\Users\rkbanda\AppData\Local\Programs\Opera\74.0.3911.218\opera.exe";
        string firefoxExecutablePath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        string excelExecutablePath = @"C:\Program Files\Microsoft Office\root\Office16\EXCEL.EXE";

        const string FIREFOX = "FIREFOX";
        const string OPERA = "OPERA";

        const string PROTFOLIOURL = @"https://console.zerodha.com/api/download/holdings/portfolio?date=";
        const string PROTFOLIOMIDDLEURL = @"&csrf=";
        const string LEDGERSTARTURL = @"https://console.zerodha.com/api/download/ledger?segment=EQ&from_date=";
        const string LEDGERMIDDLEURL = @"&to_date=";
        const string LEDGERENDURL = @"&format=xlsx";
        const string TRADESTARTURL = @"https://console.zerodha.com/api/download/tradebook?segment=EQ&tradingsymbol=&from_date=";
        const string TRADEMIDDLEURL = @"&to_date=";
        const string TRADEENDURL = @"&format=xlsx";

        #endregion


        #region "FOLDER RELATED INFO"

        const string DOWNLOADSPATH = @"C:\Users\rkbanda\Downloads";

        string firefoxDownloadsPath = System.IO.Path.Combine(DOWNLOADSPATH, FIREFOX);
        string operaDownloadsPath = System.IO.Path.Combine(DOWNLOADSPATH, OPERA);

        const string ZERODHAPATH = @"D:\R\Drives\R.B.T.2\Zerodha";

        #endregion

        #region "ZERODHA"
        const string PROTFOLIONAME = @"holdings-";
        const string LEDGERNAME = @"ledger-";
        const string TRADENAME = @"tradebook-";
        const string FILEEXTENSION = @".xlsx";
        const string REPORTSFOLDERNAME = @"REPORTS";


        #region "ZERODHA - NAGAMANI"
        const string NAGAMANIUSERID = @"DP7866";
        const string NAGAMANINAME = @"NAGAMANI";
        #endregion

        #region "ZERODHA - RAJAPPA"
        const string RAJAPPAUSERID = @"MH3211";
        const string RAJAPPANAME = @"RAJAPPA";

        #endregion
        #endregion


        string userID = string.Empty;
        string userName = string.Empty;
        string browserDownloadPath = string.Empty;
        string browserExecutablePath = string.Empty;

        public string selectedName { 
            get
            {
                if((chkNagamani.IsChecked.HasValue) && chkNagamani.IsChecked.Value)
                {
                    return NAGAMANINAME;
                }
                else if((chkRajappa.IsChecked.HasValue) && chkRajappa.IsChecked.Value)
                {
                    return RAJAPPANAME;
                }
                return string.Empty;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            SetSelectedNameVariables(selectedName);

            URLsToOpen(selectedName);

            Button_Click_2(sender, e);

            Button_Click_1(sender, e);
        }

        private void FilesToOpen(string selectedName)
        {
            string targetPath = Path.Combine(ZERODHAPATH, userName, REPORTSFOLDERNAME, SelectedDate);

            string[] sourcefiles = Directory.GetFiles(targetPath);

            foreach (string sourcefile in sourcefiles)
            {
                System.Diagnostics.Process.Start(excelExecutablePath, sourcefile);
            }


        }


        private void FolderToOpen(string selectedName)
        {
            string targetPath = Path.Combine(ZERODHAPATH, userName, REPORTSFOLDERNAME, SelectedDate);

            System.Diagnostics.Process.Start(@"C:\Windows\explorer.exe", targetPath);
        }

        private void SetSelectedNameVariables(string selectedName)
        {
            if (string.Equals(selectedName, RAJAPPANAME))
            {
                userID = RAJAPPAUSERID;
                userName = RAJAPPANAME;
                browserExecutablePath = firefoxExecutablePath;
                browserDownloadPath = firefoxDownloadsPath;
            }
            else if (string.Equals(selectedName, NAGAMANINAME))
            {
                userID = NAGAMANIUSERID;
                userName = NAGAMANINAME;
                browserExecutablePath = operaExecutablePath;
                browserDownloadPath = operaDownloadsPath;
            }
        }

        private void URLsToOpen(string selectedName)
        {


            OpenBrowserAndDownloadFile(browserExecutablePath, string.Concat(PROTFOLIOURL, SelectedDate, PROTFOLIOMIDDLEURL, txtCRSF.Text), userID, userName, browserDownloadPath, string.Concat(PROTFOLIONAME, userID, FILEEXTENSION));
            OpenBrowserAndDownloadFile(browserExecutablePath, string.Concat(LEDGERSTARTURL, SelectedDate, LEDGERMIDDLEURL, SelectedDate, LEDGERENDURL, PROTFOLIOMIDDLEURL, txtCRSF.Text), userID, userName, browserDownloadPath, string.Concat(LEDGERNAME, userID, FILEEXTENSION));
            OpenBrowserAndDownloadFile(browserExecutablePath, string.Concat(TRADESTARTURL, SelectedDate, TRADEMIDDLEURL, SelectedDate, TRADEENDURL, PROTFOLIOMIDDLEURL, txtCRSF.Text), userID, userName, browserDownloadPath, string.Concat(TRADENAME, userID, FILEEXTENSION));
            SleepForSeconds(5);
            //MoveFilesToReportsIfSuccessful();
        }

        private void MoveFilesToReportsIfSuccessful()
        {
            //MoveFiles(operaDownloadsPath, )

        }

        private void OpenBrowserAndDownloadFile(string browserUrl, string fileToDownload, string userID, string userName, string browserDownloadPath, string downloadedFileName)
        {
            int tries = 0;
            string fileName = downloadedFileName;

            while (!File.Exists(Path.Combine(browserDownloadPath, fileName)) && tries < 5)
            {
                System.Diagnostics.Process.Start(browserUrl, fileToDownload);
                SleepForSeconds(5);
                tries++;
            }
            if (!File.Exists(Path.Combine(browserDownloadPath, fileName)))
            {
                CreateExceptionAndThrow(string.Concat(Path.Combine(browserDownloadPath, fileName), "File doesn't exists"));
            }

            string targetPath = Path.Combine(ZERODHAPATH, userName, REPORTSFOLDERNAME, SelectedDate);
            DirectoryNotExistsThenCreate(targetPath);
            MoveFile(Path.Combine(browserDownloadPath, fileName), targetPath, string.Concat(SelectedDate, "-"));
            //test = System.Diagnostics.Process.Start(browserUrl, fileToDownload);
        }

        private static void SleepForSeconds(int seconds)
        {
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        private void MoveFiles(string sourcePath, string targetPath)
        {
            DirectoryNotExistsThenCreate(targetPath);
            DirectoryNotExistsThenCreate(sourcePath);
            string[] sourcefiles = Directory.GetFiles(sourcePath);

            foreach (string sourcefile in sourcefiles)
            {
                MoveFile(sourcefile, targetPath, string.Empty);
            }
        }

        private static void DirectoryNotExistsThenCreate(string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        private static void MoveFile(string sourcefile, string targetPath, string prefix)
        {
            string fileName = Path.GetFileName(sourcefile);
            string destFile = Path.Combine(targetPath, string.Concat(prefix, fileName));

            File.Move(sourcefile, destFile, true);

            if (!File.Exists(destFile))
            {
                CreateExceptionAndThrow(string.Concat(destFile, "- moving failed"));
            }
        }

        private void CheckNoFileExistsInAFolder(string sourcePath)
        {
            string[] sourcefiles = Directory.GetFiles(sourcePath);

            if (sourcefiles.Length != 0)
            {
                CreateExceptionAndThrow("Folder is not empty");
                return;
            }
        }

        private static void CreateExceptionAndThrow(string exceptionMessage)
        {
            Exception exception = new Exception(exceptionMessage);
            throw exception;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetSelectedNameVariables(selectedName);

            FilesToOpen(selectedName);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetSelectedNameVariables(selectedName);

            FolderToOpen(selectedName);
        }
    }
}
