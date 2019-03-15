using QLabOSCInterface;
using QLabOSCInterface.QLabClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClientApp.Services
{
    public class QLabService : IDisposable
    {
        private QLabOSCClient _client;
        private string _currentWorkspaceId;
        private string _cueId;

        private bool _isDisposed = false;
        public QLabService()
        {

        }
        public void ConnectQLab(int portNum, string ipAddress)
        {
            _client?.Dispose();
            _client = new QLabOSCClient(ipAddress);
            _client.Connect();
        }
        public async Task<List<WorkSpace>> GetWorkspaces()
        {
            var workspaces = await _client.GetWorkSpaces();
            return workspaces.data;
        }
        public async Task ConnectToWorkspace(string workspaceId)
        {
            await _client.ConnectToWorkSpace(workspaceId);
            _currentWorkspaceId = workspaceId;
            _cueId = (await _client.CreateWorkSpaceCue(_currentWorkspaceId, QLabOSCInterface.Enums.CueType.Light)).data;
        }
        public async Task SetLighting(string lightname, int intensity)
        {
            //disable current cue
            await _client.HardStopCue(_currentWorkspaceId, _cueId);
            string command = lightname + ".intensity = " + intensity;
            await _client.SetCueLightCommand(_currentWorkspaceId, _cueId, command, 0);
            await _client.StartCue(_currentWorkspaceId, _cueId, 0);
        }
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _client.DeleteWorkSpaceCue(_currentWorkspaceId, _cueId).Wait();
                _client.DisconnectFromWorkSpace(_currentWorkspaceId).Wait();
                _client.Dispose();
                _cueId = null;
                _currentWorkspaceId = null;
                _client = null;
            }
        }

    }
}
