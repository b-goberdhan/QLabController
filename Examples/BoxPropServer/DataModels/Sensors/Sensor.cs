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
        public GravitySensor GravitySensor { get; set; }
        public FlexSensor FlexSensor { get; set; }
        public override string ToString()
        {
            string result = "";
            result = "LightSensor:\n";
            result += "\tIntensity = " + LightSensor?.Intensity + "\n";

            result += "OrientationSensor:\n";
            result += "\tX = " + OrientationSensor?.X + "\n";
            result += "\tY = " + OrientationSensor?.Y + "\n";
            result += "\tZ = " + OrientationSensor?.Z + "\n";

            result += "GravitySensor:\n";
            result += "\tX = " + GravitySensor?.X + "\n";
            result += "\tY = " + GravitySensor?.Y + "\n";
            result += "\tZ = " + GravitySensor?.Z + "\n";

            result += "FlexSensor:\n";
            result += "\tDegrees = " + FlexSensor?.FlexResistence + "\n";
            return result;
        }
    }
}
