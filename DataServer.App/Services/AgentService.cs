using DataServer.App.CacheLayer;
using DataServer.App.Models.AgentModels;
using DataServer.App.Repositories;
using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataServer.App.Services
{
    public class AgentService
    {
        private readonly CustomCacheService<Agent> _cacheService;
        private readonly AgentRepository _agentRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        public AgentService(ApplicationDbContext applicationDbContext, IMemoryCache memoryCache, AgentRepository agentRepository)
        {
            _applicationDbContext = applicationDbContext;
            _cacheService = new CustomCacheService<Agent>(memoryCache);
            _agentRepository = agentRepository;
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
            var agent = _cacheService.Read(code);

            if (agent == null)
            {
                agent = _agentRepository.GetByCode(code);
                _cacheService.Write(agent);

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

        public CreateAgentResponseModel Create(CreateAgentRequestModel requestModel)
        {
            var agent = _agentRepository.Create(requestModel);

            if (agent != null)
            {
                _cacheService.Write(agent);

                return new CreateAgentResponseModel()
                {
                    Id = agent.Id,
                    IsSucceded = true
                };
            }

            return new CreateAgentResponseModel()
            {
                IsSucceded = false
            };
        }

        public RemoveAgentResponseModel Remove(RemoveAgentRequestModel requestModel)
        {
            var agent = _agentRepository.Remove(requestModel);

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
