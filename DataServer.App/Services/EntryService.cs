using DataServer.App.Data;
using DataServer.Common.Models.EntryModels;
using DataServer.Domain;
using DataServer.Infrastructure.Caching;
using DataServer.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace DataServer.Common.Services
{
    public class EntryService
    {
        private readonly SecurityService _securityService;
        private readonly AgentRepository _agentRepository;
        private readonly CustomCacheWrapper<Entry> _entryCacheService;
        private readonly CustomCacheWrapper<Agent> _agentCacheService;
        private readonly EntryRepository _entryRepository;
        public EntryService(EntryRepository entryRepository, IMemoryCache memoryCache, AgentRepository agentRepository, SecurityService securityService)
        {
            _entryRepository = entryRepository;
            CacheData.Load(memoryCache);
            _entryCacheService = CacheData._entryCacheService;
            _agentCacheService = CacheData._agentCacheService;
            _agentRepository = agentRepository;
            _securityService = securityService;
        }

        public void Reload()
        {
            var entries = _entryRepository.All();
            foreach (var entry in entries)
            {
                _entryCacheService.Write(entry);
            }
        }

        public List<Entry> All()
        {
            return _entryRepository.All();
        }

        public ReadEntryResponseModel GetByDataCode(ReadEntryRequestModel requestModel)
        {
            var entry = _entryCacheService.Read(requestModel.DataCode);

            if (entry == null)
            {
                entry = _entryRepository.GetByDataCode(requestModel.DataCode);
                _entryCacheService.Write(entry);
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

        public WriteEntryResponseModel Write(WriteEntryRequestModel requestModel)
        {
            var agent = _agentCacheService.Read(requestModel.RequestData.AgentCode);
            // Agent-Entry control
            var result = _securityService.ValidateSignature(requestModel.RequestData, agent.SecurityToken, requestModel.RequestSignature);
            if (!result)
            {
                return new WriteEntryResponseModel()
                {
                    IsSucceded = false
                };
            }

            if (!_agentRepository.IsPermitted(requestModel.RequestData.AgentCode, requestModel.RequestData.DataCode))
            {
                return new WriteEntryResponseModel()
                {
                    IsSucceded = false
                };
            }

            var entry = new Entry()
            {
                AgentId = requestModel.RequestData.AgentId,
                Value = requestModel.RequestData.Value,
                Code = requestModel.RequestData.DataCode,
                TimeStamp = DateTime.Now
            };

            _entryCacheService.Write(entry);

            entry = _entryRepository.Create(entry);
            if (entry != null)
            {
                return new WriteEntryResponseModel()
                {
                    Id = entry.Id,
                    AgentId = requestModel.RequestData.AgentId,
                    Value = entry.Value,
                    DataCode = entry.Code,
                    TimeStamp = entry.TimeStamp,
                    IsSucceded = true
                };
            }
            return new WriteEntryResponseModel()
            {
                IsSucceded = false
            };
        }

        public RemoveEntryResponseModel Remove(RemoveEntryRequestModel requestModel)
        {
            var entry = _entryRepository.Remove(requestModel.Id);

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
