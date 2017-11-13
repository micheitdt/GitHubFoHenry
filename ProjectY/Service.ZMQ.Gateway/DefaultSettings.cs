using CommonLibrary;
using System;

namespace Service.ZMQ.Gateway
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }
        
        public string S_FRONTEND_ADDRESS;
        public string S_BACKEND_ADDRESS;
        public string XS_FRONTEND_ADDRESS;
        public string XS_BACKEND_ADDRESS;
        public string PU_FRONTEND_ADDRESS;
        public string PU_BACKEND_ADDRESS;
        public string RO_FRONTEND_ADDRESS;
        public string RO_BACKEND_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            S_FRONTEND_ADDRESS = mapIni.ParamMap["S_FRONTEND_ADDRESS"];
            S_BACKEND_ADDRESS = mapIni.ParamMap["S_BACKEND_ADDRESS"];
            XS_FRONTEND_ADDRESS = mapIni.ParamMap["XS_FRONTEND_ADDRESS"];
            XS_BACKEND_ADDRESS = mapIni.ParamMap["XS_BACKEND_ADDRESS"];
            PU_FRONTEND_ADDRESS = mapIni.ParamMap["PU_FRONTEND_ADDRESS"];
            PU_BACKEND_ADDRESS = mapIni.ParamMap["PU_BACKEND_ADDRESS"];
            RO_FRONTEND_ADDRESS = mapIni.ParamMap["RO_FRONTEND_ADDRESS"];
            RO_BACKEND_ADDRESS = mapIni.ParamMap["RO_BACKEND_ADDRESS"];
        }
    }
}
