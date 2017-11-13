using NetMQ;
using NetMQ.Sockets;
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

namespace ZMQ.Client
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        SubscriberSocket _socketSub1;
        SubscriberSocket _socketSub2;
        SubscriberSocket _socketSub3;
        SubscriberSocket _socketSub4;
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
            tbMessage.Text = string.Format("PUB_IP={0}", DefaultSettings.Instance.PUB_IP, DefaultSettings.Instance.PUB_PORT) + "    ";
            tbMessage.Text += string.Format("XPUB_IP={0}", DefaultSettings.Instance.XPUB_IP, DefaultSettings.Instance.XPUB_PORT) + "    ";
            tbMessage.Text += string.Format("PUSH_IP={0}", DefaultSettings.Instance.PUSH_IP, DefaultSettings.Instance.PUSH_PORT) + "    ";
            tbMessage.Text += string.Format("DEALER_IP={0}", DefaultSettings.Instance.DEALER_IP, DefaultSettings.Instance.DEALER_PORT);
            _socketSub1 = new SubscriberSocket(string.Format(">tcp://{0}:{1}", DefaultSettings.Instance.PUB_IP, DefaultSettings.Instance.PUB_PORT));
            _socketSub2 = new SubscriberSocket(string.Format(">tcp://{0}:{1}", DefaultSettings.Instance.XPUB_IP, DefaultSettings.Instance.XPUB_PORT));
            _socketSub3 = new SubscriberSocket(string.Format(">tcp://{0}:{1}", DefaultSettings.Instance.PUSH_IP, DefaultSettings.Instance.PUSH_PORT));
            _socketSub4 = new SubscriberSocket(string.Format(">tcp://{0}:{1}", DefaultSettings.Instance.DEALER_IP, DefaultSettings.Instance.DEALER_PORT));
        }
        private void pubsub_Click(object sender, RoutedEventArgs e)
        {
            //using (var subSocket = new SubscriberSocket())
            //{
            //    subSocket.Options.ReceiveHighWatermark = 1000;
            //    subSocket.Connect(string.Format("tcp://{0}:{1}", DefaultSettings.Instance.PUB_IP, DefaultSettings.Instance.PUB_PORT));
            //    subSocket.Subscribe("1#1#1#");
            //    Console.WriteLine("Subscriber socket connecting...");
            //    while (true)
            //    {
            //        NetMQ.Msg data = new NetMQ.Msg();
            //        subSocket.TryReceive(ref data, TimeSpan.Parse("10000"));
            //        //下原始用法
            //        //string messageTopicReceived = subSocket.ReceiveFrameString();
            //        //string messageReceived = subSocket.ReceiveFrameString();
            //        Console.WriteLine(data.ToString());
            //    }
            //}
            _socketSub1.Options.ReceiveHighWatermark = 1000;
            System.Threading.Thread.Sleep(100);
            System.Threading.ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    List<byte[]> messages = new List<byte[]>();
                    while (true)
                    {
                        string temp1 = _socketSub1.ReceiveFrameString();
                        string temp2 = _socketSub1.ReceiveFrameString();
                        string temp3 = _socketSub2.ReceiveFrameString();
                        string temp4 = _socketSub2.ReceiveFrameString();
                        string temp5 = _socketSub3.ReceiveFrameString();
                        string temp6 = _socketSub3.ReceiveFrameString();
                        string temp7 = _socketSub4.ReceiveFrameString();
                        string temp8 = _socketSub4.ReceiveFrameString();
                    }
                }
                catch (TerminatingException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ErrorCode);
                }
            });
        }

        private void sub_Click(object sender, RoutedEventArgs e)
        {
            _socketSub1.Subscribe(Encoding.UTF8.GetBytes(""));
            //_socketSub2.Subscribe(Encoding.UTF8.GetBytes(""));
            //_socketSub3.Subscribe(Encoding.UTF8.GetBytes(""));
            //_socketSub4.Subscribe(Encoding.UTF8.GetBytes(""));
            _socketSub1.Subscribe(Encoding.UTF8.GetBytes("1#1#1#"));
            //_socketSub2.Subscribe(Encoding.UTF8.GetBytes("1#1#2#"));
            //_socketSub3.Subscribe(Encoding.UTF8.GetBytes("1#1#3#"));
            //_socketSub4.Subscribe(Encoding.UTF8.GetBytes("1#1#4#"));
        }
    }
}
