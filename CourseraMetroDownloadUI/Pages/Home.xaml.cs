using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseraMetroDownloadUI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : System.Windows.Controls.UserControl
    {
        public string COURSERA_DOWNLOAD_EXE_PATH = string.Empty;
        public string DOWNLOAD_DIRECTORY = string.Empty;
        public ObservableCollection<string> ProxyList = new ObservableCollection<string>();
        ModernWindow ParentWindow;
        public Home()
        {
            InitializeComponent();
            ProxyList.Add("lxml");
            ProxyList.Add("xml");
            ProxyList.Add("html.parser");
            ProxyList.Add("html5lib");
            ParserCombo.ItemsSource = ProxyList;
            ParserCombo.SelectedIndex = 0;
            this.Loaded += Home_Loaded;
        }

        void Home_Loaded(object sender, RoutedEventArgs e)
        {
            ParentWindow = (ModernWindow)Window.GetWindow(this);
        }

        private void FindCourseDLExeBtn_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name 
            dlg.DefaultExt = ".exe"; // Default file extension 
            dlg.Filter = "Executable Binary (.exe)|*.exe"; // Filter files by extension
            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();
            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                COURSERA_DOWNLOAD_EXE_PATH = dlg.FileName;
                PathText.Text = COURSERA_DOWNLOAD_EXE_PATH;
                PathText.ToolTip = COURSERA_DOWNLOAD_EXE_PATH;
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = "C:\\";
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                DOWNLOAD_DIRECTORY = folderDialog.SelectedPath;
                downloadPath.Text = DOWNLOAD_DIRECTORY;
                downloadPath.ToolTip = DOWNLOAD_DIRECTORY;
            }
        }

        private void AddCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CourseText.Text != string.Empty)
            {
                CourseList.Items.Add(CourseText.Text);
                CourseText.Text = string.Empty;
            }
        }

        private void DeleteCourseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CourseList.SelectedItems.Count > 0)
            {
                CourseList.Items.RemoveAt(CourseList.Items.IndexOf(CourseList.SelectedItem));
            }
            else
            {
                CourseText.Text = string.Empty;
            }
        }

        private void IgnoreFileExtBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IgnoreExtText.Text != string.Empty)
            {
                FileExtList.Items.Add(IgnoreExtText.Text);
                IgnoreExtText.Text = string.Empty;
            }
        }

        private void DeleteFileExtBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FileExtList.SelectedItems.Count > 0)
            {
                FileExtList.Items.RemoveAt(FileExtList.Items.IndexOf(FileExtList.SelectedItem));
            }
            else
            {
                IgnoreExtText.Text = string.Empty;
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParentWindow != null)
            {
                ParentWindow.ContentSource = new Uri("/Pages/Settings.xaml", UriKind.Relative);
            }
        }

        private void ClassDownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            using (System.Diagnostics.Process runantc = new System.Diagnostics.Process())
            {
                runantc.StartInfo.FileName = "CMD.EXE";
                runantc.StartInfo.Arguments = "/C " + COURSERA_DOWNLOAD_EXE_PATH + " --help";
                runantc.StartInfo.UseShellExecute = false;
                runantc.StartInfo.RedirectStandardOutput = true;
                runantc.StartInfo.RedirectStandardError = true;
                runantc.OutputDataReceived += runantc_OutputDataReceived;
                if (ParentWindow != null)
                {
                    ParentWindow.ContentSource = new Uri("/Pages/Console.xaml", UriKind.Relative);
                }
                runantc.Start();
                runantc.BeginOutputReadLine();
                runantc.Close();
            }
        }

        void runantc_OutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            MainWindow mw = (ParentWindow as MainWindow);
            if (mw != null) { if (mw.DataReceivedEvent != null) { mw.DataReceivedEvent(outLine.Data); } }
        }
    }
}
