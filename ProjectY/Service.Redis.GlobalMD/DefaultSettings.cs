using CommonLibrary;
using System;

namespace Service.Redis.GlobalMD
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public bool IS_LOAD_FILE;
        public string REDIS_DB_IP;
        public int REDIS_DB_PORT;
        public string UDP_IP;
        public int UDP_PORT;
        public string PUB_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            IS_LOAD_FILE = Convert.ToBoolean(mapIni.ParamMap["IS_LOAD_FILE"]);
            REDIS_DB_IP = mapIni.ParamMap["REDIS_DB_IP"];
            REDIS_DB_PORT = Convert.ToInt32(mapIni.ParamMap["REDIS_DB_PORT"]);
            UDP_IP = mapIni.ParamMap["UDP_IP"];
            UDP_PORT = Convert.ToInt32(mapIni.ParamMap["UDP_PORT"]);
            PUB_ADDRESS = mapIni.ParamMap["PUB_ADDRESS"];
        }
    }
}
