using Common;
using CommonLibrary;
using NetMQ;
using NetMQ.Sockets;
using ServiceStack.Redis;
using System;
using System.Threading;
using System.Windows;

namespace Adapter.ReplayRedisMD
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string ORIGINAL_MD_HASH_KEY = "OriginalMD";
        public const int SIZE_OF_EXCHANGENAME = 10;
        public const int SIZE_OF_COMMODITYNAME = 10;
        public const int SIZE_OF_CONTRACTDATE = 50;
        RedisClient _client;

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
            tbMessage.Text += string.Format("PUB_ADDRESS={0}", DefaultSettings.Instance.PUB_ADDRESS) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_IP={0}", DefaultSettings.Instance.REDIS_DB_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_PORT={0}", DefaultSettings.Instance.REDIS_DB_PORT);

            SendReplayQuotes(DefaultSettings.Instance.PUB_ADDRESS);
        }

        private void SendReplayQuotes(string PUB_ADDRESS)
        {
            var socket = new PublisherSocket(PUB_ADDRESS);
            var thread = new Thread(() =>
            {
                if (Utility.TestConn(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT))
                {
                    //連接redis
                    _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
                    try
                    {
                        string prefix;
                        foreach (var data in Utility.GetOriginalRedisDB(_client, ORIGINAL_MD_HASH_KEY))
                        {
                            var prefixAry = data.Key.Split('#');
                            prefix = string.Format("{0}#{1}#{2}#", prefixAry[0], prefixAry[1], prefixAry[2]);
                            socket.SendMoreFrame(prefix).SendFrame(data.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });
            thread.Start();
        }
    }
}
