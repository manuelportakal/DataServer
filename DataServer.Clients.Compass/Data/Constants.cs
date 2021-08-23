using DataServer.Common.Models.AgentModels;
using System;
using System.Collections.Generic;

namespace DataServer.Clients.Navigation
{
    public class Constants
    {
        public static readonly string AgentName = "Garmin Compass HL";
        public static readonly string AgentCode = "garmin-compass-hl";
        public static readonly string DataCode = "compass-direction";
        public static readonly List<EntryRequestModel> EntryCodes = new List<EntryRequestModel>()
        {
            new EntryRequestModel() { Code = "compass-direction", IsSignatureEnabled = true},
            new EntryRequestModel() { Code = "compass-temperature", IsSignatureEnabled = false},
        };
    }
}
