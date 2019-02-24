using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechnicalPropClient.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechnicalPropClient.Controllers
{
    [Route("api/QLab")]
    [ApiController]
    public class QLabController : Controller
    {
        private QLabService _qLabService;
        public QLabController(QLabService qLabService)
        {
            _qLabService = qLabService;
        }

        [HttpPost]
        [Route("Connect")]
        public IActionResult Connect([FromQuery] string ipAddress)
        {
            _qLabService.Reset(ipAddress);
            _qLabService.Client.Connect();
            return Ok();
        }
    }
}
