using Guestline.Games.Battleships.Server.DTOs.Requests;
using Guestline.Games.Battleships.Server.DTOs.Responses;
using Guestline.Games.Battleships.Server.Models.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Common;

namespace Guestline.Games.Battleships.Server.IntegrationTests
{
    public class BattleshipsGameIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BattleshipsGameIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _factory.CreateClient();
        }

        [Fact]
        public async Task StartGame_ReturnsExpectedResponse()
        {
            // Arrange
            HubConnection connection = await CreateConnection();

            // Act
            var response = await connection.InvokeAsync<StartNewGameResponse>("StartGame");

            // Assert
            Assert.NotNull(response);
            Assert.NotEqual(default, response.GameId);
        }


        [Fact]
        public async Task TryHit_GivenRequest_ReturnsExpectedResponse()
        {
            // Arrange
            HubConnection connection = await CreateConnection();
            var startGameResponse = await connection.InvokeAsync<StartNewGameResponse>("StartGame");

            // Act 
            int hit = 0;
            int miss = 0;
            int sink = 0;
            int gameOver = 0;

            for(int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++) 
                {
                    var hitRequest = new TryHitRequest()
                    {
                        GameId = startGameResponse.GameId,
                        Position = new Models.Position(x, y)
                    };

                    var response = await connection.InvokeAsync<TryHitResponse>("TryHit", hitRequest);

                    if(response.HitResult == HitResult.Hit.ToString())
                        hit++;
                    if (response.HitResult == HitResult.Miss.ToString())
                        miss++;
                    if (response.HitResult == HitResult.Sink.ToString())
                        sink++;
                    if (response.HitResult == HitResult.GameOver.ToString())
                        gameOver++;
                }
            }

            //Assert
            Assert.Equal(1, gameOver);
            Assert.Equal(2, sink); //Three ships sink but we will get 2 sink response and one gameOver
            Assert.Equal(10, hit); //There were 13 hit, but last hit for each ship results in sink event
            Assert.Equal(87, miss);
        }

        private async Task<HubConnection> CreateConnection()
        {
            var server = _factory.Server;

            var connection = await StartConnectionAsync(server.CreateHandler(), "battleshipsHub");

            connection.Closed += async error =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            return connection;
        }

        private static async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler, string hubName)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://localhost/{hubName}", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }
    }
}