﻿using BoxPropServer.DataModels.QLab;
using BoxPropServer.DataModels.Sensors;
using QLabInterface;
using QLabInterface.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPropServer.Extensions
{
    public static partial class QLabExtension
    {
        public static async Task<Group> SetupOrientationSensorGridEffect(this QLabClient client, string workspaceId)
        {
            var response = await client.CreateWorkSpaceCue(workspaceId, CueType.Group);
            
            string cueGroupId = response.data;
            
            Group group = new Group();
            group.Id = cueGroupId;
            int i = 0;
            while (i < 9)
            {
                string cueId = (await client.CreateWorkSpaceCue(workspaceId, CueType.Light, 1000))?.data;
                if (cueId == null)
                {
                    int x = 0;
                }
                var moveResponse = await client.Move(workspaceId, cueId, i, cueGroupId, 500);
                await client.SetCueDuration(workspaceId, cueId, 0, 100);
                if (moveResponse.status != "ok")
                {
                    await client.DeleteWorkSpaceCue(workspaceId, cueGroupId);
                    throw new Exception();
                }
                group.Children.Add(cueId);
                i++;
            }
            return group;
           
        }
        private static int prevRow = -1;
        private static int prevCol = -1;
        public static async Task RunOrientationSensorGridEffect(this QLabClient client, string workspaceId, Group group, OrientationSensor sensor)
        {
            

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
            if (prevRow == row && prevCol == col)
            {
                //same configuration as before, dont send anything!
                return;
            }
            prevRow = row;
            prevCol = col;
            await client.HardStopCue(workspaceId, group.Id, 0);
            //First we need to recall what the layout of lights are we
            // are just going to do a representations of the lights in a 
            // 3 x 3 overhead configuration
            int rowOffset = 0;
            Task[] tasks = new Task[9];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float distance = (float)Math.Sqrt(Math.Pow(row - i, 2) + Math.Pow(col - j, 2));
                    float intensity = (distance / 3) * 100;
                    string lightCommand = lights[i, j] + ".intensity = " + intensity;
                    string cueID = group.Children[j + rowOffset];
                    tasks[j+rowOffset] = client.SetCueLightCommand(workspaceId, cueID, lightCommand, 0).ContinueWith(async (response) =>
                    {
                        await client.StartCue(workspaceId, cueID, 0);
                    });
                    
                }
                rowOffset += 3;
            }
            Task.WaitAll(tasks);
        }
       
    }

}
