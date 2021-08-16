using System;

namespace DataServer.Common.Models.EntryModels
{
    public class RemoveEntryResponseModel
    {
        public Guid Id { get; set; }

        public bool IsSucceded { get; set; }
    }
}
