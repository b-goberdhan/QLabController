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

            int[,] lights = new int[3, 3]
            {
                { 1,2,3 },
                { 4,5,6 },
                { 7,8,9 }
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


            //now set the light with the corresponding coords with the highest intensity...
            intensities[col, row] = 100;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (row != i && col != j)
                    {
                        intensities[i, j] = (float)Math.Sqrt(i * i + j * j);
                    }
                }
            }

            //now realise that z corresponds to the row of the light
            Console.Clear();
            Console.WriteLine("Y: " + col);
            Console.WriteLine("Z: " + row);
            
        }
    }
}
