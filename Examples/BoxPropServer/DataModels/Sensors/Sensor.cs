using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer.DataModels.Sensors
{
    public class Sensors
    {
        public LightSensor LightSensor { get; set; }
        public OrientationSensor OrientationSensor { get; set; }
    }
}
