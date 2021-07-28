using System;

namespace DataServer.Domain
{
    public class Entry
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string DataCode { get; set; }

        public Guid AgentId { get; set; }
        public Agent Agent { get; set; }
    }
}
