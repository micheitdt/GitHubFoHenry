using CommonLibrary;
using System;

namespace Service.ZMQ.Server
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }
        
        public string PUB_ADDRESS;
        public string XPUB_ADDRESS;
        public string PUSH_ADDRESS;
        public string DEALER_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            PUB_ADDRESS = mapIni.ParamMap["PUB_ADDRESS"];
            XPUB_ADDRESS = mapIni.ParamMap["XPUB_ADDRESS"];
            PUSH_ADDRESS = mapIni.ParamMap["PUSH_ADDRESS"];
            DEALER_ADDRESS = mapIni.ParamMap["DEALER_ADDRESS"];
        }
    }
}
