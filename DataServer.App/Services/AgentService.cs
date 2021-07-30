using DataServer.App.Models.AgentModels;
using DataServer.Domain;
using DataServer.Infrastructure;
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
        private readonly ApplicationDbContext _applicationDbContext;
        public AgentService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Console.WriteLine("Agent Service Created");
        }

        public List<Agent> All()
        {
            var agents = _applicationDbContext.Agents
                                    .ToList();

            return agents;
        }

        public Agent GetById(Guid id)
        {
            var agent = _applicationDbContext.Agents
                                                .FirstOrDefault(x => x.Id == id);

            return agent;
        }

        public CreateAgentResponseModel Create(CreateAgentRequestModel requestModel)
        {
            var controlAgent = _applicationDbContext.Agents.
                                                FirstOrDefault(x => x.AgentCode == requestModel.AgentCode);

            if (controlAgent != null)
            {
                return new CreateAgentResponseModel()
                {
                    Id = controlAgent.Id,
                    IsSucceded = true
                };
            }
            else
            {
                var agent = new Agent()
                {
                    Name = requestModel.Name,
                    AgentCode = requestModel.AgentCode
                };

                _applicationDbContext.Agents.Add(agent);
                _applicationDbContext.SaveChanges();

                return new CreateAgentResponseModel()
                {
                    Id = agent.Id,
                    IsSucceded = true
                };

            }
        }

        public RemoveAgentResponseModel Remove(RemoveAgentRequestModel requestModel)
        {
            var agent = _applicationDbContext.Agents
                                    .FirstOrDefault(x => x.Id == requestModel.Id);

            if (agent != null)
            {
                _applicationDbContext.Agents.Remove(agent);
                _applicationDbContext.SaveChanges();

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
    }
}
