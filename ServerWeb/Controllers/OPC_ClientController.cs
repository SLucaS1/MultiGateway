using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OPC_Client.Classi;
using OPC_Client.Shared;


namespace ServerWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OPC_ClientController : ControllerBase
    {
 
        private readonly ILogger<OPC_ClientController> _logger;

        public OPC_ClientController(ILogger<OPC_ClientController> logger)
        {
            //_logger = logger;
        }




        [HttpGet("tree")]
        public clsTree[] GetTree()
        {
            return Shared_Vars.OPC_Tree?.children;
        }


        [HttpGet("status")]
        public clsStatus GetStatus()
        {
            return Shared_Vars.OPC_Status;
        }

        [HttpGet("tags")]
        public clsTag[] GetTags()
        {
            return Shared_Vars.Tags;
        }

        [HttpPost("PostCommand")]
        public void PostCommand(string command, string args = null)
        {
            switch (command.ToLower())
            {
                case "start":
                    break;
                case "stop":
                    break;
                case "addsubscribe":
                    break;
                case "removesubscribe":
                    break;
                case "write":
                    break;
                default:
                    break;
            }

        }



    }
}
