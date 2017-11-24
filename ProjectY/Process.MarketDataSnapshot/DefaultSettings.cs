using CommonLibrary;
using System;

namespace Process.MarketDataSnapshot
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }
        
        public string REDIS_DB_IP;
        public int REDIS_DB_PORT;
        public string UDP_IP;
        public int UDP_PORT;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            REDIS_DB_IP = mapIni.ParamMap["REDIS_DB_IP"];
            REDIS_DB_PORT = Convert.ToInt32(mapIni.ParamMap["REDIS_DB_PORT"]);
            UDP_IP = mapIni.ParamMap["UDP_IP"];
            UDP_PORT = Convert.ToInt32(mapIni.ParamMap["UDP_PORT"]);
        }
    }
}
