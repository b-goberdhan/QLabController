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
        public static async Task<string> SetupFlexSensorEffect(this QLabOSCClient client, string workspaceId)
        {
            string cueId = (await client.CreateWorkSpaceCue(workspaceId, QLabOSCInterface.Enums.CueType.Light)).data;
            await client.SetCueDuration(workspaceId, cueId, 0);
            return cueId;
        }
        public static async Task RunFlexSensorEffect(this QLabOSCClient client, string workspaceId, string cueId, FlexSensor sensor)
        {
            await client.HardStopCue(workspaceId, cueId, 50);
            float sensorResistence = (sensor.FlexResistence / 45) * 100; //45 is the max value for this setup
            string lightCommand = "all.intensity =" + sensorResistence;
            await client.SetCueLightCommand(workspaceId, cueId, lightCommand, 0);
            await client.StartCue(workspaceId, cueId, 0);            
        }
    }
}
