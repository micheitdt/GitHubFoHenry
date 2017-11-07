using MarketDataApi;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Service.Redis.GlobalMD
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModels.MainViewModel.Instance;

            tbMessage.Text = string.Format("IS_LOAD_FILE={0}", DefaultSettings.Instance.IS_LOAD_FILE) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_IP={0}", DefaultSettings.Instance.REDIS_DB_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_PORT={0}", DefaultSettings.Instance.REDIS_DB_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("UDP_IP={0}", DefaultSettings.Instance.UDP_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("UDP_PORT={0}", DefaultSettings.Instance.UDP_PORT);
        }
    }
}
