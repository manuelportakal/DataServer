using System;
using System.Net.Http;
using System.Net;
using System.Runtime;
using DataServer.ClientLibrary.Models;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using DataServer.ClientLibrary.RequestResponseModels;
using System.Collections.Generic;

namespace DataServer.ClientLibrary
{
    public class DataServerClient
    {
        readonly JsonSerializerOptions serializerSettings = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<RegisterResult> Register(string name, string agentCode, List<string> entryCodes)
        {
            var request = new RegisterRequestModel()
            {
                Name = name,
                AgentCode = agentCode,
                EntryCodes = entryCodes
            };

            var jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            using var client = new HttpClient();
            var requestUrl = $"{Constants.ApiUrl}/Agents";
            var result = await client.PostAsync(requestUrl, stringContent);
            var responseAsJsonString = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<RegisterResponseModel>(responseAsJsonString, serializerSettings);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return new RegisterResult()
                {
                    Id = null,
                    IsSucceded = false
                };
            }

            return new RegisterResult()
            {
                Id = responseModel.Id,
                IsSucceded = true
            };
        }

        public async Task<WriteDataResult> WriteData(Guid agentId, string agentCode, string dataCode, string value)
        {
            var request = new WriteDataRequestModel()
            {
                AgentId = agentId,
                AgentCode = agentCode,
                DataCode = dataCode,
                Value = value
            };

            var jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            using var client = new HttpClient();
            var requestUrl = $"{Constants.ApiUrl}/Entries/Write";
            var result = await client.PostAsync(requestUrl, stringContent);
            var responseAsJsonString = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<WriteDataResponseModel>(responseAsJsonString, serializerSettings);

            if (result.StatusCode != HttpStatusCode.OK)
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
            var request = new ReadDataRequestModel()
            {
                DataCode = dataCode,
            };

            var jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
            using var client = new HttpClient();
            var requestUrl = $"{Constants.ApiUrl}/Entries/Read";
            var result = await client.PostAsync(requestUrl, stringContent);
            var responseAsJsonString = await result.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<ReadDataResponseModel>(responseAsJsonString, serializerSettings);

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
