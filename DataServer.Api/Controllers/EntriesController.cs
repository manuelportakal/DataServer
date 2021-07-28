using DataServer.App;
using DataServer.App.Models.EntryModels;
using DataServer.Domain;
using DataServer.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntriesController : ControllerBase
    {
        private readonly EntryService _entryService;
        public EntriesController(EntryService entryService)
        {
            _entryService = entryService;
        }


        [HttpPost("[action]")]
        public ReadEntryResponseModel Read(ReadEntryRequestModel requestModel)
        {
            return _entryService.GetByDataCode(requestModel);
        }

        [HttpGet]
        public IActionResult All()
        {
            return Ok(_entryService.All());
        }

        [HttpPost("[action]")]
        public CreateEntryResponseModel Write(CreateEntryRequestModel request)
        {
            return _entryService.Create(request);
        }

        [HttpDelete]
        public RemoveEntryResponseModel Remove(RemoveEntryRequestModel requestModel)
        {
            return _entryService.Remove(requestModel);
        }
    }
}
