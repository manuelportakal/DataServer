using DataServer.Api.Utilities;
using DataServer.App.Services;
using DataServer.Common.Exceptions;
using DataServer.Common.Models.AgentModels;
using DataServer.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public ActionResult<ReadAgentResponseModel> Get(Guid id)
        {
            return ResponseMapper.Map(_agentService.GetById(id));
        }


        [HttpGet]
        public ActionResult<List<Agent>> All()
        {
            return ResponseMapper.Map(_agentService.All());
        }


        [HttpPost]
        public ActionResult<RegisterAgentResponseModel> Create(RegisterAgentRequestModel requestModel)
        {
            return ResponseMapper.Map(_agentService.Register(requestModel));
        }


        [HttpDelete]
        public ActionResult<RemoveAgentResponseModel> Remove(RemoveAgentRequestModel requestModel)
        {
            return ResponseMapper.Map(_agentService.Remove(requestModel));
        }
    }
}
