using System;
using System.Collections.Generic;

namespace MarketDataApi
{
    public class IniReader
    {
        public Dictionary<string, string> ParamMap;
        public IniReader(string path)
        {
            ParamMap = new Dictionary<string, string>();
            Read(path);
        }

        private void Read(string path)
        {
            var lines = System.IO.File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line.Length >= 2)
                {
                    if (line.Substring(0, 2) != "//")
                    {
                        var newLine = line.Replace("==", Environment.NewLine);
                        var data = newLine.Split('=');
                        if (data.Length == 2)
                        {
                            var value = data[1].Replace(Environment.NewLine, "=");
                            ParamMap.Add(data[0], value);
                        }
                    }
                }
            }
        }
    }
}
