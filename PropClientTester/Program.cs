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
        }
        private static async void Device_Recieved(Device<ExampleA> device, ExampleA response)
        {
            Console.WriteLine(response.hi);
            //Now send the light value to QLAB!
            await LightIntensityChange(_client, response.hi);
        }

        private static async Task LightIntensityChange(QLabOSCClient client, int intensity)
        {
            
            Console.WriteLine("Connected to workspace");
            //Create lighting cue first
            var cueResponse = await client.CreateWorkSpaceCue(_workspace.uniqueID, CueType.Light);

            var y = await client.SetCueDuration(_workspace.uniqueID, cueResponse.data, .25f);
            Console.WriteLine("Set cue lighting settings");

            string command = "all.intensity = " + intensity;
            var x = await client.SetCueLightCommand(_workspace.uniqueID, cueResponse.data, command);
            await client.StartCue(_workspace.uniqueID, cueResponse.data);
            //give some time for the cue to finish
            await Task.Delay(100);
            await client.DeleteWorkSpaceCue(_workspace.uniqueID, cueResponse.data);
        }
        class ExampleA
        {
            public string hello { get; set; }
            public int hi { get; set; }
        }
    }
}
