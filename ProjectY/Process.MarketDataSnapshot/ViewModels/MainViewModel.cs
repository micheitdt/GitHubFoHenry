using ServiceStack.Redis;
using System;
using CommonLibrary;
using NetMQ.Sockets;
using System.Threading;
using System.Collections.Generic;
using NetMQ;
using System.Text;

namespace Process.MarketDataSnapshot.ViewModels
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
            Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "   初始化");
            if (Utility.TestConn(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT))
            {
                //連接redis
                _client = new RedisClient(DefaultSettings.Instance.REDIS_DB_IP, DefaultSettings.Instance.REDIS_DB_PORT);
                Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "   盤中資料方法-錄制期/現貨日盤商品訊息");

                if (string.IsNullOrEmpty(DefaultSettings.Instance.UDP_IP) == false)
                {
                    BuildSubSocket(DefaultSettings.Instance.UDP_IP, DefaultSettings.Instance.UDP_PORT);
                    
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("4#I022#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("4#I082#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("5#I022#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("5#I082#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("2#I022#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("2#I082#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("3#I022#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("3#I082#"));

                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("4#I020#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("4#I080#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("5#I020#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("5#I080#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("2#I020#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("2#I080#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("3#I020#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("3#I080#"));

                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("0#6#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("0#17#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("1#6#"));
                    _socketSub.Subscribe(Encoding.UTF8.GetBytes("1#17#"));

                    Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + "連接並訂閱:4#I022#, 4#I082#, 5#I022#, 5#I082#, 2#I022#, 2#I082#, 3#I022#, 3#I082#, 4#I020#, 4#I080#, 5#I020#, 5#I080#, 2#I020#, 2#I080#, 3#I020#, 3#I080#, 0#6#, 0#17#, 1#6#, 1#17#");
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
                                                ProcessSolvedPacket_TSE(data[1], messages[1]); break;
                                            case "1":
                                                ProcessSolvedPacket_TPEX(data[1], messages[1]); break;
                                            case "2"://期日
                                            case "3"://選日
                                            case "4"://期夜
                                            case "5"://選夜
                                                ProcessSolvedPacket_TAIFEX(data[1], messages[1]); break;
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
        private void ProcessSolvedPacket_TSE(string packetType, byte[] packetData)
        {
            try
            {
                switch (packetType)
                {
                    case "6":
                        {
                            _client.HSet(Parameter.TSE_FORMAT6_HASH_KEY, Utility.ByteGetSubArray(packetData, 10, 6), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 10, 6).Trim();
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上市格式6", symbol));
                            break;
                        }
                    case "17":
                        {
                            _client.HSet(Parameter.TSE_FORMAT17_HASH_KEY, Utility.ByteGetSubArray(packetData, 10, 6), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 10, 6).Trim();
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上市格式17", symbol));
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProcessSolvedPacket_TPEX(string packetType, byte[] packetData)
        {
            try
            {
                switch (packetType)
                {
                    case "6":
                        {
                            _client.HSet(Parameter.TPEX_FORMAT6_HASH_KEY, Utility.ByteGetSubArray(packetData, 10, 6), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 10, 6).Trim();
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上櫃格式6", symbol));
                            break;
                        }
                    case "17":
                        {
                            _client.HSet(Parameter.TPEX_FORMAT17_HASH_KEY, Utility.ByteGetSubArray(packetData, 10, 6), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 10, 6).Trim();
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "上櫃格式17", symbol));
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProcessSolvedPacket_TAIFEX(string packetType, byte[] packetData)
        {
            try
            {
                switch (ConvertToPacketType_TAIFEX(packetType))
                {
                    case "I020":
                        {
                            _client.HSet(Parameter.I020_HASH_KEY, Utility.ByteGetSubArray(packetData, 14, 20), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 14, 20).TrimEnd(' ');
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I020", symbol));
                            break;
                        }
                    case "I080":
                        {
                            _client.HSet(Parameter.I080_HASH_KEY, Utility.ByteGetSubArray(packetData, 14, 20), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 14, 20).TrimEnd(' ');
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I080", symbol));
                            break;
                        }
                    case "I022":
                        {
                            _client.HSet(Parameter.I022_HASH_KEY, Utility.ByteGetSubArray(packetData, 14, 20), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 14, 20).TrimEnd(' ');
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I022", symbol));
                            break;
                        }
                    case "I082":
                        {
                            _client.HSet(Parameter.I082_HASH_KEY, Utility.ByteGetSubArray(packetData, 14, 20), packetData);
                            //string symbol = Encoding.ASCII.GetString(packetData, 14, 20).TrimEnd(' ');
                            //Utility.SaveLog(DateTime.Now.ToString("HH:mm:ss:ttt") + string.Format("Redis新增{0}：{1}", "I082", symbol));
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private string ConvertToPacketType_TAIFEX(string packetType)
        {
            switch (packetType)
            {
                case "21":
                case "51":
                    return "I020";
                case "22":
                case "52":
                    return "I080";
                case "23":
                case "53":
                    return "I140";
                case "24":
                case "54":
                    return "I100";
                case "25":
                case "55":
                    return "I021";
                case "26":
                case "56":
                    return "I023";
                case "27":
                case "57":
                    return "I022";
                case "28":
                case "58":
                    return "I082";
                case "11":
                case "41":
                    return "I010";
                case "12":
                case "42":
                    return "I030";
                case "13":
                case "43":
                    return "I011";
                case "14":
                case "44":
                    return "I050";
                case "15":
                case "45":
                    return "I060";
                case "16":
                case "46":
                    return "I120";
                case "17":
                case "47":
                    return "I130";
                case "18":
                case "48":
                    return "I064";
                case "19":
                    return "I065";
                case "31":
                case "61":
                    return "I070";
                case "32":
                case "62":
                    return "I071";
                case "33":
                case "63":
                    return "I072";
                case "34":
                    return "I073";
                case "71":
                    return "B020";
                case "72":
                    return "B080";
                case "73":
                    return "B021";
                default:
                    return "Unknown";
            }
        }
        #endregion
    }
}
