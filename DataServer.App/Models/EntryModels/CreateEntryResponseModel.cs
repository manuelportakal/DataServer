using System;

namespace DataServer.App.Models.EntryModels
{
    public class CreateEntryResponseModel
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string DataCode { get; set; }
        public Guid AgentId { get; set; }

        public bool IsSucceded { get; set; }
    }
}
