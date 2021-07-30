using DataServer.App.CacheLayer;
using DataServer.App.Models.EntryModels;
using DataServer.App.Repositories;
using DataServer.Domain;
using System;
using System.Collections.Generic;

namespace DataServer.App.Services
{
    public class EntryService
    {
        private readonly EntryCacheService _entryCacheService;
        private readonly EntryRepository _entryRepository;
        public EntryService(EntryRepository entryRepository, EntryCacheService entryCacheService)
        {
            _entryRepository = entryRepository;
            _entryCacheService = entryCacheService;
            Console.WriteLine("Entry Service Created");
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
                entry = _entryRepository.GetByDataCode(requestModel);
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
                DataCode = entry.DataCode,
                Value = entry.Value,
                TimeStamp = entry.TimeStamp,
                IsSucceded = true
            };
        }

        public CreateEntryResponseModel Write(CreateEntryRequestModel requestModel)
        {
            var entry = new Entry()
            {
                AgentId = requestModel.AgentId,
                Value = requestModel.Value,
                DataCode = requestModel.DataCode,
                TimeStamp = DateTime.Now
            };

            _entryCacheService.Write(entry);

            entry = _entryRepository.Create(entry);
            if (entry != null)
            {
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
