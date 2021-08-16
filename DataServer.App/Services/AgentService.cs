
using DataServer.App.Data;
using DataServer.Common.Models.AgentModels;
using DataServer.Domain;
using DataServer.Infrastructure;
using DataServer.Infrastructure.Caching;
using DataServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataServer.Common.Services
{
    public class AgentService
    {
        private readonly SecurityService _securityService;
        private readonly CustomCacheWrapper<Agent> _agentCacheService;
        private readonly AgentRepository _agentRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        public AgentService(ApplicationDbContext applicationDbContext, IMemoryCache memoryCache, AgentRepository agentRepository, SecurityService securityService)
        {
            _applicationDbContext = applicationDbContext;
            CacheData.Load(memoryCache);
            _agentCacheService = CacheData._agentCacheService;
            _agentRepository = agentRepository;
            _securityService = securityService;
        }

        public List<Agent> All()
        {
            return _agentRepository.All();
        }

        public ReadAgentResponseModel GetById(Guid id)
        {
            var agent = _agentRepository.GetById(id);
            if (agent != null)
            {
                return new ReadAgentResponseModel()
                {
                    Id = agent.Id,
                    AgentCode = agent.Code,
                    Name = agent.Name,
                    IsSucceded = true
                };
            }

            return new ReadAgentResponseModel()
            {
                IsSucceded = false
            };
        }

        public ReadAgentResponseModel GetByCode(string code)
        {
            var agent = _agentCacheService.Read(code);

            if (agent == null)
            {
                agent = _agentRepository.GetByCode(code);
                _agentCacheService.Write(agent);

                return new ReadAgentResponseModel()
                {
                    IsSucceded = false
                };
            }

            return new ReadAgentResponseModel()
            {
                Id = agent.Id,
                AgentCode = agent.Code,
                Name = agent.Name,
                IsSucceded = true
            };
        }

        public RegisterAgentResponseModel Create(RegisterAgentRequestModel requestModel)
        {
            var agent = _agentRepository.Create(requestModel.Name, requestModel.AgentCode, requestModel.EntryCodes);

            if (agent != null)
            {
                int serverRandomNumber = new Random(Environment.TickCount).Next(1000, 9999);
                var securityToken = _securityService.CreateAgentKey(agent.Id, serverRandomNumber, requestModel.RandomNumber);
                agent = _agentRepository.UpdateSecurityToken(agent, securityToken);
                Console.WriteLine($"created agent security token = {securityToken}");

                _agentCacheService.Write(agent);

                return new RegisterAgentResponseModel()
                {
                    Id = agent.Id,
                    ServerNumber = serverRandomNumber,
                    IsSucceded = true
                };
            }

            return new RegisterAgentResponseModel()
            {
                IsSucceded = false
            };
        }

        public RemoveAgentResponseModel Remove(RemoveAgentRequestModel requestModel)
        {
            var agent = _agentRepository.Remove(requestModel.Id);

            if (agent != null)
            {
                return new RemoveAgentResponseModel()
                {
                    Id = agent.Id,
                    IsSucceded = true
                };
            }

            return new RemoveAgentResponseModel()
            {
                IsSucceded = false
            };
        }

        public bool IsPermitted(string agentCode, string entryCode)
        {
            return _agentRepository.IsPermitted(agentCode, entryCode);
        }
    }
}
