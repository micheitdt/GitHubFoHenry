using CommonLibrary.Model;
using ServiceStack.Redis;
using System;
using System.IO;
using System.Text;
using CommonLibrary;
using System.Collections.Generic;
using System.Threading;
using NetMQ.Sockets;
using NetMQ;
using System.Linq;

namespace Process.BaseMarketData.ViewModels
{
    public class MainViewModel
    {
        private static MainViewModel _instance;
        private static SubscriberSocket _socketSub;
        private static RedisClient _client;

        #region prop
        /// <summary>
        /// 主畫面操作
        /// </summary>
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                }
                return _instance;
            }
        }
        #endregion

        public MainViewModel()
        {
            //讀設定檔
            DefaultSettings.Instance.Initialize();
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "初始化");
            if (Utility.TestConn(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT))
            {
                //連接redis
                _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
                switch (DefaultSettings.Instance.LOAD_MD_MODE)
                {
                    case "0":
                        {
                            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "盤前資料方法-讀檔");
                            LoadTSEData();
                            LoadTPEXData();
                            LoadTAIFEXData();
                            System.Windows.MessageBox.Show("已把檔案轉存Redis資料庫內", "注意", System.Windows.MessageBoxButton.OK);
                            Environment.Exit(0);
                            break;
                        }
                    case "1":
                        {
                            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "盤前資料方法-連接proxy接收訊息");

                            if (string.IsNullOrEmpty(DefaultSettings.Instance.UDP_IP) == false)
                            {
                                BuildSubSocket(DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT);

                                _socketSub.Subscribe(Encoding.UTF8.GetBytes("0#1#"));
                                _socketSub.Subscribe(Encoding.UTF8.GetBytes("1#1#"));
                                _socketSub.Subscribe(Encoding.UTF8.GetBytes("2#11#"));//日期
                                _socketSub.Subscribe(Encoding.UTF8.GetBytes("3#41#"));//日選
                                _socketSub.Subscribe(Encoding.UTF8.GetBytes("4#11#"));//夜期
                                _socketSub.Subscribe(Encoding.UTF8.GetBytes("5#41#"));//夜選

                                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "連接並訂閱:0#1#, 1#1#, 2#I010#, 3#I010#, 4#I010#, 5#I010#");
                            }
                            break;
                        }
                    default:
                        Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "LoadTSEData(): ErrMsg = LOAD_MD_MODE無法辨視.");
                        break;
                }
            }
            else
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "    Redis连接失敗:" + DefaultSettings.Instance.REDIS_DB_IP + ":" + DefaultSettings.Instance.REDIS_DB_PORT + "  無法抓取盤前資料");
            }
        }

        #region func
        private void BuildSubSocket(string ip, int port)
        {
            _socketSub = new SubscriberSocket(string.Format(">tcp://{0}:{1}", ip, port));
            _socketSub.Options.ReceiveHighWatermark = 10000;
            Thread.Sleep(100);
            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    List<byte[]> messages = new List<byte[]>();
                    while (true)
                    {
                        _socketSub.ReceiveMultipartBytes(ref messages, 2);
                        switch (messages.Count)
                        {
                            case 2:
                                var data = Encoding.UTF8.GetString(messages[0]).Split('#');
                                switch (data.Length)
                                {
                                    case 4:
                                        switch (data[0])
                                        {
                                            case "0":
                                                if(data[1] =="1")
                                                {
                                                    SaveRedisTSE(messages[1]);
                                                }
                                                break;
                                            case "1":
                                                if (data[1] == "1")
                                                {
                                                    SaveRedisTPEX(messages[1]);
                                                }
                                                break;
                                            case "2"://期日
                                            case "3"://選日
                                            case "4"://期夜
                                            case "5"://選夜
                                                if (data[1] == "11" || data[1] == "41")
                                                {
                                                    SaveRedisTAIFEX(messages[1]);
                                                }
                                                break;
                                            case "6":
                                            default:
                                                break;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (TerminatingException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ErrorCode);
                }
            });
        }
        /// <summary>
        /// 讀現貨 上市
        /// </summary>
        private void LoadTSEData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TSE_1";
                if (File.Exists(path) == false)
                {
                    return;
                }
                using (var reader = File.OpenText(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        SaveRedisTSE(Encoding.Default.GetBytes(line));
                    }
                }
            }
            catch (Exception err)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("LoadTSEData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void SaveRedisTSE(byte[] data)
        {
            string symbol = Encoding.ASCII.GetString(data, 10, 6).Trim();
            string stkCountMark = Encoding.ASCII.GetString(data, 36, 2).Trim();
            if (symbol == "NE" || symbol == "AL")//NE:開盤-收盤；AL開盤前 資料末筆
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("接收末筆TSE：{0}{1}筆資料", stkCountMark, symbol));
            }
            else
            {
                _client.HSet(Parameter.TSE_FORMAT1_HASH_KEY, Utility.ByteGetSubArray(data, 10, 6), data);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增TSE：{0}", symbol));
            }
        }
        /// <summary>
        /// 讀現貨 上櫃
        /// </summary>
        private void LoadTPEXData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TPEX_1";
                if (File.Exists(path) == false)
                {
                    return;
                }
                using (var reader = File.OpenText(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        SaveRedisTPEX(Encoding.Default.GetBytes(line));
                    }
                }
            }
            catch (Exception err)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void SaveRedisTPEX(byte[] data)
        {
            string symbol = Encoding.ASCII.GetString(data, 10, 6).Trim();
            string stkCountMark = Encoding.ASCII.GetString(data, 36, 2).Trim();
            if (symbol == "NE" || symbol == "AL")//NE:開盤-收盤；AL開盤前 資料末筆
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("接收末筆TPEX：{0}{1}筆資料", stkCountMark, symbol));
            }
            else
            {
                _client.HSet(Parameter.TPEX_FORMAT1_HASH_KEY, Utility.ByteGetSubArray(data, 10, 6), data);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增TPEX：{0}", symbol));
            }
        }
        /// <summary>
        /// 讀期權
        /// </summary>
        private void LoadTAIFEXData()
        {
            try
            {
                string path = System.Environment.CurrentDirectory + "\\TAIFEX_I010";
                if (File.Exists(path) == false)
                {
                    return;
                }
                using (var reader = File.OpenText(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        SaveRedisTAIFEX(Encoding.Default.GetBytes(line));
                    }
                }
            }
            catch (Exception err)
            {
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("LoadData(): ErrMsg = {0}.", err.Message));
            }
        }
        private void SaveRedisTAIFEX(byte[] data)
        {
            _client.HSet(Parameter.I010_HASH_KEY, Utility.ByteGetSubArray(data, 14, 10), data);
            string symbol = Encoding.ASCII.GetString(data, 14, 10).TrimEnd(' ');
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}", symbol));
        }
        #endregion
    }
}
