using CommonLibrary;
using System;

namespace Service.Main
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string DEALER_ADDRESS;
        public string REDIS_IP;
        public int REDIS_PORT;
        public int REDIS_DB_NUMBER;
        public int NUMBER_OF_INSTANCES;
        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            DEALER_ADDRESS = mapIni.ParamMap["DEALER_ADDRESS"];
            REDIS_IP = mapIni.ParamMap["REDIS_IP"];
            REDIS_PORT = Convert.ToInt32(mapIni.ParamMap["REDIS_PORT"]);
            REDIS_DB_NUMBER = Convert.ToInt32(mapIni.ParamMap["REDIS_DB_NUMBER"]);
            NUMBER_OF_INSTANCES = Convert.ToInt32(mapIni.ParamMap["NUMBER_OF_INSTANCES"]);
        }
    }
}
