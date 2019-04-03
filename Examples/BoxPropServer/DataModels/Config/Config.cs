using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer.DataModels.Config
{
    public class Config
    {
        public WifiCredentials WifiCredentials { get; set; }
        public string Status { get; set; }
        public string DeviceIp { get; set; }
    }
}
