using DeviceInterface.Devices;
using QLabOSCInterface;
using QLabOSCInterface.Enums;
using QLabOSCInterface.QLabClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropClientTester
{
    class Program
    {
        static QLabOSCClient _client;
        static WorkSpace _workspace;
        static string cueId;
        static void Main(string[] args)
        {

            var device = new SerialPortDevice<ExampleA>(3, "Arduino1");
            device.Recieved += Device_Recieved;
            
            _client = new QLabOSCClient("192.168.0.103");
            _client.Connect();
            AsyncMain().Wait();
            device.Connect();
            while (true) { }
        }
        static async Task AsyncMain()
        {
            var workspaces = await _client.GetWorkSpaces();
            _workspace = workspaces.data[0];
            
            await _client.ConnectToWorkSpace(_workspace.uniqueID);
            cueId = (await _client.CreateWorkSpaceCue(_workspace.uniqueID, CueType.Light)).data;
            await _client.SetCueDuration(_workspace.uniqueID, cueId, 0);
        }
        private static async void Device_Recieved(Device<ExampleA> device, ExampleA response)
        {
            //Now send the light value to QLAB!
            Console.Clear();
            Console.WriteLine(response.Name + ": " + response.SensorValue);

            await LightIntensityChange(_client, response.SensorValue);
        }

        private static async Task LightIntensityChange(QLabOSCClient client, long intensity)
        {
            Console.WriteLine("Set cue lighting settings");
            await client.HardStopCue(_workspace.uniqueID, cueId, 0);
            string command = "all.intensity = " + intensity* 2.5;
            await client.SetCueLightCommand(_workspace.uniqueID, cueId, command, 0);
            await client.StartCue(_workspace.uniqueID, cueId, 0);
            //give some time for the cue to finish
            //await Task.Delay(100);
            //await client.DeleteWorkSpaceCue(_workspace.uniqueID, cueResponse.data);
        }
        class ExampleA
        {
            public string Name { get; set; }
            public long SensorValue { get; set; }
        }
    }
}
