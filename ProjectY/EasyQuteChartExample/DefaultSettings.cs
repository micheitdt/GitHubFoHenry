using CommonLibrary;
using System;

namespace EasyQuteChartExample
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string SUP_IP;
        public int SUP_PORT;
        public string SERVICE_IP;
        public int SERVICE_PORT;
        public string[] PATS_SYMBOL;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            SUP_IP = mapIni.ParamMap["SUP_IP"];
            SUP_PORT = Convert.ToInt32(mapIni.ParamMap["SUP_PORT"]);
            SERVICE_IP = mapIni.ParamMap["SERVICE_IP"];
            SERVICE_PORT = Convert.ToInt32(mapIni.ParamMap["SERVICE_PORT"]);
            PATS_SYMBOL = mapIni.ParamMap["PATS_SYMBOL"].Split(',');
        }
    }
}
