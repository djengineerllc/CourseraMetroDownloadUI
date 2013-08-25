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
        ModernWindow ParentWindow;
        public Console()
        {
            InitializeComponent();
            this.Loaded += Console_Loaded;
        }

        public object CONSOLE_OUT
        {
            get { return Output.Content; }
            set { Output.Content = value; }
        }

        void Console_Loaded(object sender, RoutedEventArgs e)
        {
            ParentWindow = (ModernWindow)Window.GetWindow(this);
            if (ParentWindow != null)
            {
                MainWindow mw = ParentWindow as MainWindow;
                if (mw != null)
                {
                    mw.DataReceivedEvent += new CourseraMetroDownloadUI.MainWindow.ConsoleDataReceivedEventHandler(runantc_OutputDataReceived);
                }
            }
        }
        void runantc_OutputDataReceived(string data)
        {
            DispatcherOperation dOper = Output.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                try
                {
                    CONSOLE_OUT += data + "\n";
                }
                catch { }
            }));
        }
    }
}
