using QLabOSCInterface;
using QLabOSCInterface.Constants;
using QLabOSCInterface.Enums;
using System;
using System.Threading.Tasks;

namespace QLabOscClientTester
{
    class Program
    {
        private static QLabOSCClient _client; 
        static void Main(string[] args)
        {
            AsyncMain().Wait();
        }

        static async Task AsyncMain()
        {
            _client = new QLabOSCClient("192.168.0.103");
            _client.Connect();
            Console.Write("Connected! press any key to continue");
            var response = await _client.SetAlwaysReply(true);
            await LightIntensityChange(_client);
            /*Console.ReadKey();
            var workspaces = await _client.GetWorkSpaces();
            var workspace = workspaces.data[0];
            Console.WriteLine(workspace.displayName);
            var response = await _client.ConnectToWorkSpace(workspace.uniqueID);
            Console.WriteLine("connected to work space");
            var y = await _client.CreateWorkSpaceCue(workspace.uniqueID, CueType.Text);
            Console.WriteLine("Text cue created with id:");
            Console.WriteLine(y.data);
            var z = await _client.GetWorkspaceCueLists(workspace.uniqueID);
            await _client.StartCue(workspace.uniqueID, y.data);
            //await Task.Delay(500);
            Console.WriteLine("Start entering some text!!");
            string input = Console.ReadLine();
            while (input != "end")
            {
                await _client.SetCueLiveText(workspace.uniqueID, y.data, input);
                input = Console.ReadLine();
            }
            await _client.StopCue(workspace.uniqueID, y.data);
            Console.ReadKey();
            */
        }

        private static async Task LightIntensityChange(QLabOSCClient client)
        {
            var workspaces = await client.GetWorkSpaces();
            var workspace = workspaces.data[0];
            var response = await client.ConnectToWorkSpace(workspace.uniqueID);
            Console.WriteLine("Connected to workspace");
            //Create lighting cue first
            var cueResponse = await client.CreateWorkSpaceCue(workspace.uniqueID, CueType.Light);
            
            var y = await client.SetCueDuration(workspace.uniqueID, cueResponse.data, .25f);
            Console.WriteLine("Set cue lighting settings");
            
            Console.WriteLine("Press enter to toggle on and off lights!");
            string endCommand = Console.ReadLine();
            bool lightOn = false;
            while (endCommand != "end")
            {
                string command = lightOn ? "all.intensity = 100" : "all.intensity = 0";
                var x = await client.SetCueLightCommand(workspace.uniqueID, cueResponse.data, command);
                await client.StartCue(workspace.uniqueID, cueResponse.data);
                lightOn = !lightOn;
                endCommand = Console.ReadLine();
            }
            Console.ReadKey();
        }
    }
}
