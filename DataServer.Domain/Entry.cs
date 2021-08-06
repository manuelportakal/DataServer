using DataServer.Domain.Interfaces;
using System;

namespace DataServer.Domain
{
    public class Entry : IDataCode
    {

        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string Code { get; set; }

        public Guid AgentId { get; set; }
        public Agent Agent { get; set; }
    }
}
