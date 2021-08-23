using DataServer.App.Services;
using DataServer.Common.Models.AgentModels;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public RegisterAgentResponseModel Create(RegisterAgentRequestModel requestModel)
        {
            return _agentService.Register(requestModel);
        }


        [HttpDelete]
        public RemoveAgentResponseModel Remove(RemoveAgentRequestModel requestModel)
        {
            return _agentService.Remove(requestModel);
        }
    }
}
