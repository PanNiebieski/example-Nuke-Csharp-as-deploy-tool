using Xunit;

namespace GamesAPI.UnitTest
{
    public class TestGame
    {
        [Fact]
        public void CanCreateGame()
        {
            Game game = new(1, "A", 100, 2000);

            var (a, b, c, d) = game;

            Assert.Equal(1, a);
            Assert.Equal("A", b);
            Assert.Equal(100, c);
            Assert.Equal(2000, d);
        }
    }
}
