using QLabOSCInterface.Constants;
using QLabOSCInterface.Enums;
using QLabOSCInterface.QLabClasses;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QLabOSCInterface
{
    public class QLabOSCClient : OSCClient
    {
        /// <summary>
        /// Default recieve and send osc ports for QLAB
        /// </summary>
        public const int QLAB_SEND_PORT = 53000;
        public const int QLAB_RECV_PORT = 53001;



        public QLabOSCClient(string ipAddress) : base(ipAddress, QLAB_SEND_PORT, QLAB_RECV_PORT)
        {
        }

        public override void Connect()
        {
            base.Connect();
        }
        #region APP METHODS
        public async Task<QLabResponse<dynamic>> SetAlwaysReply(bool reply)
        {
            string path = AppConstants.ALWAYS_REPLY;
            int replyNum = reply ? 1 : 0;
            return await base.Send<QLabResponse<dynamic>>(path, timeout: 500, replyNum);
        }
        #endregion
        #region WORKSPACE METHODS
        public async Task<QLabResponse<List<WorkSpace>>> GetWorkSpaces()
        {
            return await base.Send<QLabResponse<List<WorkSpace>>>(WorkspaceConstants.WORKSPACES);
        }
        public async Task<QLabResponse<dynamic>> ConnectToWorkSpace(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.CONNECT;
            return await base.Send<QLabResponse<dynamic>>(path, timeout:1000);
        }
        public async Task<QLabResponse<dynamic>> DisconnectFromWorkSpace(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.DISCONNECT;
            return await Send<QLabResponse<dynamic>>(path);
        }
        public async Task GoWorkSpace(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.GO;
            await base.Send<QLabResponse<dynamic>>(path, timeout:500);
        }
        public async Task PauseWorkSpace(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.PAUSE;
            await base.Send<QLabResponse<dynamic>>(path, timeout:500);
        }
        public async Task ResumeWorkSpace(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.RESUME;
            await base.Send<QLabResponse<dynamic>>(path, timeout:500);
        }
        public async Task StopWorkSpace(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.STOP;
            await base.Send<QLabResponse<dynamic>>(path, timeout:500);
        }
        public async Task<QLabResponse<dynamic>> GetWorkspaceCueLists(string guid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, guid) + WorkspaceConstants.CUE_LISTS;
            return await base.Send<QLabResponse<dynamic>>(path);
        }
        public async Task<QLabResponse<dynamic>> DeleteWorkSpaceCue(string workspaceGuid, string cueGuid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(WorkspaceConstants.DELETE_CUE_ID, cueGuid);
            return await base.Send<QLabResponse<dynamic>>(path);
        }
        public async Task<QLabResponse<string>> CreateWorkSpaceCue(string workspaceGuid, CueType type)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) + WorkspaceConstants.CREATE_CUE;
            object cueTypeName = Enum.GetName(typeof(CueType), type).ToLower();
            return await base.Send<QLabResponse<string>>(path, timeout:500, cueTypeName);
        }
        #endregion
        #region CUE METHODS
        public async Task<QLabResponse> StartCue(string workspaceGuid, string cueGuid, int timeout = 500)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(CueConstants.CUE_ID, cueGuid) + CueConstants.START;
            return await base.Send<QLabResponse>(path, timeout:timeout);
        }
        public async Task<QLabResponse> StopCue(string workspaceGuid, string cueGuid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(CueConstants.CUE_ID, cueGuid) + CueConstants.STOP;
            return await base.Send<QLabResponse>(path);
        }
        public async Task<QLabResponse> HardStopCue(string workspaceGuid, string cueGuid, int timeout = 500)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(CueConstants.CUE_ID, cueGuid) + CueConstants.HARD_STOP;
            return await base.Send<QLabResponse>(path, timeout:timeout);
        }
        public async Task<QLabResponse> SetCueDuration(string workspaceGuid, string cueGuid, float duration, int timeout = 500)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(CueConstants.CUE_ID, cueGuid) + CueConstants.DURATION;
            return await base.Send<QLabResponse>(path, timeout: timeout, duration);
        }
        public async Task<QLabResponse> GetCueDuration(string workspaceGuid, string cueGuid)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(CueConstants.CUE_ID, cueGuid) + CueConstants.DURATION;
            return await base.Send<QLabResponse>(path);
        }
        public async Task<QLabResponse> SetCueLiveText(string workspaceGuid, string textCueGuid, string text)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
               string.Format(CueConstants.CUE_ID, textCueGuid) + CueConstants.LIVE_TEXT;
            return await base.Send<QLabResponse>(path, timeout:500, text);
        }
        public async Task<QLabResponse> SetCueLightCommand(string workspaceGuid, string lightCueGuid, string command, int timeout=500)
        {
            string path = string.Format(WorkspaceConstants.WORKSPACE, workspaceGuid) +
                string.Format(CueConstants.CUE_ID, lightCueGuid) + CueConstants.LIGHT_COMMAND;
            return await base.Send<QLabResponse>(path, timeout:timeout, command);
        }
        #endregion
    }
}
