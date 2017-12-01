using CommonLibrary;
using System;

namespace Process.RemoveMDSnapshot
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string REDIS_IP;
        public int REDIS_PORT;
        public long REDIS_DB_NUMBER;
        public string[] KEYS_TO_REMOVE;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            REDIS_IP = mapIni.ParamMap["REDIS_IP"];
            REDIS_PORT = Convert.ToInt32(mapIni.ParamMap["REDIS_PORT"]);
            REDIS_DB_NUMBER = Convert.ToInt64(mapIni.ParamMap["REDIS_DB_NUMBER"]);
            KEYS_TO_REMOVE = mapIni.ParamMap["KEYS_TO_REMOVE"].Split(',');
        }
    }
}
