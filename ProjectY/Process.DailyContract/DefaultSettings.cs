using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.DailyContract
{
    public sealed class DefaultSettings
    {
        private readonly static DefaultSettings _instance = new DefaultSettings();
        public static DefaultSettings Instance { get { return _instance; } }

        public string REDIS_IP;
        public int REDIS_PORT;
        public long REDIS_DB_NUMBER;
        public string SUB_ADDRESS;
        public string[] SUB_PREFIXES;

        private DefaultSettings() { }

        public void Initialize()
        {
            var mapIni = new IniReader(@"DefaultSettings.ini");
            REDIS_IP = mapIni.ParamMap["REDIS_IP"];
            REDIS_PORT = Convert.ToInt32(mapIni.ParamMap["REDIS_PORT"]);
            REDIS_DB_NUMBER = Convert.ToInt64(mapIni.ParamMap["REDIS_DB_NUMBER"]);
            SUB_ADDRESS = mapIni.ParamMap["SUB_ADDRESS"];
            SUB_PREFIXES = mapIni.ParamMap["SUB_PREFIXES"].Split(',');
        }
    }
}
