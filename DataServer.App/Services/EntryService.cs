using DataServer.App.Data;
using DataServer.Common.Exceptions;
using DataServer.Common.Models.EntryModels;
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

        public CustomResponse<List<Entry>> All()
        {
            var data = _entryRepository.All();

            return CustomResponses.Ok(data);
        }

        public CustomResponse<ReadEntryResponseModel> GetByDataCode(ReadEntryRequestModel requestModel)
        {
            var entry = _entryCacheService.Read(requestModel.DataCode);

            if (entry == null)
            {
                entry = _entryRepository.GetByDataCode(requestModel.DataCode);
                _entryCacheService.Write(entry);
                Console.WriteLine($"{requestModel.DataCode}: miss");

                throw new InternalException($"No data found for: {requestModel.DataCode}");
            }

            Console.WriteLine($"{requestModel.DataCode}: hit");

            var responseModel = new ReadEntryResponseModel()
            {
                Id = entry.Id,
                DataCode = entry.Code,
                Value = entry.Value,
                TimeStamp = entry.TimeStamp,
            };

            return CustomResponses.Ok(responseModel);
        }

        public CustomResponse<WriteEntryResponseModel> Write(WriteEntryRequestModel requestModel)
        {
            var agent = _agentCacheService.Read(requestModel.RequestData.AgentCode);

            if (agent == null)
            {
                throw new InternalException($"There is no such agent: {requestModel.RequestData.AgentCode}");
            }

            if (!_agentRepository.IsPermitted(requestModel.RequestData.AgentCode, requestModel.RequestData.DataCode))
            {
                throw new UnAuthorizedException($"Could not create because it does not have write permissions!");
            }

            if (_agentRepository.IsSignatureEnabled(requestModel.RequestData.AgentCode, requestModel.RequestData.DataCode))
            {
                var result = _securityService.ValidateSignature(requestModel.RequestData, agent.SecurityToken, requestModel.RequestSignature);
                if (!result)
                {
                    throw new UnAuthorizedException($"Could not be created because the signature could not be verified!");
                }
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

                var responseModel = new WriteEntryResponseModel()
                {
                    Id = entry.Id,
                    AgentId = requestModel.RequestData.AgentId,
                    Value = entry.Value,
                    DataCode = entry.Code,
                    TimeStamp = entry.TimeStamp
                };

                return CustomResponses.Ok(responseModel);
            }

            throw new InternalException($"Entry could not be created!");
        }

        public CustomResponse<RemoveEntryResponseModel> Remove(RemoveEntryRequestModel requestModel)
        {
            var entry = _entryRepository.Remove(requestModel.Id);

            if (entry != null)
            {

                var responseModel = new RemoveEntryResponseModel()
                {
                    Id = entry.Id
                };

                return CustomResponses.Ok(responseModel);
            }

            throw new InternalException($"There is no such entry: {requestModel.Id}");
        }
    }
}