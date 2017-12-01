using CommonLibrary;
using System;

namespace Proxy
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string PROXY_NAME;
        public string PROXY_TYPE;
        public string FRONTEND_ADDRESS;
        public string BACKEND_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            PROXY_NAME = mapIni.ParamMap["PROXY_NAME"];
            PROXY_TYPE = mapIni.ParamMap["PROXY_TYPE"];
            FRONTEND_ADDRESS = mapIni.ParamMap["FRONTEND_ADDRESS"];
            BACKEND_ADDRESS = mapIni.ParamMap["BACKEND_ADDRESS"];
        }
    }
}
