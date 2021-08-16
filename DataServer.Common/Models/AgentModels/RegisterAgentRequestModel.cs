using System;
using System.Collections.Generic;

namespace DataServer.Common.Models.AgentModels
{
    public class RegisterAgentRequestModel
    {
        public string Name { get; set; }
        public int RandomNumber { get; set; }
        public string AgentCode { get; set; }
        public List<string> EntryCodes { get; set; }
    }
}
