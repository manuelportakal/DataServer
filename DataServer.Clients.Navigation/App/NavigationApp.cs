using DataServer.ClientLibrary;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace DataServer.Clients.Navigation
{
    public class NavigationApp
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
            {
                Console.WriteLine("Agent registering failed");
            }
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
                    var startTime = DateTime.Now;
                    var readResponse = await serverClient.ReadData("compass-direction");
                    if (!readResponse.IsSucceded)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    string turnDirection = Evaluate(Convert.ToInt32(readResponse.Value));
                    var stopTime = DateTime.Now;

                    var writeResponse = await serverClient.WriteData(agentId, Constants.AgentCode, Constants.DataCode, turnDirection, agentSecurityToken);

                    dataStore.SetTurnDirection(turnDirection);

                    var diff = stopTime - startTime;

                    if (writeResponse.IsSucceded)
                        Console.WriteLine($"Direction:{turnDirection} for {readResponse.Value}, total(ms)={diff.TotalMilliseconds}");
                    else
                        Console.WriteLine("Error: Could not write data to server");

                    Thread.Sleep(3000);
                }
            });
        }

        public string Evaluate(int compassDirection)
        {
            if (compassDirection <= 360 && compassDirection >= 180)
            {
                return "right";
            }
            else if (compassDirection > 0 && compassDirection < 180)
            {
                return "left";
            }
            else
            {
                return "unknown";
            }
        }
    }
}
