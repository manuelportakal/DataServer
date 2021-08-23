using System;

namespace DataServer.Common.Models.EntryModels
{
    public class WriteEntryRequestModel
    {
        public Data RequestData { get; set; }
        public string RequestSignature { get; set; }

        public class Data
        {
            public Guid AgentId { get; set; }
            public string AgentCode { get; set; }
            public string DataCode { get; set; }
            public string Value { get; set; }
        }
    }
}
