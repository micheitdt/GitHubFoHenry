using NetMQ;
using NetMQ.Sockets;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using CommonLibrary;

namespace MarketDataApi
{
    public class MarketDataApi
    {
        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger

        private SubscriberSocket _socketSub;
        public MarketDataApi(string subIP, int subPort)
        {
            if (string.IsNullOrEmpty(subIP) == false)
                BuildSubSocket(subIP, subPort);
        }

        public void Sub(AdapterCode adapterCode, string packetType, string packetTypeSection)
        {
            Sub(string.Format("{0}#{1}#{2}#", ((int)adapterCode).ToString(), GetPacketType(adapterCode, packetType), packetTypeSection));
        }

        public void Sub(AdapterCode adapterCode, string packetType)
        {
            Sub(string.Format("{0}#{1}#", ((int)adapterCode).ToString(), GetPacketType(adapterCode, packetType)));
        }

        public void UnSub(AdapterCode adapterCode, string packetType, string packetTypeSection)
        {
            UnSub(string.Format("{0}#{1}#{2}#", ((int)adapterCode).ToString(), GetPacketType(adapterCode, packetType), packetTypeSection));
        }

        public void UnSub(AdapterCode adapterCode, string packetType)
        {
            UnSub(string.Format("{0}#{1}#", ((int)adapterCode).ToString(), GetPacketType(adapterCode, packetType)));
        }

        //public bool GetTaifexSymbolInfo(string redisip,int redisport, ref Dictionary<string, CommonLibrary.Model.PacketTAIFEX.I010> retdata )
        //{
        //    if (Utility.TestConn(redisip, redisport))
        //    {
        //        ServiceStack.Redis.RedisClient _redisClient = new ServiceStack.Redis.RedisClient(redisip, redisport);
                
        //        if (_redisClient.GetHashKeys(Parameter.TAIFEX_HASH_KEY).Count == 0)
        //            return false;
        //        retdata = new Dictionary<string, CommonLibrary.Model.PacketTAIFEX.I010>(_redisClient.GetAll<CommonLibrary.Model.PacketTAIFEX.I010>(_redisClient.GetHashKeys(Parameter.TAIFEX_HASH_KEY)));
        //        return true;
        //    }
        //    else
        //    {
        //        _logger.Debug(string.Format("GetTaifexSymbolInfo(): Redis連接失敗:(0):{1} Data = {2}.", redisip, redisport));
        //        return false;
        //    }
        //}

        private void Sub(string prefix)
        {
            _socketSub.Subscribe(Encoding.UTF8.GetBytes(prefix));
        }

        private void UnSub(string prefix)
        {
            _socketSub.Unsubscribe(Encoding.UTF8.GetBytes(prefix));
        }

