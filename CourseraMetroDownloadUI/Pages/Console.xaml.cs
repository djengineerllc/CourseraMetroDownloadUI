using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CourseraMetroDownloadUI.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class Console : UserControl
    {

        public static string NewConsoleLine = "\n\n--------------------------------------------------------------------------\n\n";
        MainWindow mwParentWindow;
        private object CONSOLE_OUT
        {
            get { return Output.Content; }
            set { Output.Content = value; }
        }

        public Console()
        {
            InitializeComponent();
            this.Loaded += Console_Loaded;
        }

        private void Console_Loaded(object sender, EventArgs e)
        {
            mwParentWindow = (MainWindow)Window.GetWindow(this);
            if (mwParentWindow != null)
            {
                if (mwParentWindow.DataReceivedEvent == null)
                {
                    mwParentWindow.DataReceivedEvent += new CourseraMetroDownloadUI.MainWindow.ConsoleDataReceivedEventHandler(runantc_OutputDataReceived);
                }
            }
        }

        private void runantc_OutputDataReceived(string data)
        {
            Output.Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(() => CONSOLE_OUT += data));
        }

        private void CancelOpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mwParentWindow.runantc != null)
            {
                try
                {
                    try { mwParentWindow.runantc.CancelOutputRead(); } catch (Exception ex) { }
                    mwParentWindow.runantc.Close();
                    mwParentWindow.runantc.Dispose();
                    try { mwParentWindow.runantc.Kill(); } catch (Exception ex) { }
                    mwParentWindow.runantc = null;
                    CONSOLE_OUT += Console.NewConsoleLine;
                    CONSOLE_OUT += "Operations have been aborted!";
                }
                catch(Exception ex) { CONSOLE_OUT += Console.NewConsoleLine + "No operations to cancel."; }
            }
            else { CONSOLE_OUT += Console.NewConsoleLine + "No operations to cancel."; }
        }

        private void OpenDlDirBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mwParentWindow.DOWNLOAD_DIRECTORY != string.Empty && System.IO.Directory.Exists(mwParentWindow.DOWNLOAD_DIRECTORY))
            {
                Process.Start(mwParentWindow.DOWNLOAD_DIRECTORY);
            }
        }

        private void ClearConsoleBtn_Click(object sender, RoutedEventArgs e)
        {
            CONSOLE_OUT = string.Empty;
        }
        
    }
}
