using NetMQ.Sockets;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Proxy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            this.Title = string.Format("Proxy.{0}", DefaultSettings.Instance.PROXY_NAME);
            tbMessage.Text = string.Format("FRONTEND_ADDR={0}", DefaultSettings.Instance.FRONTEND_ADDRESS) + Environment.NewLine;
            tbMessage.Text += string.Format("BACKEND_ADDR={0}", DefaultSettings.Instance.BACKEND_ADDRESS);
            switch (DefaultSettings.Instance.PROXY_TYPE)
            {
                case "XSUB_XPUB":
                    BuildProxy_XSubXPub(DefaultSettings.Instance.FRONTEND_ADDRESS, DefaultSettings.Instance.BACKEND_ADDRESS);
                    break;
                case "ROUTER_DEALER":
                    BuildProxy_RouterDealer(DefaultSettings.Instance.FRONTEND_ADDRESS, DefaultSettings.Instance.BACKEND_ADDRESS);
                    break;
                default:
                    break;
            }
        }

        private void BuildProxy_XSubXPub(string addressFront, string addressBack)
        {
            Task.Factory.StartNew(() =>
            {
                using (var backSocket = new XPublisherSocket(addressBack))
                using (var frontSocket = new XSubscriberSocket(addressFront))
                {
                    backSocket.Options.ReceiveHighWatermark = 10000;
                    frontSocket.Options.ReceiveHighWatermark = 10000;
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
