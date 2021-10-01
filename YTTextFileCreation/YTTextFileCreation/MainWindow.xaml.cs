using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace YTTextFileCreation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string lstrDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string lstrFile = string.Concat(lstrDesktopPath, "\\", txtTextFileName.Text);
            bool test = int.TryParse(txtCount.Text, out int lintCount);
            List<string> URLs = new List<string>();
            for (int i = 0; i < lintCount; i++)
            {
                URLs.Add(txtURL.Text);
            }
            File.WriteAllLinesAsync(lstrFile, URLs);
        }
    }
}
