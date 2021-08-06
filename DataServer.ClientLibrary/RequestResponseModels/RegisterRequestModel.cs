using System;
using System.Collections.Generic;

namespace DataServer.ClientLibrary.Models
{
    public class RegisterRequestModel
    {
        public string Name { get; set; }
        public string AgentCode { get; set; }
        public List<string> EntryCodes { get; set; }
    }
}
