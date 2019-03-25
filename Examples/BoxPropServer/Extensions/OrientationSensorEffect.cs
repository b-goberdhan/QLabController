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
        public static async Task OrientationSensorEffect(this QLabOSCClient client, string cueId, string workspaceId, OrientationSensor sensor)
        {
            await client.HardStopCue(workspaceId, cueId, 50);
            //First we need to recall what the layout of lights are we
            // are just going to do a representations of the lights in a 
            // 3 x 3 overhead configuration

            string[,] lights = new string[3, 3]
            {
                { "1","2","3" },
                { "4","5","6" },
                { "7","8","9" }
            };
            float[,] intensities = new float[3, 3]
            {
                {0f,0f,0f },
                {0f,0f,0f },
                {0f,0f,0f }
            };
            //Next we need to normalize the sensor data
            decimal y = ((decimal)sensor.Y / 45) / 2;
            decimal z = ((decimal)sensor.Z / 60) / 2;// * 2;
            int col = (int)Math.Round(y) + 1;

            int row = (int)Math.Round(z) + 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(row - i, 2) + Math.Pow(col - j, 2));
                    float intensity = (distance / 3) * 100;
                    string lightCommand = lights[i, j] + ".intensity = " + intensity;
                    await client.SetCueLightCommand(workspaceId, cueId, lightCommand, 0);
                }
            }


            await client.StartCue(workspaceId, cueId, 100);
            
        }
    }
}
