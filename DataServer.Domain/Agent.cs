using System;
using System.Collections.Generic;

namespace DataServer.Domain
{
    public class Agent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AgentCode { get; set; }

        public List<Entry> Entries { get; set; } 
    }
}
