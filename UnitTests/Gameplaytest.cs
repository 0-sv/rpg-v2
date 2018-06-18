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
            string savedFile = savegame.OpenFile(0);
            Assert.Contains("Bram", savedFile);
            Assert.Contains("Bag content", savedFile);
            Assert.Contains("Player location", savedFile);
            Assert.Contains("Difficulty level", savedFile);
            Assert.Contains("Number of monsters", savedFile);
            Assert.Contains("END", savedFile);
        }
        /*
        [Fact]
        public void GamestateIsTheSameAfterOpening() {
            savegame.SaveTurn();
            string savedFile = savegame.OpenFile(0);
            Gamestate result = new Gamestate(g, savedFile);
            Assert.Equal(gs, result);
        } */
        
        [Fact]
        public void ExtractPacksWorks() {
            string openGame = savegame.OpenFile(0);
            Gamestate gsOpenFile = new Gamestate(g, openGame);
            Assert.Equal(gs.PacksToString(gs.GetGame().dungeon.packs),
                gs.PacksToString(gsOpenFile.ExtractPacks()));

        } 

        /*[Fact]
        public void PackToStringWorks() {
        // Skip for now because pack placement is non-deterministic. 
            Assert.Equal(packString,
            gs.PacksToString(packs));
        }
                */
    }
}
