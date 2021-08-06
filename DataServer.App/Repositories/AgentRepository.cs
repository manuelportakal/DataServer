using DataServer.App.CacheLayer;
using DataServer.App.Models.AgentModels;
using DataServer.App.Models.EntryModels;
using DataServer.App.Services;
using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.App.Repositories
{
    public class AgentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AgentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
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

        public Agent GetByCode(string code)
        {
            var agent = _applicationDbContext.Agents
                                                .Include(x => x.PermittedEntries)
                                                .FirstOrDefault(x => x.Code == code);

            return agent;
        }

        public Agent Create(CreateAgentRequestModel requestModel)
        {
            var controlAgent = _applicationDbContext.Agents
                                                .Include(a => a.PermittedEntries)
                                                .FirstOrDefault(x => x.Code == requestModel.AgentCode);

            if (controlAgent != null)
            {
                foreach (string item in requestModel.EntryCodes)
                {
                    //new entries
                    if (!controlAgent.PermittedEntries.Any(x => x.DataCode == item))
                    {
                        var permittedEntry = new PermittedEntry()
                        {
                            AgentId = controlAgent.Id,
                            DataCode = item
                        };
                        _applicationDbContext.PermittedEntries.Add(permittedEntry);
                    }
                }

                foreach (PermittedEntry item in controlAgent.PermittedEntries)
                {
                    //old entries
                    if (!requestModel.EntryCodes.Contains(item.DataCode))
                    {
                        _applicationDbContext.PermittedEntries.Remove(item);
                    }
                }

                _applicationDbContext.SaveChanges();

                return controlAgent;
            }
            else
            {
                var agent = new Agent()
                {
                    Name = requestModel.Name,
                    Code = requestModel.AgentCode,
                    PermittedEntries = new List<PermittedEntry>()
                };

                foreach (var item in requestModel.EntryCodes)
                {
                    var permittedEntry = new PermittedEntry()
                    {
                        DataCode = item
                    };
                    agent.PermittedEntries.Add(permittedEntry);
                }

                _applicationDbContext.Agents.Add(agent);
                _applicationDbContext.SaveChanges();

                return agent;
            }
        }

        public Agent Remove(RemoveAgentRequestModel requestModel)
        {
            var agent = _applicationDbContext.Agents
                                    .FirstOrDefault(x => x.Id == requestModel.Id);

            if (agent != null)
            {
                _applicationDbContext.Agents.Remove(agent);
                _applicationDbContext.SaveChanges();

                return agent;
            }

            return null;
        }

        public bool IsPermitted(string agentCode, string entryCode)
        {
            var agent = this.GetByCode(agentCode);

            return agent.PermittedEntries.Any(x => x.DataCode == entryCode);
        }
    }
}
