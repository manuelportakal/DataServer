using DataServer.App.Models.EntryModels;
using DataServer.App.Services;
using Microsoft.AspNetCore.Mvc;

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
            return _entryService.Write(request);
        }


        [HttpDelete]
        public RemoveEntryResponseModel Remove(RemoveEntryRequestModel requestModel)
        {
            return _entryService.Remove(requestModel);
        }
    }
}
