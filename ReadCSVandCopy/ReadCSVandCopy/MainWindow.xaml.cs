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

namespace ReadCSVandCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcessCopy processCopy = new ProcessCopy();

        public MainWindow()
        {
            InitializeComponent();
            Button_Click(this.btnReadCSVandCopy, new RoutedEventArgs());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<CopyDetails> copyDetailsList = processCopy.ReadCSVAndPopulateList(".\\HelperFiles\\SourceFileFraming_ForDLLs_Notepad+++_Then_TargetFolder.csv");
            copyDetailsList.AddRange(processCopy.ReadCSVAndPopulateList(".\\HelperFiles\\SourceFileFraming_ForATPConfig.csv"));

            processCopy.PerformCopying(copyDetailsList);
        }
    }
}
