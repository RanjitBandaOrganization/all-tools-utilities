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
            youtubeURLsToOpen.AddRange(System.IO.File.ReadAllLines(".\\HelperFiles\\YT_URLs.txt"));
            foreach (var item in youtubeURLsToOpen)
            {
                System.Diagnostics.Process.Start(item);
                System.Threading.Thread.Sleep(1000);
            }

            System.Threading.Thread.Sleep(360000);
            
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }
    }
}
