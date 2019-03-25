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
            //First we need to recall what the layout of lights are we
            // are just going to do a representations of the lights in a 
            // 3 x 3 overhead configuration

            string[,] lights = new string[3, 3]
            {
                { "backLeft", "backMid", "backRight" },
                { "left", "mid", "right" },
                { "frontLeft", "frontMid", "frontRight" }
            };
            float[,] distances = new float[3, 3]
            {
                {0f,0f,0f },
                {0f,0f,0f },
                {0f,0f,0f }
            };
            //Next we need to normalize the sensor data
            float x = Math.Abs(sensor.X / 360);
            float z = Math.Abs(sensor.Z / 360);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; i++)
                {
                  //  distances[i,j] = 
                }
            }
        }
    }
}
