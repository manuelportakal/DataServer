using System;

namespace DataServer.App.Models.EntryModels
{
    public class CreateEntryRequestModel
    {
        public Guid AgentId { get; set; } 
        public string AgentCode { get; set; }
        public string DataCode { get; set; }
        public string Value { get; set; }
    }
}
