using System;

namespace DataServer.ClientLibrary.RequestResponseModels
{
    public class WriteDataRequestModel
    {
        public Guid AgentId { get; set; }
        public string DataCode { get; set; }
        public string Value { get; set; }
    }
}
