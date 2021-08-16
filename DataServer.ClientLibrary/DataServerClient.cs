using System;
using System.Net.Http;
using System.Net;
using System.Runtime;
using DataServer.ClientLibrary.Models;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using DataServer.Common.Models.AgentModels;
using DataServer.Common.Models.EntryModels;
using DataServer.Common.Services;

namespace DataServer.ClientLibrary
{
    public class DataServerClient
    {
        private readonly SecurityService _securityService;
        private readonly JsonSerializerOptions serializerSettings;

        public DataServerClient()
        {
            serializerSettings = new()
            {
                PropertyNameCaseInsensitive = true
            };

            _securityService = new SecurityService();
        }


        public async Task<RegisterResult> Register(string name, string agentCode, List<string> entryCodes)
        {
            var agentRandomNumber = new Random(Environment.TickCount).Next(1000, 9999);
            var request = new RegisterAgentRequestModel()
            {
                Name = name,
                AgentCode = agentCode,
                EntryCodes = entryCodes,
                RandomNumber = agentRandomNumber
            };

            var jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            using var client = new HttpClient();
            var requestUrl = $"{Constants.ApiUrl}/Agents";
            var result = await client.PostAsync(requestUrl, stringContent);
            var responseAsJsonString = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<RegisterAgentResponseModel>(responseAsJsonString, serializerSettings);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return new RegisterResult()
                {
                    Id = null,
                    IsSucceded = false,
                };
            }

            var agentSecurityToken = _securityService.CreateAgentKey(responseModel.Id, responseModel.ServerNumber, agentRandomNumber);
            Console.WriteLine($"created agent security token = {agentSecurityToken}");
            return new RegisterResult()
            {

                Id = responseModel.Id,
                IsSucceded = true,
                AgentSecurityToken = agentSecurityToken
            };
        }

        public async Task<WriteDataResult> WriteData(Guid agentId, string agentCode, string dataCode, string value, string agentSecurityToken)
        {
            var request = new WriteEntryRequestModel()
            {
                RequestData = new WriteEntryRequestModel.Data()
                {
                    AgentId = agentId,
                    AgentCode = agentCode,
                    DataCode = dataCode,
                    Value = value
                },
            };

            request.RequestSignature = _securityService.CalculateSignature(request.RequestData, agentSecurityToken);

            var jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            using var client = new HttpClient();
            var requestUrl = $"{Constants.ApiUrl}/Entries/Write";
            var result = await client.PostAsync(requestUrl, stringContent);
            var responseAsJsonString = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<WriteEntryResponseModel>(responseAsJsonString, serializerSettings);

            if (result.StatusCode != HttpStatusCode.OK || !responseModel.IsSucceded)
            {
                return new WriteDataResult()
                {
                    IsSucceded = false
                };
            }

            return new WriteDataResult()
            {
                IsSucceded = true
            };
        }

        public async Task<ReadDataResult> ReadData(string dataCode)
        {
            var request = new ReadEntryRequestModel()
            {
                DataCode = dataCode,
            };

            var jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            using var client = new HttpClient();
            var requestUrl = $"{Constants.ApiUrl}/Entries/Read";
            var result = await client.PostAsync(requestUrl, stringContent);
            var responseAsJsonString = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<ReadEntryResponseModel>(responseAsJsonString, serializerSettings);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return new ReadDataResult()
                {
                    IsSucceded = false
                };
            }

            return new ReadDataResult()
            {
                IsSucceded = true,
                Value = responseModel.Value
            };
        }
    }
}
