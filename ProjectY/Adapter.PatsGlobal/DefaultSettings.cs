using CommonLibrary;

namespace Adapter.PatsGlobal
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string PATS_ID;
        public string PATS_PASSWORD;
        public string PATS_HOST_IP;
        public string PATS_HOST_PORT;
        public string PATS_PRICE_IP;
        public string PATS_PRICE_PORT;
        public string PATS_APPID;
        public string PATS_LICENSE;
        public string PUB_ADDRESS;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            PATS_ID = mapIni.ParamMap["PATS_ID"];
            PATS_PASSWORD = mapIni.ParamMap["PATS_PASSWORD"];
            PATS_HOST_IP = mapIni.ParamMap["PATS_HOST_IP"];
            PATS_HOST_PORT = mapIni.ParamMap["PATS_HOST_PORT"];//Convert.ToInt32(mapIni.ParamMap["MD_PORT"]);
            PATS_PRICE_IP = mapIni.ParamMap["PATS_PRICE_IP"];
            PATS_PRICE_PORT = mapIni.ParamMap["PATS_PRICE_PORT"];
            PATS_APPID = mapIni.ParamMap["PATS_APPID"];
            PATS_LICENSE = mapIni.ParamMap["PATS_LICENSE"];
            PUB_ADDRESS = mapIni.ParamMap["PUB_ADDRESS"];
        }
    }
}
