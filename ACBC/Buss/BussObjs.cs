using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACBC.Buss
{
    public class GetConfigParam
    {
        public string env;
        public string group;
    }

    public class UpdateConfigParam
    {
        public string env;
        public string group;
    }

    public class ConfigGroup
    {
        public string key;
        public List<ConfigItem> list = new List<ConfigItem>();
        public ConfigGroup(string key)
        {
            this.key = key;
        }
    }

    public class ConfigItem
    {
        public string key;
        public string value;
    }
}
