using CommonLibrary;
using System;

namespace Adapter.ESGlobalMD
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string MD_IP;        
        public int MD_PORT;
        public string MD_ID;    
        public string MD_PASSWORD;
        public string PUB_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            MD_IP = mapIni.ParamMap["MD_IP"];            
            MD_PORT = Convert.ToInt32(mapIni.ParamMap["MD_PORT"]);
            MD_ID = mapIni.ParamMap["MD_ID"];            
            MD_PASSWORD = mapIni.ParamMap["MD_PASSWORD"];
            PUB_ADDRESS = mapIni.ParamMap["PUB_ADDRESS"];
        }
    }
}
