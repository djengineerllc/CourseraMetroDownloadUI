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
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        //string to create separation for new console output
        public static string NewConsoleLine = "\n\n--------------------------------------------------------------------------\n\n";
        MainWindow mwParentWindow;
        /// <summary>
        /// Set and read the console output from the CONSOLE_OUT variable
        /// </summary>
        private object CONSOLE_OUT
        {
            get { return Output.Content; }
            set { Output.Content = value; }
        }
        /// <summary>
        /// Constructor: Initialize component and set a loaded event
        /// </summary>
        public Console()
        {
            InitializeComponent();
            this.Loaded += Console_Loaded;
        }
        /// <summary>
        /// Once the console page has loaded, get a reference of the MainWindow and subscribe to the DataReceivedEvent
        /// </summary>
        /// <param name="sender">Console page</param>
        /// <param name="e">EventArgs</param>
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
        /// <summary>
        /// Event for getting live data from an executed process (command line output from Home.xaml)
        /// As output is retrieved, the console on the page is updated.
        /// </summary>
        /// <param name="data">line of data streamed from the output of the process running a command prompt command</param>
        private void runantc_OutputDataReceived(string data)
        {
            Output.Dispatcher.BeginInvoke(new System.Windows.Forms.MethodInvoker(() => CONSOLE_OUT += data));
        }
        /// <summary>
        /// Cancel operation button.  This was to cancel the download process, but the executable leaves the python script running.  It has to be killed from task manager.
        /// </summary>
        /// <param name="sender">Cancel button</param>
        /// <param name="e">RoutedEventArgs</param>
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
                    CONSOLE_OUT += "Operations have been aborted! Check task manager and close any python scripts running to cancel the downloading process.";
                }
                catch(Exception ex) { CONSOLE_OUT += Console.NewConsoleLine + "No operations to cancel."; }
            }
            else { CONSOLE_OUT += Console.NewConsoleLine + "No operations to cancel."; }
        }
        /// <summary>
        /// Open the directory to where all the files are set to be downloaded to or are downloading to.
        /// </summary>
        /// <param name="sender">Open directory button</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OpenDlDirBtn_Click(object sender, RoutedEventArgs e)
        {
            if (mwParentWindow.DOWNLOAD_DIRECTORY != string.Empty && System.IO.Directory.Exists(mwParentWindow.DOWNLOAD_DIRECTORY))
            {
                Process.Start(mwParentWindow.DOWNLOAD_DIRECTORY);
            }
        }
        /// <summary>
        /// Clear the screen console
        /// </summary>
        /// <param name="sender">Clear console button</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ClearConsoleBtn_Click(object sender, RoutedEventArgs e) { CONSOLE_OUT = string.Empty; }
        
    }
}