        private string GetPacketType(AdapterCode adapterCode, string code)
        {
            switch (adapterCode)
            {
                case AdapterCode.TAIFEX_FUTURES_NIGHT:
                case AdapterCode.TAIFEX_FUTURES_DAY:
                    switch (code)
                    {
                        case "I020": return "21";
                        case "I080": return "22";
                        case "I140": return "23";
                        case "I100": return "24";
                        case "I021": return "25";
                        case "I023": return "26";
                        case "I022": return "27";
                        case "I082": return "28";
                        case "I010": return "11";
                        case "I030": return "12";
                        case "I011": return "13";
                        case "I050": return "14";
                        case "I060": return "15";
                        case "I120": return "16";
                        case "I130": return "17";
                        case "I064": return "18";
                        case "I065": return "19";
                        case "I070": return "31";
                        case "I071": return "32";
                        case "I072": return "33";
                        case "I073": return "34";
                        case "B020": return "71";
                        case "B080": return "72";
                        case "B021": return "73";
                        default: return "99";
                    }
                case AdapterCode.TAIFEX_OPTIONS_DAY:
                case AdapterCode.TAIFEX_OPTIONS_NIGHT:
                    switch (code)
                    {
                        case "I020": return "51";
                        case "I080": return "52";
                        case "I140": return "53";
                        case "I100": return "54";
                        case "I021": return "55";
                        case "I023": return "56";
                        case "I022": return "57";
                        case "I082": return "58";
                        case "I010": return "41";
                        case "I030": return "42";
                        case "I011": return "43";
                        case "I050": return "44";
                        case "I060": return "45";
                        case "I120": return "46";
                        case "I130": return "47";
                        case "I064": return "48";
                        case "I070": return "61";
                        case "I071": return "62";
                        case "I072": return "63";
                        default: return "99";
                    }
                case AdapterCode.TAIFEX_GLOBAL_PATS:
                    switch (code)
                    {
                        case "0": return "0";
                        case "1": return "1";
                        case "2": return "2";
                        default: return "99";
                    }
                default: return code;
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

        private void BuildSubSocket(string ip, int port)
        {
            _socketSub = new SubscriberSocket(string.Format(">tcp://{0}:{1}", ip, port));
            _socketSub.Options.ReceiveHighWatermark = 0;
            //_socketSub.Options.SendHighWatermark = 0;
            Thread.Sleep(100);
            //Sub(string.Format("{0}#{1}", (int)EnumMarketDataChannel.INTERNAL_MARKET_DATA, (int)EnumMarketDataType.HEARTBEAT));
            System.Threading.ThreadPool.QueueUserWorkItem(x =>
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
                                    case 3:
                                        switch (data[0])
                                        {
                                            case "0":
                                                OnUnSolvedPacketReceived(AdapterCode.TSE, data[1], messages[1]); break;
                                            case "1":
                                                OnUnSolvedPacketReceived(AdapterCode.TPEX, data[1], messages[1]); break;
                                            case "2":
                                                OnUnSolvedPacketReceived(AdapterCode.TAIFEX_FUTURES_DAY, ConvertToPacketType_TAIFEX(data[1]), messages[1]); break;
                                            case "3":
                                                OnUnSolvedPacketReceived(AdapterCode.TAIFEX_OPTIONS_DAY, ConvertToPacketType_TAIFEX(data[1]), messages[1]); break;
                                            case "4":
                                                OnUnSolvedPacketReceived(AdapterCode.TAIFEX_FUTURES_NIGHT, ConvertToPacketType_TAIFEX(data[1]), messages[1]); break;
                                            case "5":
                                                OnUnSolvedPacketReceived(AdapterCode.TAIFEX_OPTIONS_NIGHT, ConvertToPacketType_TAIFEX(data[1]), messages[1]); break;
                                            case "6":
                                                OnUnSolvedPacketReceived(AdapterCode.TAIFEX_GLOBAL_PATS, data[1], messages[1]); break;
                                            default:
                                                break;
                                        }
                                        break;
                                    case 4:
                                        switch (data[0])
                                        {
                                            case "0":
                                                ProcessSolvedPacket_TSE(data[1], messages[1]); break;
                                            case "1":
                                                ProcessSolvedPacket_TPEX(data[1], messages[1]); break;
                                            case "2":
                                            case "3":
                                            case "4":
                                            case "5":
                                                ProcessSolvedPacket_TAIFEX(data[1], messages[1]); break;
                                            case "6":
                                                ProcessSolvedPacket_PATS(data[1], messages[1], data[2]); break;
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
                    case "1":
                        OnTseFormat1Received(new CommonLibrary.Model.PacketTSE.Format1(packetData));
                        //string str = System.Environment.CurrentDirectory;
                        //Utility.SaveData(str + "\\TSE_1", packetData);
                        break;
                    case "6":
                        OnTseFormat6Received(new CommonLibrary.Model.PacketTSE.Format6(packetData));
                        break;
                    case "17":
                        OnTseFormat17Received(new CommonLibrary.Model.PacketTSE.Format17(packetData));
                        break;
                    default:
                        break;
                }
            }
            catch(Exception err)
            {
                _logger.Error(err, string.Format("ProcessSolvedPacket_TSE(): packetType:(0) ErrMsg = {1} Data = {2}.", packetType, err.Message, packetData.ToString()));
            }
        }

        private void ProcessSolvedPacket_TPEX(string packetType, byte[] packetData)
        {
            try
            {
                switch (packetType)
                {
                    case "1":
                        OnTpexFormat1Received(new CommonLibrary.Model.PacketTPEX.Format1(packetData));
                        //string str = System.Environment.CurrentDirectory;
                        //Utility.SaveData(str + "\\TPEX_1", packetData);
                        break;
                    case "6":
                        OnTpexFormat6Received(new CommonLibrary.Model.PacketTPEX.Format6(packetData));
                        break;
                    case "17":
                        OnTpexFormat17Received(new CommonLibrary.Model.PacketTPEX.Format17(packetData));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("ProcessSolvedPacket_TPEX(): packetType:(0) ErrMsg = {1} Data = {2}.", packetType, err.Message, packetData.ToString()));
            }
        }

        private void ProcessSolvedPacket_TAIFEX(string packetType, byte[] packetData)
        {
            try
            {
                switch (ConvertToPacketType_TAIFEX(packetType))
                {
                    case "I010":
                        OnTaifexI010Received(new CommonLibrary.Model.PacketTAIFEX.I010(packetData, 0));
                        //string str = System.Environment.CurrentDirectory;
                        //Utility.SaveData(str + "\\TAIFEX_I010", packetData);
                        break;
                    case "I020":
                        OnTaifexI020Received(new CommonLibrary.Model.PacketTAIFEX.I020(packetData, 0));
                        break;
                    case "I080":
                        OnTaifexI080Received(new CommonLibrary.Model.PacketTAIFEX.I080(packetData, 0));
                        break;
                    case "I022":
                        OnTaifexI022Received(new CommonLibrary.Model.PacketTAIFEX.I022(packetData, 0));
                        break;
                    case "I082":
                        OnTaifexI082Received(new CommonLibrary.Model.PacketTAIFEX.I082(packetData, 0));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("ProcessSolvedPacket_TAIFEX(): packetType:(0) ErrMsg = {1} Data = {2}.", packetType, err.Message, packetData.ToString()));
            }
        }

        private void ProcessSolvedPacket_PATS(string packetType, byte[] packetData, string symbol)
        {
            try
            {
                switch (packetType)
                {
                    case "0":
                        OnPatsFormat0Received(new CommonLibrary.Model.PacketPATS.Format0(packetData)); break;
                    case "1":
                        OnPatsFormat1Received(new CommonLibrary.Model.PacketPATS.Format1(packetData, symbol)); break;
                    case "2":
                        OnPatsFormat2Received(new CommonLibrary.Model.PacketPATS.Format2(packetData, symbol)); break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("ProcessSolvedPacket_TAIFEX(): packetType:(0) ErrMsg = {1} Data = {2}.", packetType, err.Message, packetData.ToString()));
            }
        }

        #region Event
        public event EventHandler<TaifexI082ReceivedEventArgs> TaifexI082Received;
        public class TaifexI082ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTAIFEX.I082 PacketData;
            public TaifexI082ReceivedEventArgs(CommonLibrary.Model.PacketTAIFEX.I082 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTaifexI082Received(CommonLibrary.Model.PacketTAIFEX.I082 packetData)
        {
            if (TaifexI082Received != null)
                TaifexI082Received(this, new TaifexI082ReceivedEventArgs(packetData));
        }

        public event EventHandler<TaifexI080ReceivedEventArgs> TaifexI080Received;
        public class TaifexI080ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTAIFEX.I080 PacketData;
            public TaifexI080ReceivedEventArgs(CommonLibrary.Model.PacketTAIFEX.I080 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTaifexI080Received(CommonLibrary.Model.PacketTAIFEX.I080 packetData)
        {
            if (TaifexI080Received != null)
                TaifexI080Received(this, new TaifexI080ReceivedEventArgs(packetData));
        }

        public event EventHandler<TaifexI022ReceivedEventArgs> TaifexI022Received;
        public class TaifexI022ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTAIFEX.I022 PacketData;
            public TaifexI022ReceivedEventArgs(CommonLibrary.Model.PacketTAIFEX.I022 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTaifexI022Received(CommonLibrary.Model.PacketTAIFEX.I022 packetData)
        {
            if (TaifexI022Received != null)
                TaifexI022Received(this, new TaifexI022ReceivedEventArgs(packetData));
        }

        public event EventHandler<TaifexI010ReceivedEventArgs> TaifexI010Received;
        public class TaifexI010ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTAIFEX.I010 PacketData;
            public TaifexI010ReceivedEventArgs(CommonLibrary.Model.PacketTAIFEX.I010 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTaifexI010Received(CommonLibrary.Model.PacketTAIFEX.I010 packetData)
        {
            if (TaifexI010Received != null)
                TaifexI010Received(this, new TaifexI010ReceivedEventArgs(packetData));
        }

        public event EventHandler<TseFormat1ReceivedEventArgs> TseFormat1Received;
        public class TseFormat1ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTSE.Format1 PacketData;
            public TseFormat1ReceivedEventArgs(CommonLibrary.Model.PacketTSE.Format1 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTseFormat1Received(CommonLibrary.Model.PacketTSE.Format1 packetData)
        {
            if (TseFormat1Received != null)
                TseFormat1Received(this, new TseFormat1ReceivedEventArgs(packetData));
        }

        public event EventHandler<TpexFormat1ReceivedEventArgs> TpexFormat1Received;
        public class TpexFormat1ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTPEX.Format1 PacketData;
            public TpexFormat1ReceivedEventArgs(CommonLibrary.Model.PacketTPEX.Format1 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTpexFormat1Received(CommonLibrary.Model.PacketTPEX.Format1 packetData)
        {
            if (TpexFormat1Received != null)
                TpexFormat1Received(this, new TpexFormat1ReceivedEventArgs(packetData));
        }

        public event EventHandler<TaifexI020ReceivedEventArgs> TaifexI020Received;
        public class TaifexI020ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTAIFEX.I020 PacketData;
            public TaifexI020ReceivedEventArgs(CommonLibrary.Model.PacketTAIFEX.I020 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTaifexI020Received(CommonLibrary.Model.PacketTAIFEX.I020 packetData)
        {
            if (TaifexI020Received != null)
                TaifexI020Received(this, new TaifexI020ReceivedEventArgs(packetData));
        }

        public event EventHandler<TseFormat6ReceivedEventArgs> TseFormat6Received;
        public class TseFormat6ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTSE.Format6 PacketData;
            public TseFormat6ReceivedEventArgs(CommonLibrary.Model.PacketTSE.Format6 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTseFormat6Received(CommonLibrary.Model.PacketTSE.Format6 packetData)
        {
            if (TseFormat6Received != null)
                TseFormat6Received(this, new TseFormat6ReceivedEventArgs(packetData));
        }

        public event EventHandler<TpexFormat6ReceivedEventArgs> TpexFormat6Received;
        public class TpexFormat6ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTPEX.Format6 PacketData;
            public TpexFormat6ReceivedEventArgs(CommonLibrary.Model.PacketTPEX.Format6 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTpexFormat6Received(CommonLibrary.Model.PacketTPEX.Format6 packetData)
        {
            if (TpexFormat6Received != null)
                TpexFormat6Received(this, new TpexFormat6ReceivedEventArgs(packetData));
        }

        public event EventHandler<TseFormat17ReceivedEventArgs> TseFormat17Received;
        public class TseFormat17ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTSE.Format17 PacketData;
            public TseFormat17ReceivedEventArgs(CommonLibrary.Model.PacketTSE.Format17 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTseFormat17Received(CommonLibrary.Model.PacketTSE.Format17 packetData)
        {
            if (TseFormat17Received != null)
                TseFormat17Received(this, new TseFormat17ReceivedEventArgs(packetData));
        }

        public event EventHandler<TpexFormat17ReceivedEventArgs> TpexFormat17Received;
        public class TpexFormat17ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketTPEX.Format17 PacketData;
            public TpexFormat17ReceivedEventArgs(CommonLibrary.Model.PacketTPEX.Format17 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnTpexFormat17Received(CommonLibrary.Model.PacketTPEX.Format17 packetData)
        {
            if (TpexFormat17Received != null)
                TpexFormat17Received(this, new TpexFormat17ReceivedEventArgs(packetData));
        }

        public event EventHandler<PatsFormat0ReceivedEventArgs> PatsFormat0Received;
        public class PatsFormat0ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketPATS.Format0 PacketData;
            public PatsFormat0ReceivedEventArgs(CommonLibrary.Model.PacketPATS.Format0 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnPatsFormat0Received(CommonLibrary.Model.PacketPATS.Format0 packetData)
        {
            if (PatsFormat0Received != null)
                PatsFormat0Received(this, new PatsFormat0ReceivedEventArgs(packetData));
        }

        public event EventHandler<PatsFormat1ReceivedEventArgs> PatsFormat1Received;
        public class PatsFormat1ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketPATS.Format1 PacketData;
            public PatsFormat1ReceivedEventArgs(CommonLibrary.Model.PacketPATS.Format1 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnPatsFormat1Received(CommonLibrary.Model.PacketPATS.Format1 packetData)
        {
            if (PatsFormat1Received != null)
                PatsFormat1Received(this, new PatsFormat1ReceivedEventArgs(packetData));
        }

        public event EventHandler<PatsFormat2ReceivedEventArgs> PatsFormat2Received;
        public class PatsFormat2ReceivedEventArgs : EventArgs
        {
            public readonly CommonLibrary.Model.PacketPATS.Format2 PacketData;
            public PatsFormat2ReceivedEventArgs(CommonLibrary.Model.PacketPATS.Format2 packetData)
            {
                PacketData = packetData;
            }
        }
        private void OnPatsFormat2Received(CommonLibrary.Model.PacketPATS.Format2 packetData)
        {
            if (PatsFormat2Received != null)
                PatsFormat2Received(this, new PatsFormat2ReceivedEventArgs(packetData));
        }

        public event EventHandler<UnSolvedPacketReceivedEventArgs> UnSolvedPacketReceived;

        public class UnSolvedPacketReceivedEventArgs : EventArgs
        {
            public readonly AdapterCode AdapterCode;
            public readonly string PacketType;
            public readonly byte[] PacketData;
            public UnSolvedPacketReceivedEventArgs(AdapterCode adapterCode, string packetType, byte[] packetData)
            {
                AdapterCode = adapterCode;
                PacketType = packetType;
                PacketData = packetData;
            }
        }
        private void OnUnSolvedPacketReceived(AdapterCode adapterCode, string packetType, byte[] packetData)
        {
            if (UnSolvedPacketReceived != null)
                UnSolvedPacketReceived(this, new UnSolvedPacketReceivedEventArgs(adapterCode, packetType, packetData));
        }
        #endregion
    }
}
