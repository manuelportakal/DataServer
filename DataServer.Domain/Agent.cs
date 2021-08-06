using DataServer.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace DataServer.Domain
{
    public class Agent : IDataCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public List<Entry> Entries { get; set; }
        public List<PermittedEntry> PermittedEntries { get; set; }
    }
}
