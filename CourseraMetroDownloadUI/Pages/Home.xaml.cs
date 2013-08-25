using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

        public ObservableCollection<string> ParserList = new ObservableCollection<string>();
        MainWindow mwParentWindow;
        /// <summary>
        /// Constructor, initializes Parser list combobox, add loaded event
        /// </summary>
        public Home()
        {
            InitializeComponent();
            ParserList.Add("html.parser");
            ParserList.Add("html5lib");
            ParserList.Add("lxml");
            ParserList.Add("xml");
            ParserCombo.ItemsSource = ParserList;
            //The default parser is html.parser
            ParserCombo.SelectedIndex = 0;
            this.Loaded += Home_Loaded;
        }
        /// <summary>
        /// Store reference to the MainWindow when the page loads
        /// </summary>
        /// <param name="sender">Home page</param>
        /// <param name="e">RoutedEventArgs</param>
        void Home_Loaded(object sender, RoutedEventArgs e) { mwParentWindow = (MainWindow)Window.GetWindow(this); }
        /// <summary>
        /// Open the OpenFileDialog to capture the path to the coursera-dl.exe or coursera-dl-script.py file to be able to download properly
        /// </summary>
        /// <param name="sender">Find coursera-dl.exe button</param>
        /// <param name="e">RoutedEventArgs</param>
        private void FindCourseDLExeBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "Document"; // Default file name 
            dlg.DefaultExt = ".exe"; // Default file extension 
            dlg.Filter = "Executable Binary (.exe)|*.exe|Python Script (*.py)|*.py"; // Filter files by extension
            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();
            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                mwParentWindow.COURSERA_DOWNLOAD_EXE_PATH = dlg.FileName;
                PathText.Text = mwParentWindow.COURSERA_DOWNLOAD_EXE_PATH;
                PathText.ToolTip = mwParentWindow.COURSERA_DOWNLOAD_EXE_PATH;
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = "C:\\";
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                mwParentWindow.DOWNLOAD_DIRECTORY = folderDialog.SelectedPath;
                downloadPath.Text = mwParentWindow.DOWNLOAD_DIRECTORY;
                downloadPath.ToolTip = mwParentWindow.DOWNLOAD_DIRECTORY;
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
            else { CourseText.Text = string.Empty; }
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
            else { IgnoreExtText.Text = string.Empty; }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (mwParentWindow != null)
            {
                mwParentWindow.ContentSource = new Uri("/Pages/Settings.xaml", UriKind.Relative);
            }
        }

        private void ClassDownloadBtn_Click(object sender, RoutedEventArgs e)
        {
            mwParentWindow.runantc = new System.Diagnostics.Process();
            mwParentWindow.runantc.OutputDataReceived += runantc_OutputDataReceived;
            mwParentWindow.runantc.StartInfo.FileName = "CMD.EXE";
            string args = string.Format(
                @"-u {0} -p {1} -d ""{2}""", 
                email.Text,
                password.Password,
                mwParentWindow.DOWNLOAD_DIRECTORY
            );
            if (FileExtList.Items.Count > 0)
            {
                string csvExts = string.Empty;
                for (int i = 0; i < FileExtList.Items.Count; ++i)
                {
                    if (i == 0) { csvExts = FileExtList.Items.GetItemAt(i).ToString(); }
                    else { csvExts += "," + FileExtList.Items.GetItemAt(i).ToString(); }
                }
                args += string.Format(@" -n ""{0}""", csvExts);
            }
            args += string.Format(" -q {0}", ParserCombo.SelectedValue.ToString());
            if (proxy.Text != string.Empty)
            {
                args += string.Format(@" -x ""{0}""", proxy.Text);
            }
            if (ReverseSelections.IsChecked.GetValueOrDefault())
            {
                args += " --reverse-sections";
            }
            if (TrimPaths.IsChecked.GetValueOrDefault())
            {
                args += " --trim-path";
            }
            if (CourseList.Items.Count > 0)
            {
                string courses = string.Empty;
                foreach (object item in CourseList.Items) { courses += " " + item.ToString(); }
                args += courses;
            }
            mwParentWindow.runantc.StartInfo.Arguments = String.Format(
                "/C {0} {1}",
                mwParentWindow.COURSERA_DOWNLOAD_EXE_PATH,
                args
            );
            mwParentWindow.runantc.StartInfo.CreateNoWindow = true;
            mwParentWindow.runantc.StartInfo.UseShellExecute = false;
            mwParentWindow.runantc.StartInfo.RedirectStandardOutput = true;
            if (mwParentWindow != null)
            {
                mwParentWindow.ContentSource = new Uri("/Pages/Console.xaml", UriKind.Relative);
            }
            mwParentWindow.runantc.Start();
            mwParentWindow.runantc.BeginOutputReadLine();
            if (mwParentWindow.runantc != null)
            {
                try { mwParentWindow.runantc.Close(); }
                catch (Exception ex) { }
            }
            if (mwParentWindow.DataReceivedEvent != null) { mwParentWindow.DataReceivedEvent(Console.NewConsoleLine); }
        }

        private void runantc_OutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (mwParentWindow != null) { if (mwParentWindow.DataReceivedEvent != null) { mwParentWindow.DataReceivedEvent(outLine.Data + "\n"); } }
        }

    }
}
