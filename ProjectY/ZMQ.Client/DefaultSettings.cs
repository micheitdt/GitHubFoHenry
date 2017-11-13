using CommonLibrary;
using System;

namespace ZMQ.Client
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }
        
        public string PUB_IP;
        public int PUB_PORT;
        public string XPUB_IP;
        public int XPUB_PORT;
        public string PUSH_IP;
        public int PUSH_PORT;
        public string DEALER_IP;
        public int DEALER_PORT;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            PUB_IP = mapIni.ParamMap["PUB_IP"];
            PUB_PORT = Convert.ToInt32(mapIni.ParamMap["PUB_PORT"]);
            XPUB_IP = mapIni.ParamMap["XPUB_IP"];
            XPUB_PORT = Convert.ToInt32(mapIni.ParamMap["XPUB_PORT"]);
            PUSH_IP = mapIni.ParamMap["PUSH_IP"];
            PUSH_PORT = Convert.ToInt32(mapIni.ParamMap["PUSH_PORT"]);
            DEALER_IP = mapIni.ParamMap["DEALER_IP"];
            DEALER_PORT = Convert.ToInt32(mapIni.ParamMap["DEALER_PORT"]);
        }
    }
}
