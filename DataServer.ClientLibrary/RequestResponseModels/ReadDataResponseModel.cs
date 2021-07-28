using System;

namespace DataServer.ClientLibrary.RequestResponseModels
{
    public class ReadDataResponseModel
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string DataCode { get; set; }

        public Guid AgentId { get; set; }
    }
}
