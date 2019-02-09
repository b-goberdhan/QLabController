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
            Console.ReadKey();
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

            Console.ReadKey();
        }

        private static void _client_OnRecieved(string response, System.Net.IPEndPoint endpoint)
        {
            Console.Write(response);
            Console.WriteLine(endpoint.ToString());
        }
    }
}
