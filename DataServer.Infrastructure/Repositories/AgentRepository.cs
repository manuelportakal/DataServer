using DataServer.Common.Models.AgentModels;
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

        public Agent Register(string name, string agentCode, List<EntryRequestModel> entries)
        {
            var allEntries = _applicationDbContext.Entries
                                                        .Include(e => e.Agent)
                                                        .ToList();

            var matchedEntries = allEntries.Where(entryOnDb => entries.Any(e => e.Code == entryOnDb.Code && agentCode != entryOnDb.Agent.Code))
                                           .ToList();

            if (matchedEntries != null && matchedEntries.Count > 0)
            {
                throw new Exception("One or more entries exists on db that belong to another Agent");
            }

            var controlAgent = _applicationDbContext.Agents
                                                .Include(a => a.PermittedEntries)
                                                .FirstOrDefault(x => x.Code == agentCode);

            if (controlAgent != null)
            {
                foreach (var item in entries)
                {
                    var controlPermittedEntries = controlAgent.PermittedEntries.FirstOrDefault(x => x.DataCode == item.Code);
                    //new entries
                    if (controlPermittedEntries == null)
                    {
                        var permittedEntry = new PermittedEntry()
                        {
                            AgentId = controlAgent.Id,
                            DataCode = item.Code,
                            IsSignatureEnabled = item.IsSignatureEnabled
                        };
                        _applicationDbContext.PermittedEntries.Add(permittedEntry);
                    }
                    else
                    {
                        controlPermittedEntries.IsSignatureEnabled = item.IsSignatureEnabled;
                        _applicationDbContext.PermittedEntries.Update(controlPermittedEntries);
                    }
                }

                foreach (PermittedEntry item in controlAgent.PermittedEntries)
                {
                    //old entries
                    foreach (var entryCode in entries)
                    {
                        if (!entryCode.Code.Contains(entryCode.Code))
                        {
                            _applicationDbContext.PermittedEntries.Remove(item);
                        }
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

                foreach (var item in entries)
                {
                    var permittedEntry = new PermittedEntry()
                    {
                        DataCode = item.Code,
                        IsSignatureEnabled = item.IsSignatureEnabled
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

        public bool IsSignatureEnabled(string agentCode, string entryCode)
        {
            var agent = this.GetByCode(agentCode);

            return agent.PermittedEntries
                            .Where(x => x.DataCode == entryCode)
                            .Any(x => x.IsSignatureEnabled);
        }
    }
}
