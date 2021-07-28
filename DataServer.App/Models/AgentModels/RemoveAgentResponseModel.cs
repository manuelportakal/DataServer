using System;

namespace DataServer.App.Models.AgentModels
{
    public class RemoveAgentResponseModel
    {
        public Guid Id { get; set; }

        public bool IsSucceded { get; set; }
    }
}
