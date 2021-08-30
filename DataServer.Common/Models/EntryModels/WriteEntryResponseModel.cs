using System;

namespace DataServer.Common.Models.EntryModels
{
    public class WriteEntryResponseModel
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string DataCode { get; set; }
        public Guid AgentId { get; set; }
    }
}
