using NetMQ;
using NetMQ.Sockets;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Service.Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text = string.Format("DEALER_ADDRESS={0}", DefaultSettings.Instance.DEALER_ADDRESS) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_ADDRESS={0}:{1}", DefaultSettings.Instance.REDIS_IP, DefaultSettings.Instance.REDIS_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_NUMBER={0}", DefaultSettings.Instance.REDIS_DB_NUMBER) + Environment.NewLine;
            tbMessage.Text += string.Format("NUMBER_OF_INSTANCES={0}", DefaultSettings.Instance.NUMBER_OF_INSTANCES);
            for (int i = 0; i < DefaultSettings.Instance.NUMBER_OF_INSTANCES; i++)
            {
                RunService(DefaultSettings.Instance.REDIS_IP, DefaultSettings.Instance.REDIS_PORT, DefaultSettings.Instance.REDIS_DB_NUMBER, DefaultSettings.Instance.DEALER_ADDRESS);
            }
        }

        private void RunService(string redisIP, int redisPort, int redisDbNumber, string dealerAddress)
        {
            Task.Factory.StartNew(() =>
            {
                var db = new RedisClient(redisIP, redisPort, null, redisDbNumber);
                using (var socket = new ResponseSocket(dealerAddress))
                {
                    string funcID;
                    string args;
                    while (true)
                    {
                        var msg = socket.ReceiveMultipartStrings();
                        if (msg.Count == 2)
                        {
                            funcID = msg[0];
                            args = msg[1];
                            switch (funcID)
                            {
                                case "GetSnapshot":
                                    GetSnapshot(db, socket, args);
                                    break;
                                case "GetContracts":
                                    GetContracts(db, socket, args);
                                    break;
                                default:
                                    socket.SendFrame("");
                                    break;
                            }
                        }
                        else
                        {
                            socket.SendFrame("");
                        }
                    }
                }
            });
        }

        public void GetSnapshot(RedisClient db, ResponseSocket socket, string args)
        {
            var data = args.Split(',');
            if (data.Length == 2)
            {
                var result = db.HGet(data[0], Encoding.ASCII.GetBytes(data[1]));
                if (result != null)
                {
                    socket.SendFrame(result);
                    return;
                }
            }
            socket.SendFrame(new byte[0]);
        }

        public void GetContracts(RedisClient db, ResponseSocket socket, string args)
        {
            var output = new List<byte[]>();
            var data = args.Split(',');
            if (data.Length == 3)
            {
                int startIndex;
                if (int.TryParse(data[1], out startIndex) == false) { startIndex = -1; }
                int increment;
                if (int.TryParse(data[2], out increment) == false) { increment = 100; }
                if (startIndex >= 0)
                {
                    var key = string.Format("key_list:{0}", data[0]);
                    if (db.ContainsKey(key) == true)
                    {
                        var length = db.LLen(key);
                        if (startIndex < length)
                        {                            
                            byte[][] contracts;
                            if (startIndex + increment >= length)
                            {
                                contracts = db.LRange(key, startIndex, Convert.ToInt32(length) - 1);
                                foreach (var contract in contracts)
                                {
                                    output.Add(db.HGet(data[0], contract));
                                }                                    
                                output.Add(new byte[1] { 1 });
                                socket.SendMultipartBytes(output);
                                return;
                            }
                            else
                            {
                                contracts = db.LRange(key, startIndex, Convert.ToInt32(startIndex + increment) - 1);
                                foreach (var contract in contracts)
                                {
                                    output.Add(db.HGet(data[0], contract));
                                }
                                output.Add(new byte[1] { 0 });
                                socket.SendMultipartBytes(output);
                                return;
                            }
                        }
                    }
                }
            }
            output.Add(new byte[1] { 1 });
            socket.SendMultipartBytes(output);
        }
    }
}
