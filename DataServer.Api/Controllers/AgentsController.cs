using DataServer.App;
using DataServer.App.Models.AgentModels;
using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController : ControllerBase
    {
        private readonly AgentService _agentService;
        public AgentsController(AgentService agentService)
        {
            _agentService = agentService;
        }


        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_agentService.GetById(id));
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_agentService.All());
        }

        [HttpPost]
        public CreateAgentResponseModel Create(CreateAgentRequestModel requestModel)
        {
            return _agentService.Create(requestModel);
        }

        [HttpDelete]
        public RemoveAgentResponseModel Remove(RemoveAgentRequestModel requestModel)
        {
            return _agentService.Remove(requestModel);
        }
    }
}
