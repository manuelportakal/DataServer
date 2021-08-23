using System;

namespace DataServer.Common.Models.AgentModels
{
    public class RegisterAgentResponseModel
    {
        public Guid Id { get; set; }
        public int ServerNumber { get; set; }

        public bool IsSucceded { get; set; }
    }
}
