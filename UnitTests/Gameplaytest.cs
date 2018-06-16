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
        Gameplaytest() {
            g = new Game(5, 1, 5);
            gs = new Gamestate(g);
            savegame = new Savegame(gs);
        }
        [Fact]
        public void SaveFileIsSameAsOpenFile() {
            savegame.SaveTurn();
            Console.WriteLine()
        }
    }
}
