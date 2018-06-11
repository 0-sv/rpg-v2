using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using STVRogue.GameLogic;
using STVRogue;

namespace UnitTests
{
    public class GameStateTest {
        Game g = new Game(3, 3, 5);
        HealingPotion hp = new HealingPotion("1");
        Crystal c = new Crystal("2");

        [Fact]
        public void GamePlayCreatesSaveGame() {
            GameState gs = new GameState(g);
            GamePlay gp = new GamePlay(gs);

            g.player.bag.Add(hp);
            g.player.bag.Add(c);

            Assert.Contains(hp, g.player.bag);
            Assert.Contains(c, g.player.bag);

            gp.CreateSaveGameFile();
            gp.SaveTurnToSaveGameFile();
        }
    }
}
