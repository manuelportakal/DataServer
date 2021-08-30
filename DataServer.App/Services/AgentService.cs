
using DataServer.App.Data;
using DataServer.Common.Models.AgentModels;
using DataServer.Common.ResponseObjects;
using DataServer.Common.Services;
using DataServer.Domain;
using DataServer.Infrastructure.Caching;
using DataServer.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace DataServer.App.Services
{
    public class AgentService
    {
        private readonly SecurityService _securityService;
        private readonly CustomCacheWrapper<Agent> _agentCacheService;
        private readonly AgentRepository _agentRepository;
        public AgentService(IMemoryCache memoryCache, AgentRepository agentRepository, SecurityService securityService)
        {
            CacheData.Load(memoryCache);
            _agentCacheService = CacheData._agentCacheService;
            _agentRepository = agentRepository;
            _securityService = securityService;
        }

        public CustomResponse<List<Agent>> All()
        {
            var data = _agentRepository.All();

            return CustomResponses.Ok(data);
        }

        public CustomResponse<ReadAgentResponseModel> GetById(Guid id)
        {
            var agent = _agentRepository.GetById(id);
            if (agent != null)
            {
                var responseModel = new ReadAgentResponseModel()
                {
                    Id = agent.Id,
                    AgentCode = agent.Code,
                    Name = agent.Name,
                };

                return CustomResponses.Ok(responseModel);
            }

            return CustomResponses.ServerError<ReadAgentResponseModel>($"No data found for: {id}");
        }

        public CustomResponse<ReadAgentResponseModel> GetByCode(string code)
        {
            var agent = _agentCacheService.Read(code);

            if (agent == null)
            {
                agent = _agentRepository.GetByCode(code);
                _agentCacheService.Write(agent);

                return CustomResponses.ServerError<ReadAgentResponseModel>($"No data found for: {code}");
            }

            var responseModel = new ReadAgentResponseModel()
            {
                Id = agent.Id,
                AgentCode = agent.Code,
                Name = agent.Name,
            };

            return CustomResponses.Ok(responseModel);
        }

        public CustomResponse<RegisterAgentResponseModel> Register(RegisterAgentRequestModel requestModel)
        {
            var agent = _agentRepository.Register(requestModel.Name, requestModel.AgentCode, requestModel.Entries);

            if (agent != null)
            {
                int serverRandomNumber = new Random(Environment.TickCount).Next(1000, 9999);
                var securityToken = _securityService.CreateAgentKey(agent.Id, serverRandomNumber, requestModel.RandomNumber);
                agent = _agentRepository.UpdateSecurityToken(agent, securityToken);
                Console.WriteLine($"created agent security token = {securityToken}");

                _agentCacheService.Write(agent);

                var responseModel = new RegisterAgentResponseModel()
                {
                    Id = agent.Id,
                    ServerNumber = serverRandomNumber,
                };

                return CustomResponses.Ok(responseModel);
            }

            return CustomResponses.ServerError<RegisterAgentResponseModel>($"Could not be created!");
        }

        public CustomResponse<RemoveAgentResponseModel> Remove(RemoveAgentRequestModel requestModel)
        {
            var agent = _agentRepository.Remove(requestModel.Id);

            if (agent != null)
            {
                var responseModel = new RemoveAgentResponseModel()
                {
                    Id = agent.Id
                };

                return CustomResponses.Ok(responseModel);
            }

            return CustomResponses.ServerError<RemoveAgentResponseModel>($"There is no such agent: {requestModel.Id}");
        }

        public bool IsPermitted(string agentCode, string entryCode)
        {
            return _agentRepository.IsPermitted(agentCode, entryCode);
        }
    }
}