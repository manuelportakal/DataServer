using System;

namespace DataServer.Common.Models.AgentModels
{
    public class RemoveAgentResponseModel
    {
        public Guid Id { get; set; }

        public bool IsSucceded { get; set; }
    }
}
