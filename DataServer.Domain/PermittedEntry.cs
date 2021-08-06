using System;

namespace DataServer.Domain
{
    public class PermittedEntry
    {
        public Guid Id { get; set; }
        public string DataCode { get; set; }

        public Guid AgentId { get; set; }
        public Agent Agent { get; set; }
    }
}
