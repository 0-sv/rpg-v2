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
	public class GamestateTest {
        Game g;
        Gamestate gs;
        List<Item> bag;
        List<Pack> packs;
        string contentOfBag;
        string packString;
        public GamestateTest() {
            g = new Game(5, 1, 2);
            gs = new Gamestate(g);
            bag = new List<Item>();
            bag.Add(new HealingPotion("0"));
            bag.Add(new Crystal("1"));
            bag.Add(new HealingPotion("2"));
            bag.Add(new Crystal("3"));
            bag[0].used = true;
            bag[1].used = true;
            packs = gs.GetGame().dungeon.packs;
            contentOfBag = "Number of items: 4;\nCrystal 0: no;, used: yes;\nCrystal 1: yes;, used: yes;\nCrystal 2: no;, used: no;\nCrystal 3: yes;, used: no;\n";
            packString = "Pack count: 1;\nLocation: 23 \nnumOfPacks: 2;\n------------------\nMonster: 0 with HP: 4; and attackrating: 1;\nMonster: 1 with HP: 3; and attackrating: 1;\n------------------";
            gs.file = contentOfBag + packString;

        }

        [Fact]
        public void GetSingleValueFromFileWorks() {
            gs.file = "------SOMETHING: hello123;";

            string answer = "hello123";

            Assert.Equal(answer, gs.GetSingleValueFromFile("SOMETHING: "));
        }

        [Fact]
        public void GetSingleValueFromFileWorks_AlsoWithInt() {
            gs.file = "SOMEINTEGER: 1234;";
            
            int answer = 1234;
            
            Assert.Equal(answer, gs.GetVal("SOMEINTEGER: "));
        }

        [Fact]
        public void BagToStringWorks() {
            Assert.Equal(contentOfBag,
                gs.BagToString(bag));
        }

        [Fact]
        public void ExtractBagWorks() {
            Assert.Equal(gs.BagToString(bag), 
                gs.BagToString(gs.ExtractBag()));
        }

        [Fact]
        public void WriteSaveGameForPackString() {
            Savegame savegame = new Savegame(gs);
            savegame.SaveTurn();
            Assert.Equal(0, gs.GetGame().dungeon.turn);
        }

        /*[Fact]
        public void PackToStringWorks() {
            // Skip for now because pack placement is non-deterministic. 
            Assert.Equal(packString,
                gs.PacksToString(packs));
        }
        */
        [Fact]
        public void ExtractPacksWorks() {
            Assert.Equal(gs.PacksToString(packs),
                gs.PacksToString(gs.ExtractPacks()));
        }
	}
}
