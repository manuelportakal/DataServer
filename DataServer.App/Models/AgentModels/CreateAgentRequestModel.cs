﻿using System;
using System.Collections.Generic;

namespace DataServer.App.Models.AgentModels
{
    public class CreateAgentRequestModel
    {
        public string Name { get; set; }
        public string AgentCode { get; set; }
        public List<string> EntryCodes { get; set; }
    }
}
