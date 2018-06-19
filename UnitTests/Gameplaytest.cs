using STVRogue;
using STVRogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests {
    public class Gameplaytest {
        Game g;
        Gamestate gs;
        Savegame savegame;
        public Gameplaytest() {
            g = new Game(5, 1, 5);
            g.dungeon.player.bag.Add(new HealingPotion("0"));
            gs = new Gamestate(g);
            savegame = new Savegame(gs);
        }

        [Fact]
        public void SaveFileIsSameAsOpenFile() {
            savegame.SaveTurn();
            string savedFile = savegame.OpenFile(0, "");
            Assert.Contains("Bag content", savedFile);
            Assert.Contains("Player location", savedFile);
            Assert.Contains("END", savedFile);
        }
    }
}