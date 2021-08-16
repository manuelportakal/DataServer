using System;

namespace DataServer.Clients.Navigation
{
    public class DataStore
    {
        private static Guid AgentId { get; set; }
        private static string DataCode { get; set; }
        private static string Value { get; set; }
        private static string SecurityToken { get; set; }

        public Guid GetAgentId()
        {
            return AgentId;
        }

        public void SetAgentId(Guid id)
        {
            AgentId = id;
        }

        public string GetDataCode()
        {
            return DataCode;
        }

        public void SetDataCode(string datacode)
        {
            DataCode = datacode;
        }

        public string GetValue()
        {
            return Value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }

        public string GetSecurityToken()
        {
            return SecurityToken;
        }

        public void SetSecurityToken(string value)
        {
            SecurityToken = value;
        }
    }
}
