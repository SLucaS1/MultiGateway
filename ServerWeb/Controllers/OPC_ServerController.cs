using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OPC_Server.Classi;
using OPC_Server.Shared;


namespace ServerWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OPC_ServerController : ControllerBase
    {
 
        private readonly ILogger<OPC_ClientController> _logger;

        public OPC_ServerController(ILogger<OPC_ClientController> logger)
        {
            //_logger = logger;
        }

    


        [HttpGet("status")]
        public clsStatus GetStatus()
        {
            return Shared_Vars.OPC_Status;
        }


    

  
    }
}
