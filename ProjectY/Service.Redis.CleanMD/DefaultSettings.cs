using CommonLibrary;
using System;

namespace Service.Redis.CleanMD
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }
        
        public string REDIS_DB_IP;
        public int REDIS_DB_PORT;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            REDIS_DB_IP = mapIni.ParamMap["REDIS_DB_IP"];
            REDIS_DB_PORT = Convert.ToInt32(mapIni.ParamMap["REDIS_DB_PORT"]);
        }
    }
}
