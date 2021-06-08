using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace GamesAPI.FunctionalTest
{
    public class GameEndpointTest
    {
        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new("http://localhost:5000")
        };

        [Fact]
        public async Task ListGames()
        {
            var games = await _httpClient.GetFromJsonAsync<List<Game>>("/games");

            Assert.NotNull(games);
            Assert.Equal(4, games.Count);

        }

        [Fact]
        public async Task GetGame()
        {
            var game = await _httpClient.GetFromJsonAsync<Game>("/game/1");

            Assert.NotNull(game);
            Assert.Equal(1, game.Id);

        }

    }
}
