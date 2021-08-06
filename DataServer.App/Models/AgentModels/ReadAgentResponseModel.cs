using System;
using System.Collections.Generic;

namespace DataServer.App.Models.AgentModels
{
    public class ReadAgentResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AgentCode { get; set; }

        public bool IsSucceded { get; set; }
    }
}
