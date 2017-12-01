using NetMQ.Sockets;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Service.ZMQ.Gateway
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
            this.Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text = string.Format("S_FRONTEND_ADDRESS={0}", DefaultSettings.Instance.S_FRONTEND_ADDRESS) + Environment.NewLine;//前端_伺服器
            tbMessage.Text += string.Format("S_BACKEND_ADDRESS={0}", DefaultSettings.Instance.S_BACKEND_ADDRESS) + Environment.NewLine;//後端_前台
            tbMessage.Text += string.Format("XS_FRONTEND_ADDRESS={0}", DefaultSettings.Instance.XS_FRONTEND_ADDRESS) + Environment.NewLine;//前端_伺服器
            tbMessage.Text += string.Format("XS_BACKEND_ADDRESS={0}", DefaultSettings.Instance.XS_BACKEND_ADDRESS) + Environment.NewLine;//後端_前台
            tbMessage.Text += string.Format("PU_FRONTEND_ADDRESS={0}", DefaultSettings.Instance.PU_FRONTEND_ADDRESS) + Environment.NewLine;//前端_伺服器
            tbMessage.Text += string.Format("PU_BACKEND_ADDRESS={0}", DefaultSettings.Instance.PU_BACKEND_ADDRESS) + Environment.NewLine;//後端_前台
            tbMessage.Text += string.Format("RO_FRONTEND_ADDRESS={0}", DefaultSettings.Instance.RO_FRONTEND_ADDRESS) + Environment.NewLine;//前端_伺服器
            tbMessage.Text += string.Format("RO_BACKEND_ADDRESS={0}", DefaultSettings.Instance.RO_BACKEND_ADDRESS);//後端_前台

            BuildProxy_SubPub(DefaultSettings.Instance.S_FRONTEND_ADDRESS, DefaultSettings.Instance.S_BACKEND_ADDRESS);
            BuildProxy_XSubXPub(DefaultSettings.Instance.XS_FRONTEND_ADDRESS, DefaultSettings.Instance.XS_BACKEND_ADDRESS);
            BuildProxy_PushPull(DefaultSettings.Instance.PU_FRONTEND_ADDRESS, DefaultSettings.Instance.PU_BACKEND_ADDRESS);
            BuildProxy_RouterDealer(DefaultSettings.Instance.RO_FRONTEND_ADDRESS, DefaultSettings.Instance.RO_BACKEND_ADDRESS);
        }

        private void BuildProxy_SubPub(string addressFront, string addressBack)
        {
            Task.Factory.StartNew(() =>
            {
                using (var backSocket = new PublisherSocket(addressBack))
                using (var frontSocket = new SubscriberSocket(addressFront))
                {
                    (new NetMQ.Proxy(frontSocket, backSocket)).Start();
                }
            });
        }

        private void BuildProxy_XSubXPub(string addressFront, string addressBack)
        {
            Task.Factory.StartNew(() =>
            {
                using (var backSocket = new XPublisherSocket(addressBack))
                using (var frontSocket = new XSubscriberSocket(addressFront))
                {
                    (new NetMQ.Proxy(frontSocket, backSocket)).Start();
                }
            });
        }

        private void BuildProxy_PushPull(string addressFront, string addressBack)
        {
            Task.Factory.StartNew(() =>
            {
                using (var backSocket = new PushSocket(addressBack))
                using (var frontSocket = new PullSocket(addressFront))
                {
                    (new NetMQ.Proxy(frontSocket, backSocket)).Start();
                }
            });
        }

        private void BuildProxy_RouterDealer(string addressFront, string addressBack)
        {
            Task.Factory.StartNew(() =>
            {
                using (var backSocket = new DealerSocket(addressBack))
                using (var frontSocket = new RouterSocket(addressFront))
                {
                    (new NetMQ.Proxy(frontSocket, backSocket)).Start();
                }
            });
        }
    }
}
