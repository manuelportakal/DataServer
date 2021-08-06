using System;
using System.Collections.Generic;

namespace DataServer.Clients.Navigation
{
    public class Constants
    {
        public static readonly string AgentName = "Garmin Compass HL";
        public static readonly string AgentCode = "garmin-compass-hl";
        public static readonly string DataCode = "compass-direction";
        public static readonly List<string> EntryCodes = new List<string>() { "compass-direction", "compass-temperature" };
    }
}
