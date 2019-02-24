using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLabOSCInterface.QLabClasses;
using TechnicalPropClient.Services;

namespace TechnicalPropClient.Controllers
{
    [Route("api/Workspace")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private QLabService _qLabService;
        public WorkspaceController(QLabService qLabService)
        {
            _qLabService = qLabService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetWorkspaces()
        {
            QLabResponse<List<WorkSpace>> response = await _qLabService.Client.GetWorkSpaces();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Connect")]
        public async Task<IActionResult> ConnectToWorkspace([FromQuery] string workspaceId)
        {
            QLabResponse<dynamic> response = await _qLabService.Client.ConnectToWorkSpace(workspaceId);
            if (response == null)
            {
                return BadRequest(workspaceId);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Disconnect")]
        public async Task<IActionResult> DisconnectWorkspace([FromQuery] string workspaceId)
        {
            QLabResponse<dynamic> response = await _qLabService.Client.DisconnectFromWorkSpace(workspaceId);
            if (response == null)
            {
                return BadRequest(workspaceId);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Go")]
        public async Task<IActionResult> GoWorkspace([FromQuery] string workspaceId)
        {
            await _qLabService.Client.GoWorkSpace(workspaceId);
            return Ok();
        }

        [HttpPost]
        [Route("Pause")]
        public async Task<IActionResult> PauseWorkspace([FromQuery] string workspaceId)
        {
            await _qLabService.Client.PauseWorkSpace(workspaceId);
            return Ok();
        }

        [HttpPost]
        [Route("Resume")]
        public async Task<IActionResult> ResumeWorkspace([FromQuery] string workspaceId)
        {
            await _qLabService.Client.ResumeWorkSpace(workspaceId);
            return Ok();
        }

        [HttpPost]
        [Route("Stop")]
        public async Task<IActionResult> StopWorkspace([FromQuery] string workspaceId)
        {
            await _qLabService.Client.StopWorkSpace(workspaceId);
            return Ok();
        }

        [HttpGet]
        [Route("Cues")]
        public async Task<IActionResult> Cues([FromQuery] string workspaceId)
        {
            QLabResponse<dynamic> response = await _qLabService.Client.GetWorkspaceCueLists(workspaceId);
            if (response == null)
            {
                return BadRequest(workspaceId);
            }
            return Ok(response.data);
        }
    }
}