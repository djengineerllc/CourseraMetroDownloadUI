using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CourseraMetroDownloadUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {

        public delegate void ConsoleDataReceivedEventHandler(string data);
        public ConsoleDataReceivedEventHandler DataReceivedEvent;
        public System.Diagnostics.Process runantc = null;
        public string COURSERA_DOWNLOAD_EXE_PATH = string.Empty;
        public string DOWNLOAD_DIRECTORY = string.Empty;
        /// <summary>
        /// MainWindow constructor, start the program's main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
