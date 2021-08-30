using DataServer.ClientLibrary;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace DataServer.Clients.Navigation
{
    public class CompassApp
    {
        public async Task Run()
        {
            await RegisterAgent();
            await SendData();
        }

        public async Task RegisterAgent()
        {
            var serverClient = new DataServerClient();
            var response = await serverClient.Register(Constants.AgentName, Constants.AgentCode, Constants.EntryCodes);
            if (response.IsSucceded)
            {
                var dataStore = new DataStore();
                dataStore.SetAgentId(response.Id.Value);
                dataStore.SetSecurityToken(response.AgentSecurityToken);

                Console.WriteLine($"Agent successfully registered. AgentId={response.Id} SecurityToken={response.AgentSecurityToken}");
            }
            else
                Console.WriteLine($"Agent registering failed. Reason={response.ErrorMessage}");
        }

        public async Task SendData()
        {
            await Task.Run(async () =>
            {
                var serverClient = new DataServerClient();
                var dataStore = new DataStore();
                var agentId = dataStore.GetAgentId();
                var agentSecurityToken = dataStore.GetSecurityToken();

                for (int i = 0; i < 100; i++)
                {
                    string value = new Random().Next(0, 360).ToString();

                    var startTime = DateTime.Now;
                    var response = await serverClient.WriteData(agentId, Constants.AgentCode, Constants.DataCode, value, agentSecurityToken);
                    var stopTime = DateTime.Now;

                    dataStore.SetValue(value);

                    var diff = stopTime - startTime;
                    if (response.IsSucceded)
                        Console.WriteLine($"New Value: {value}, IsSucceeded = {response.IsSucceded}, total(ms): {diff.TotalMilliseconds}");
                    else
                        Console.WriteLine($"Error: Could not write data to server. Reason={response.ErrorMessage}");

                    Thread.Sleep(1000);
                }
            });
        }
    }
}
