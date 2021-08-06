using DataServer.App.CacheLayer;
using DataServer.App.Models.EntryModels;
using DataServer.App.Repositories;
using DataServer.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace DataServer.App.Services
{
    public class EntryService
    {
        private readonly AgentRepository _agentRepository;
        private readonly CustomCacheService<Entry> _cacheService;
        private readonly EntryRepository _entryRepository;
        public EntryService(EntryRepository entryRepository, IMemoryCache memoryCache, AgentRepository agentRepository)
        {
            _entryRepository = entryRepository;
            _cacheService = new CustomCacheService<Entry>(memoryCache);
            _agentRepository = agentRepository;
        }

        public void Reload()
        {
            var entries = _entryRepository.All();
            foreach (var entry in entries)
            {
                _cacheService.Write(entry);
            }
        }

        public List<Entry> All()
        {
            return _entryRepository.All();
        }

        public ReadEntryResponseModel GetByDataCode(ReadEntryRequestModel requestModel)
        {
            var entry = _cacheService.Read(requestModel.DataCode);

            if (entry == null)
            {
                entry = _entryRepository.GetByDataCode(requestModel);
                _cacheService.Write(entry);
                Console.WriteLine($"{requestModel.DataCode}: miss");

                return new ReadEntryResponseModel()
                {
                    IsSucceded = false
                };
            }

            Console.WriteLine($"{requestModel.DataCode}: hit");

            return new ReadEntryResponseModel()
            {
                Id = entry.Id,
                DataCode = entry.Code,
                Value = entry.Value,
                TimeStamp = entry.TimeStamp,
                IsSucceded = true
            };
        }

        public CreateEntryResponseModel Write(CreateEntryRequestModel requestModel)
        {
            // Agent-Entry control

            if (!_agentRepository.IsPermitted(requestModel.AgentCode, requestModel.DataCode))
            {
                return new CreateEntryResponseModel()
                {
                    IsSucceded = false
                };
            }

            var entry = new Entry()
            {
                AgentId = requestModel.AgentId,
                Value = requestModel.Value,
                Code = requestModel.DataCode,
                TimeStamp = DateTime.Now
            };

            _cacheService.Write(entry);

            entry = _entryRepository.Create(entry);
            if (entry != null)
            {
                return new CreateEntryResponseModel()
                {
                    Id = entry.Id,
                    AgentId = requestModel.AgentId,
                    Value = entry.Value,
                    DataCode = entry.Code,
                    TimeStamp = entry.TimeStamp,
                    IsSucceded = true
                };
            }
            return new CreateEntryResponseModel()
            {
                IsSucceded = false
            };
        }

        public RemoveEntryResponseModel Remove(RemoveEntryRequestModel requestModel)
        {
            var entry = _entryRepository.Remove(requestModel);

            if (entry != null)
            {
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
