﻿using CommonLibrary;
using System;

namespace ESunnyPATSConverterApi
{
    public class ESunnyPATSMapConverter
    {
        private static ESunnyPATSMapConverter _instance = new ESunnyPATSMapConverter();
        public static ESunnyPATSMapConverter Instance { get { return _instance; } }

        public static IniReader _eSunnyToPATS;
        public static IniReader _patsToESunny;
        private System.Collections.Generic.Dictionary<string, string> _monthE2IMapList = new System.Collections.Generic.Dictionary<string, string>();
        private System.Collections.Generic.Dictionary<string, string> _monthI2EMapList = new System.Collections.Generic.Dictionary<string, string>();

        private ESunnyPATSMapConverter()
        {
            Initialize();
        }

        public void Initialize()
        {
            _eSunnyToPATS = new IniReader(@"ESunnyToPATSMap.ini");
            _patsToESunny = new IniReader(@"PATSToESunnyMap.ini");
            _monthE2IMapList = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"JAN", "01"},{"FEB", "02"},{"MAR", "03"},{"APR", "04"},{"MAY", "05"},{"JUN", "06"},{"JUL", "07"},{"AUG", "08"},{"SEP", "09"},{"OCT", "10"},{"NOV", "11"},{"DEC", "12"}
        };
            _monthI2EMapList = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"01","JAN"},{"02","FEB"},{"03","MAR"},{"04","APR"},{"05", "MAY"},{"06", "JUN"},{"07", "JUL"},{"08", "AUG"},{"09", "SEP"},{"10", "OCT"},{"11", "NOV"},{"12", "DEC"}
        };
        }
        
        /// <summary>
        /// PATS to 易盛交易所&商品代碼
        /// </summary>
        public bool GetESExchangeNoCommondityNo(string patsexchangekey, string patscommonditykey, ref string returnexchange, ref string returncommondity)
        {
            string key = patsexchangekey + "." + patscommonditykey;
            string returnvalue = string.Empty;
            if (_patsToESunny.ParamMap.ContainsKey(key))
            {
                returnvalue = _patsToESunny.ParamMap[key];
                if(returnvalue != "." && returnvalue.Split('.').Length == 2)
                {
                    returnexchange = returnvalue.Split('.')[0];
                    returncommondity = returnvalue.Split('.')[1];
                    return true;
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// PATS to 易盛契約代碼
        /// </summary>
        public bool GetESContract(bool islme, string patskey, ref string returnvalue)
        {
            try
            {
                if (islme)
                {
                    if (patskey == "3M")
                    {
                        returnvalue = "3M";
                        return true;
                    }
                    return false;
                }
                else if (patskey.Length == 5)//EX: OTC17  => 1710
                {
                    string monthKey = patskey.Substring(0, 3);

                    returnvalue = patskey.Substring(3, 2) + _monthE2IMapList[monthKey];
                    return true;
                }
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 易盛 to 取得PATS交易所代碼
        /// </summary>
        public bool GetPATSExchangeNo(string key, ref string returnvalue)
        {
            if (_eSunnyToPATS.ParamMap.ContainsKey(key))
            {
                returnvalue = _eSunnyToPATS.ParamMap[key];
                return true;
            }
            return false;
        }
        /// <summary>
        /// 易盛 to 取得PATS商品
        /// </summary>
        public bool GetPATSCommondityNo(string key, ref string returnvalue)
        {
            if (_eSunnyToPATS.ParamMap.ContainsKey(key))
            {
                returnvalue = _eSunnyToPATS.ParamMap[key];
                return true;
            }
            return false;
        }
        /// <summary>
        /// 易盛 to PATS交易所&商品代碼
        /// </summary>
        public bool GetPATSExchangeNoCommondityNo(string patsexchangekey, string patscommonditykey, ref string returnexchange, ref string returncommondity)
        {
            string key = patsexchangekey + "." + patscommonditykey;
            string returnvalue = string.Empty;
            if (_eSunnyToPATS.ParamMap.ContainsKey(key))
            {
                returnvalue = _eSunnyToPATS.ParamMap[key];
                if (returnvalue != "." && returnvalue.Split('.').Length == 2)
                {
                    returnexchange = returnvalue.Split('.')[0];
                    returncommondity = returnvalue.Split('.')[1];
                    return true;
                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// 易盛 to PATS契約代碼
        /// </summary>
        public bool GetPATSContract(bool islme, string esunnykey, ref string returnvalue)
        {
            try
            {
                if (islme)
                {
                    if (esunnykey == "3M")
                    {
                        returnvalue = "3M";
                        return true;
                    }
                    return false;
                }
                else if (esunnykey.Length == 4)//EX: 1710  => OTC17
                {
                    string monthKey = esunnykey.Substring(2, 2);

                    returnvalue = _monthI2EMapList[monthKey] + esunnykey.Substring(0, 2);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// PATS轉易盛訂閱資料
        /// </summary>
        public bool PATSConverterToESunny(string exchange, string commondity, string contract, ref string exchangeNo, ref string commondityNo, ref string contractNo, ref string msg)
        {
            string orgData = exchange + "." + commondity + "." + contract;
            exchangeNo = string.Empty;
            commondityNo = string.Empty;
            bool isConverter = GetESExchangeNoCommondityNo(exchange, commondity, ref exchangeNo, ref commondityNo);
            if (isConverter)
            {
                contractNo = string.Empty;
                bool isContract = GetESContract((exchange == "LME"), contract, ref contractNo);
                if (isContract)
                {
                    string esummySymbol = exchangeNo + "." + commondityNo + "." + contractNo;
                    msg = string.Format("{0}    PATS转换易盛成功:    PATS: 「{1}」,  ESunny:  「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, esummySymbol);
                    return true;
                }
                else
                {
                    msg = string.Format("{0}    PATS转换易盛失败:    PATS: 「{1}」,  ESunny:  「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchangeNo + "." + commondityNo + "." + contractNo);
                    return false;
                }
            }
            else
            {
                msg = string.Format("{0}    PATS转换易盛失败:    PATS: 「{1}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchangeNo + "." + commondityNo);
                return true;
            }
        }
        /// <summary>
        /// 易盛轉PATS訂閱資料
        /// </summary>
        public bool ESunnyConverterToPATS(string exchange, string commondity, string contract, ref string exchangeNo, ref string commondityNo, ref string contractNo, ref string msg)
        {
            string orgData = exchange + "." + commondity + "." + contract;
            exchangeNo = string.Empty;
            commondityNo = string.Empty;
            bool isConverter = GetPATSExchangeNoCommondityNo(exchange, commondity, ref exchangeNo, ref commondityNo);
            if (isConverter)
            {
                contractNo = string.Empty;
                bool isContract = GetPATSContract((exchange == "LME"), contract, ref contractNo);
                if (isContract)
                {
                    string patsSymbol = exchangeNo + "." + commondityNo + "." + contractNo;
                    msg = string.Format("{0}    易盛转换PATS成功:    ESunny:  「{1}」,  PATS: 「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, patsSymbol);
                    return true;
                }
                else
                {
                    msg = string.Format("{0}    易盛转换PATS失败:    ESunny:  「{1}」,  PATS: 「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchange + "." + commondityNo + "." + contract);
                    return false;
                }
            }
            else
            {
                msg = string.Format("{0}    易盛转换PATS失败:    ESunny:  「{1}」,  PATS: 「{2}」", DateTime.Now.ToString("HH:mm:ss:ttt"), orgData, exchangeNo + "." + commondityNo);
                return false;
            }
        }

        public bool ContainESunnyCommondityNo(string commondityno)
        {
            return _patsToESunny.ParamMap.ContainsKey(commondityno);
        }
    }
}
