﻿using System;

namespace DataServer.ClientLibrary.Models
{
    public class RegisterResult
    {
        public Guid? Id { get; set; }
        public string AgentSecurityToken { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsSucceded { get; set; }

    }
}
