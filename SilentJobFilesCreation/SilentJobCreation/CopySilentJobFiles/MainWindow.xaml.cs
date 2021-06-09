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

namespace CopySilentJobFiles
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
            for (int i = 0; i < int.Parse(CreateFiles.Text); i++)
            {
                List<string> lstFileText = new List<string>();
                lstFileText = System.IO.File.ReadAllLines("HelperFiles\\SilentJobFolder.xml").ToList();

                for (int y = 0; y < lstFileText.Count; y++)
                {
                    if (lstFileText[y].Contains(SearchText.Text))
                    {
                        lstFileText[y] = lstFileText[y].Replace(SearchText.Text, string.Concat(PreText.Text, i, PostText.Text));
                    }
                }
                System.IO.File.WriteAllLines(String.Concat("SilentJobFolder", i, ".xml"), lstFileText);
            }


            SearchText.Text = "SampleFolder";
            for (int i = 0; i < int.Parse(CreateFiles.Text); i++)
            {
                List<string> lstFileCSVText = new List<string>();
                lstFileCSVText = System.IO.File.ReadAllLines("HelperFiles\\Sample_Folder.csv").ToList();
                
                for (int y = 0; y < lstFileCSVText.Count; y++)
                {
                    if (lstFileCSVText[y].Contains(SearchText.Text))
                    {
                        lstFileCSVText[y] = lstFileCSVText[y].Replace(SearchText.Text, string.Concat(SearchText.Text, i));
                    }
                }
                System.IO.File.WriteAllLines(String.Concat("Sample_Folder", i, ".csv"), lstFileCSVText);
            }
        }
    }
}
