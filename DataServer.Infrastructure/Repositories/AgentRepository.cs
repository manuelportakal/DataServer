using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Infrastructure.Repositories
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

        public Agent Create(string name, string agentCode, List<string> entryCodes)
        {
            var controlAgent = _applicationDbContext.Agents
                                                .Include(a => a.PermittedEntries)
                                                .FirstOrDefault(x => x.Code == agentCode);

            if (controlAgent != null)
            {
                foreach (string item in entryCodes)
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
                    if (!entryCodes.Contains(item.DataCode))
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
                    Name = name,
                    Code = agentCode,
                    PermittedEntries = new List<PermittedEntry>()
                };

                foreach (var item in entryCodes)
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

        public Agent UpdateSecurityToken(Agent agent, string securityToken)
        {
            agent.SecurityToken = securityToken;
            _applicationDbContext.Agents.Update(agent);
            _applicationDbContext.SaveChanges();

            return agent;
        }

        public Agent Remove(Guid id)
        {
            var agent = _applicationDbContext.Agents
                                    .FirstOrDefault(x => x.Id == id);

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
