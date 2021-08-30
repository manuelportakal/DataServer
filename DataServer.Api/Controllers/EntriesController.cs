using DataServer.Api.Utilities;
using DataServer.App.Services;
using DataServer.Common.Models.EntryModels;
using DataServer.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public ActionResult<ReadEntryResponseModel> Read(ReadEntryRequestModel requestModel)
        {
            return ResponseMapper.Map(_entryService.GetByDataCode(requestModel));
        }


        [HttpGet]
        public ActionResult<List<Entry>> All()
        {
            return ResponseMapper.Map(_entryService.All());
        }


        [HttpPost("[action]")]
        public ActionResult<WriteEntryResponseModel> Write(WriteEntryRequestModel request)
        {
            return ResponseMapper.Map(_entryService.Write(request));
        }


        [HttpDelete]
        public ActionResult<RemoveEntryResponseModel> Remove(RemoveEntryRequestModel requestModel)
        {
            return ResponseMapper.Map(_entryService.Remove(requestModel));
        }
    }
}