using BoxPropServer.DataModels.Sensors;
using QLabOSCInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer.Extensions
{
    public static partial class QLabExtension
    {
        public static async Task LightSensorEffect(this QLabOSCClient client, string cueId, string workspaceId, LightSensor sensor)
        {
            await client.HardStopCue(workspaceId, cueId, 50);
            // the max value from the light sensor is ~~1000
            // we want the value to be in relation to the max value of light in qlab which is 255
            float lightIntensity = (((float)sensor.Intensity / 50) * 100f);
            if (lightIntensity > 50)
            {
                lightIntensity = 50;
            }
            else if (lightIntensity < 0)
            {
                lightIntensity = 0;
            }
            string lightingCommand = "all.intensity = " + lightIntensity;
            // we will timeout for 100ms since the arduino sends messges every 100ms
            await client.SetCueLightCommand(workspaceId, cueId, lightingCommand, 0);
            await client.StartCue(workspaceId, cueId, 0);
        }
    }
}
