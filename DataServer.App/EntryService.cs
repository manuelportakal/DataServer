using DataServer.App.Models.EntryModels;
using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServer.App
{
    public class EntryService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public EntryService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public List<Entry> All()
        {
            var entries = _applicationDbContext.Entries
                                                .ToList();

            return entries;
        }

        public ReadEntryResponseModel GetByDataCode(ReadEntryRequestModel requestModel)
        {
            var entry = _applicationDbContext.Entries
                                .FirstOrDefault(x => x.DataCode == requestModel.DataCode);

            if (entry != null)
            {
                return new ReadEntryResponseModel()
                {
                    Id = entry.Id,
                    DataCode = entry.DataCode,
                    Value = entry.Value,
                    TimeStamp = entry.TimeStamp,
                    IsSucceded = true
                };
            }
            else
            {
                return new ReadEntryResponseModel()
                {
                    IsSucceded = false
                };
            }
        }

        public CreateEntryResponseModel Create(CreateEntryRequestModel requestModel)
        {
            var controlAgent = _applicationDbContext.Agents
                                    .Include(x => x.Entries)
                                    .FirstOrDefault(x => x.Id == requestModel.AgentId);

            if (controlAgent == null)
                return new CreateEntryResponseModel()
                {
                    IsSucceded = false
                };

            var entry = controlAgent.Entries.FirstOrDefault(x => x.DataCode == requestModel.DataCode);

            if (entry != null)
            {
                entry.Value = requestModel.Value;
                _applicationDbContext.Entries.Update(entry);
            }
            else
            {
                entry = new Entry()
                {
                    AgentId = requestModel.AgentId,
                    Value = requestModel.Value,
                    DataCode = requestModel.DataCode,
                    TimeStamp = DateTime.Now
                };

                _applicationDbContext.Entries.Add(entry);
            }

            _applicationDbContext.SaveChanges();

            return new CreateEntryResponseModel()
            {
                Id = entry.Id,
                AgentId = requestModel.AgentId,
                Value = entry.Value,
                DataCode = entry.DataCode,
                TimeStamp = entry.TimeStamp,
                IsSucceded = true
            };

        }

        public RemoveEntryResponseModel Remove(RemoveEntryRequestModel requestModel)
        {
            var entry = _applicationDbContext.Entries
                                    .FirstOrDefault(x => x.Id == requestModel.Id);

            if (entry != null)
            {
                _applicationDbContext.Set<Entry>().Remove(entry);
                _applicationDbContext.SaveChanges();

                return new RemoveEntryResponseModel()
                {
                    Id = entry.Id,
                    IsSucceded = true
                };
            }

            return new RemoveEntryResponseModel()
            {
                IsSucceded = false
            };
        }
    }
}
