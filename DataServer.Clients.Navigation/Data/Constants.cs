using System;
using System.Collections.Generic;

namespace DataServer.Clients.Navigation
{
    public class Constants
    {
        public static readonly string AgentName = "Garmin Navigation x5000";
        public static readonly string AgentCode = "garmin-navigation-x5000";
        public static readonly string DataCode = "navigation-turn-direction"; //left or right
        public static readonly List<string> EntryCodes = new List<string>() { "navigation-turn-direction" };
    }
}
