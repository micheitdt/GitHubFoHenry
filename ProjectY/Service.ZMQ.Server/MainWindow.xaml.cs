using CommonLibrary;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Service.ZMQ.Server
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
            tbMessage.Text = string.Format("PUB_ADDRESS={0}", DefaultSettings.Instance.PUB_ADDRESS) + Environment.NewLine;
            tbMessage.Text += string.Format("XPUB_ADDRESS={0}", DefaultSettings.Instance.XPUB_ADDRESS) + Environment.NewLine;
            tbMessage.Text += string.Format("PUSH_ADDRESS={0}", DefaultSettings.Instance.PUSH_ADDRESS) + Environment.NewLine;
            tbMessage.Text += string.Format("DEALER_ADDRESS={0}", DefaultSettings.Instance.DEALER_ADDRESS);
            
            SendSubMessage(DefaultSettings.Instance.PUB_ADDRESS);
            //SendXSubMessage(DefaultSettings.Instance.XPUB_ADDRESS);
            //SendPushMessage(DefaultSettings.Instance.PUSH_ADDRESS);
            //SendDealerMessage(DefaultSettings.Instance.DEALER_ADDRESS);
        }
        /// <summary>
        /// 單一發送者
        /// </summary>
        private void SendSubMessage(string PUB_ADDRESS)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
                    using (var pubSocket = new PublisherSocket(PUB_ADDRESS))
                    {
                        pubSocket.Options.SendHighWatermark = 1000;
                        //pubSocket.Bind(PUB_ADDRESS);
                        
                        for (int i = 0; i <= 100; i++)
                        {
                            byte[] data = Encoding.ASCII.GetBytes("1#1#1#_PUB_ADDRESS");
                            pubSocket.SendMoreFrame("1#1#1#").SendFrame(data);

                            _logger.Debug(i + "1#1#1#" + "1#1#1#_PUB_ADDRESS");
                            Thread.Sleep(3000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

            thread.Start();
        }
        /// <summary>
        /// 可多發送者
        /// </summary>
        private void SendXSubMessage(string XPUB_ADDRESS)
        {
            var thread = new Thread(() =>
            {
                try
            {
                using (var pubSocket = new PublisherSocket(XPUB_ADDRESS))
                {
                    pubSocket.Options.SendHighWatermark = 1000;
                    //pubSocket.Bind(XPUB_ADDRESS);

                    for (int i = 0; i <= 100; i++)
                        {
                            byte[] data = Encoding.ASCII.GetBytes("1#1#2#_XPUB_ADDRESS");
                            pubSocket.SendMoreFrame("1#1#2#").SendFrame(data);
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                }
            });

            thread.Start();
        }
        /// <summary>
        /// 單一推送者可多者_分流
        /// </summary>
        private void SendPushMessage(string PUSH_ADDRESS)
        {
            var thread = new Thread(() =>
            {
                try
            {
                using (var pubSocket = new PublisherSocket(PUSH_ADDRESS))
                {
                    pubSocket.Options.SendHighWatermark = 1000;
                   // pubSocket.Bind(PUSH_ADDRESS);
                    for (int i = 0; i <= 100; i++)
                        {
                            byte[] data = Encoding.ASCII.GetBytes("1#1#3#_PUSH_ADDRESS");
                            pubSocket.SendMoreFrame("1#1#3#").SendFrame(data);
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        });

            thread.Start();
        }
        /// <summary>
        /// 多發送者非同步
        /// </summary>
        private void SendDealerMessage(string DEALER_ADDRESS)
        {
            var thread = new Thread(() =>
            {
                try
            {
                using (var pubSocket = new PublisherSocket(DEALER_ADDRESS))
                {
                    pubSocket.Options.SendHighWatermark = 1000;
                    //pubSocket.Bind(DEALER_ADDRESS);
                    for (int i = 0; i <= 100; i++)
                        {
                            byte[] data = Encoding.ASCII.GetBytes("1#1#4#_DEALER_ADDRESS");
                            pubSocket.SendMoreFrame("1#1#4#").SendFrame(data);
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                }
            });

            thread.Start();
        }
    }
}
