using System;
using System.Collections.Generic;

namespace DataServer.Common.Models.AgentModels
{
    public class RegisterAgentRequestModel
    {
        public string Name { get; set; }
        public int RandomNumber { get; set; }
        public string AgentCode { get; set; }
        public List<EntryRequestModel> Entries { get; set; }
    }

    public class EntryRequestModel
    {
        public string Code { get; set; }
        public bool IsSignatureEnabled { get; set; }
    }
}
