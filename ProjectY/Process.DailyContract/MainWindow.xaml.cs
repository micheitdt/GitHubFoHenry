using NetMQ;
using NetMQ.Sockets;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;

namespace Process.DailyContract
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
            tbMessage.Text = string.Format("REDIS_IP={0}", DefaultSettings.Instance.REDIS_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_PORT={0}", DefaultSettings.Instance.REDIS_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("REDIS_DB_NUMBER={0}", DefaultSettings.Instance.REDIS_DB_NUMBER) + Environment.NewLine;
            tbMessage.Text += string.Format("SUB_ADDRESS={0}", DefaultSettings.Instance.SUB_ADDRESS);

            Process(DefaultSettings.Instance.REDIS_IP, DefaultSettings.Instance.REDIS_PORT, DefaultSettings.Instance.REDIS_DB_NUMBER, DefaultSettings.Instance.SUB_ADDRESS, DefaultSettings.Instance.SUB_PREFIXES);
        }

        private int GetIndex(byte[] data, int startIndex, byte target)
        {
            if (startIndex < data.Length)
            {
                for (int i = startIndex; i < data.Length; i++)
                {
                    if (data[i] == target)
                        return i;
                }
            }
            return -1;
        }

        private void Process(string redisIP, int redisPort, long redisDbNumber, string subAddress, string[] subPrefixes)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    using (var db = new RedisClient(redisIP, redisPort, null, redisDbNumber))
                    {
                        using (var socket = new SubscriberSocket(subAddress))
                        {
                            Thread.Sleep(10);
                            foreach (var prefix in subPrefixes)
                                socket.Subscribe(prefix);
                            var messages = new List<byte[]>();
                            byte spliterByte = 35;
                            int indexOfSecondSpliter;
                            int indexOfThirdSpliter;
                            int lengthOfDataKey;
                            string hashID;
                            string setID;
                            string listID;
                            var arrayMap = new Dictionary<int, byte[]>();
                            byte[] dataKey;
                            while (true)
                            {
                                socket.ReceiveMultipartBytes(ref messages, 2);
                                if (messages.Count == 2)
                                {
                                    indexOfSecondSpliter = GetIndex(messages[0], GetIndex(messages[0], 0, spliterByte) + 1, spliterByte);
                                    if (indexOfSecondSpliter != -1)
                                    {
                                        hashID = Encoding.ASCII.GetString(messages[0], 0, indexOfSecondSpliter);
                                        setID = string.Format("key_set:{0}", hashID);
                                        listID = string.Format("key_list:{0}", hashID);
                                        indexOfThirdSpliter = GetIndex(messages[0], indexOfSecondSpliter + 1, spliterByte);
                                        if (indexOfThirdSpliter != -1)
                                        {
                                            lengthOfDataKey = indexOfThirdSpliter - indexOfSecondSpliter - 1;
                                            if (arrayMap.TryGetValue(lengthOfDataKey, out dataKey) == false)
                                            {
                                                dataKey = new byte[lengthOfDataKey];
                                                arrayMap.Add(lengthOfDataKey, dataKey);
                                            }
                                            Array.Copy(messages[0], indexOfSecondSpliter + 1, dataKey, 0, lengthOfDataKey);
                                            if (db.SAdd(setID, dataKey) == 1)
                                            {
                                                db.RPush(listID, dataKey);                                                
                                                db.HSet(hashID, dataKey, messages[1]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (TerminatingException)
                {
                }
            });
        }
    }
}
