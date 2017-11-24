using System;
using System.Windows;

namespace Process.MarketDataSnapshot
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = ViewModels.MainViewModel.Instance;
            
            tbMessage.Text += string.Format("REDIS_DB_IP={0}", DefaultSettings.Instance.REDIS_DB_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_PORT={0}", DefaultSettings.Instance.REDIS_DB_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("UDP_IP={0}", DefaultSettings.Instance.UDP_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("UDP_PORT={0}", DefaultSettings.Instance.UDP_PORT);
        }
    }
}
