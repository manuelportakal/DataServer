﻿using System;
using System.Collections.Generic;

namespace DataServer.Common.Models.AgentModels
{
    public class ReadAgentResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AgentCode { get; set; }
    }
}
