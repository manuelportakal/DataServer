using DataServer.Common.Models.EntryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using DataServer.Common.Utilities;

namespace DataServer.Common.Services
{
    public class SecurityService
    {
        public string CreateAgentKey(Guid agentId, int serverNumber, int agentNumber)
        {
            var agentBytes = agentId.ToByteArray();
            var agentNumberBytes = System.Text.Encoding.UTF8.GetBytes(agentNumber.ToString());
            var serverNumberBytes = System.Text.Encoding.UTF8.GetBytes(serverNumber.ToString());

            var combinedBytes = new List<Byte>();
            combinedBytes.AddRange(serverNumberBytes);
            combinedBytes.AddRange(agentBytes);
            combinedBytes.AddRange(agentNumberBytes);

            var base64Data = Convert.ToBase64String(combinedBytes.ToArray());
            var hashData = Sha1Utilities.CalculateHash(base64Data);

            return hashData;
        }

        public string CalculateSignature(WriteEntryRequestModel.Data requestModel, string agentSecurityToken)
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(requestModel);
            var base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
            var combinedString = $"{base64Data}{agentSecurityToken}";

            var calculatedSignature = Sha1Utilities.CalculateHash(combinedString);

            return calculatedSignature;
        }

        public bool ValidateSignature(WriteEntryRequestModel.Data requestModel, string agentSecurityToken, string currentSignature)
        {
            var calculatedSignature = CalculateSignature(requestModel, agentSecurityToken);

            return currentSignature == calculatedSignature;
        }
    }
}
