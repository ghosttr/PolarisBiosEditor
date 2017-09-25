using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace PolarisB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isBiosLoaded = false;
        private string folderPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "BIOS (.rom)|*.rom|All Files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //fileName = openFileDialog.SafeFileName;
                BiosReader reader = new BiosReader(openFileDialog.FileName);

                isBiosLoaded = true;

                FSLength.Text = reader.FSLength.ToString();
                if (reader.FSLength_Safe == true)
                {
                    FSLength.Foreground = Brushes.Green;
                }
                else
                {
                    FSLength.Foreground = Brushes.Red;
                }
                this.folderPath = System.IO.Path.GetDirectoryName(reader.Filepath);

                OpenExplorer.IsEnabled = true;
                PageBiosInfo_Display(reader);
                
            }
        }

        private void OpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo { Arguments = folderPath, FileName = "explorer.exe" };

                Process.Start(startInfo);
            }
        }
    }
}
