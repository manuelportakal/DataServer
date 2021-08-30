using DataServer.ClientLibrary.Models;
using DataServer.Common.Models.AgentModels;
using DataServer.Common.Models.EntryModels;
using DataServer.Common.ResponseObjects;
using DataServer.Common.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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


        public async Task<RegisterResult> Register(string name, string agentCode, List<EntryRequestModel> entries)
        {
            var agentRandomNumber = new Random(Environment.TickCount).Next(1000, 9999);
            var request = new RegisterAgentRequestModel()
            {
                Name = name,
                AgentCode = agentCode,
                Entries = entries,
                RandomNumber = agentRandomNumber
            };

            try
            {
                var jsonString = JsonSerializer.Serialize(request);
                var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(Constants.HttpClientTimeoutSeconds);
                var requestUrl = $"{Constants.ApiUrl}/Agents";
                var result = await client.PostAsync(requestUrl, stringContent);
                var responseAsJsonString = await result.Content.ReadAsStringAsync();
                var responseModel = JsonSerializer.Deserialize<CustomResponse<RegisterAgentResponseModel>>(responseAsJsonString, serializerSettings);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new RegisterResult()
                    {
                        Id = null,
                        ErrorMessage = responseModel?.Error?.Message ?? null,
                        IsSucceded = false,
                    };
                }

                var agentSecurityToken = _securityService.CreateAgentKey(responseModel.Data.Id, responseModel.Data.ServerNumber, agentRandomNumber);
                Console.WriteLine($"created agent security token = {agentSecurityToken}");
                return new RegisterResult()
                {
                    Id = responseModel.Data.Id,
                    IsSucceded = true,
                    AgentSecurityToken = agentSecurityToken
                };
            }
            catch (Exception ex)
            {
                return new RegisterResult()
                {
                    Id = null,
                    ErrorMessage = $"Exception thrown: {ex.Message}",
                    IsSucceded = false,
                };
            }
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

            try
            {
                request.RequestSignature = _securityService.CalculateSignature(request.RequestData, agentSecurityToken);

                var jsonString = JsonSerializer.Serialize(request);
                var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(Constants.HttpClientTimeoutSeconds);
                var requestUrl = $"{Constants.ApiUrl}/Entries/Write";
                var result = await client.PostAsync(requestUrl, stringContent);
                var responseAsJsonString = await result.Content.ReadAsStringAsync();
                var responseModel = JsonSerializer.Deserialize<CustomResponse<WriteEntryResponseModel>>(responseAsJsonString, serializerSettings);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new WriteDataResult()
                    {
                        ErrorMessage = responseModel?.Error?.Message ?? null,
                        IsSucceded = false
                    };
                }

                return new WriteDataResult()
                {
                    IsSucceded = true
                };
            }
            catch (Exception ex)
            {
                return new WriteDataResult()
                {
                    ErrorMessage = $"Exception thrown: {ex.Message}",
                    IsSucceded = false
                };
            }
        }

        public async Task<ReadDataResult> ReadData(string dataCode)
        {
            var request = new ReadEntryRequestModel()
            {
                DataCode = dataCode,
            };

            try
            {
                var jsonString = JsonSerializer.Serialize(request);
                var stringContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(Constants.HttpClientTimeoutSeconds);
                var requestUrl = $"{Constants.ApiUrl}/Entries/Read";
                var result = await client.PostAsync(requestUrl, stringContent);
                var responseAsJsonString = await result.Content.ReadAsStringAsync();
                var responseModel = JsonSerializer.Deserialize<CustomResponse<ReadEntryResponseModel>>(responseAsJsonString, serializerSettings);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new ReadDataResult()
                    {
                        ErrorMessage = responseModel?.Error?.Message ?? null,
                        IsSucceded = false
                    };
                }

                return new ReadDataResult()
                {
                    IsSucceded = true,
                    Value = responseModel.Data.Value
                };
            }
            catch (Exception ex)
            {
                return new ReadDataResult()
                {
                    ErrorMessage = $"Exception thrown: {ex.Message}",
                    IsSucceded = false
                };
            }
        }
    }
}
