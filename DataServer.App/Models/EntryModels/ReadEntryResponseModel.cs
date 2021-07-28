using System;

namespace DataServer.App.Models.EntryModels
{
    public class ReadEntryResponseModel
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string DataCode { get; set; }

        public bool IsSucceded { get; set; }
    }
}
