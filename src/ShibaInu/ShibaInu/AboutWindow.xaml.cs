using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace ShibaInu
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// About Constructor
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();

            Version.Content = String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        /// <summary>
        /// Handles opening donate link
        /// </summary>
        private void DonateButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.paypal.me/Scobalula");
        }
    }
}
