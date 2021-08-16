using System;

namespace DataServer.Clients.Navigation
{
    public class DataStore
    {
        private static Guid AgentId { get; set; }
        private static string CompassValue { get; set; }
        private static string TurnDirection { get; set; }
        private static string SecurityToken { get; set; }

        public Guid GetAgentId()
        {
            return AgentId;
        }

        public void SetAgentId(Guid id)
        {
            AgentId = id;
        }

        public string GetCompassValue()
        {
            return CompassValue;
        }

        public void SetCompassValue(string compassValue)
        {
            CompassValue = compassValue;
        }

        public string GetTurnDirection()
        {
            return TurnDirection;
        }

        public void SetTurnDirection(string turnDirection)
        {
            TurnDirection = turnDirection;
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
