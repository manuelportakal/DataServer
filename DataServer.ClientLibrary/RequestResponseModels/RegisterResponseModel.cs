using System;

namespace DataServer.ClientLibrary.Models
{
    public class RegisterResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AgentCode { get; set; }
    }
}
