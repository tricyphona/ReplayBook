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
using System.Windows.Shapes;

namespace Rofl.UI.Main.Views
{
    /// <summary>
    /// Interaction logic for MainWindowContainer.xaml
    /// </summary>
    public partial class MainWindowContainer : Window
    {
        public MainWindowContainer(bool smallMode)
        {
            InitializeComponent();

            if (smallMode)
            {
                MinWidth = 780;
                Width = 780;

                MinHeight = 670;
                Height = 670;
            }
        }

        // Window is loaded and ready to be shown on screen
        private void MainWindowContainer_Loaded(object sender, RoutedEventArgs e)
        {
            //var values = _settingsManager.TemporaryValues;

            //if (values.TryGetDouble("WindowHeight", out double savedHeight) &&
            //    values.TryGetDouble("WindowWidth", out double savedWidth) &&
            //    values.TryGetDouble("WindowLeft", out double savedLeft) &&
            //    values.TryGetDouble("WindowTop", out double savedTop) &&
            //    values.TryGetBool("WindowMaximized", out bool savedMaximized))
            //{
            //    this.Height = savedHeight;
            //    this.Width = savedWidth;
            //    this.Left = savedLeft;
            //    this.Top = savedTop;
            //    if (savedMaximized)
            //    {
            //        this.WindowState = WindowState.Maximized;
            //    }
            //}
        }

        // Window is shown on screen, load the application
        private void MainWindowContainer_ContentRendered(object sender, EventArgs e)
        {

        }
    }
}
