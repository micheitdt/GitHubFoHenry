using CommonLibrary;
using System;

namespace Adapter.TaifexOD
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string LOCAL_UDP_IP;
        public string UDP_IP;
        public int UDP_PORT;
        public string PUB_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            LOCAL_UDP_IP = mapIni.ParamMap["LOCAL_UDP_IP"];
            UDP_IP = mapIni.ParamMap["UDP_IP"];
            UDP_PORT = Convert.ToInt32(mapIni.ParamMap["UDP_PORT"]);
            PUB_ADDRESS = mapIni.ParamMap["PUB_ADDRESS"];
        }
    }
}
