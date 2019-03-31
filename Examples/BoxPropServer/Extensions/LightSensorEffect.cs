using BoxPropServer.DataModels.Sensors;
using QLabInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer.Extensions
{
    public static partial class QLabExtension
    {
        public static async Task<string> SetupLightSensorEffect(this QLabClient client, string workspaceId)
        {
            var response = await client.CreateWorkSpaceCue(workspaceId, QLabInterface.Enums.CueType.Light);
            var cueId = response.data;
            await client.SetCueDuration(workspaceId, cueId, 0);
            return cueId;
        }
        private static float prevLightIntensity = -1;
        public static async Task RunLightSensorEffect(this QLabClient client, string cueId, string workspaceId, LightSensor sensor)
        {

            if (prevLightIntensity == sensor.Intensity)
            {
                return;
            }
            prevLightIntensity = sensor.Intensity;
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
            // change this to spot light
            string lightingCommand = "all.intensity = " + lightIntensity;
            // we will timeout for 100ms since the arduino sends messges every 100ms
          
            await client.SetCueLightCommand(workspaceId, cueId, lightingCommand, 0);
            await client.StartCue(workspaceId, cueId, 0);
        }
    }
}
