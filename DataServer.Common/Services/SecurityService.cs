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
            var hashData = ShaUtilities.CalculateSha1Hash(base64Data);

            return hashData;
        }

        public string CalculateSignature(WriteEntryRequestModel.Data requestModel, string agentSecurityToken)
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(requestModel);
            var messageAsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));

            //var phaseOne = ShaUtilities.CalculateSha256Hash($"{agentSecurityToken}{base64Data}");
            //Console.WriteLine($"phaseOne={phaseOne}");
            //var phaseTwo = ShaUtilities.CalculateSha256Hash($"{agentSecurityToken}{phaseOne}");
            //Console.WriteLine($"phaseTwo={phaseTwo}");
            //var calculatedSignature = phaseTwo;

            //return calculatedSignature;
            return ShaUtilities.CalculateHmac256(agentSecurityToken, messageAsBase64);
        }

        public bool ValidateSignature(WriteEntryRequestModel.Data requestModel, string agentSecurityToken, string currentSignature)
        {
            var calculatedSignature = CalculateSignature(requestModel, agentSecurityToken);

            return currentSignature == calculatedSignature;
        }
    }
}
