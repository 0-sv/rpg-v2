using System.Collections.Generic;
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
            g = new Game(20, 1, 50);
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
            Game withPacks = new Game(5, 2, 10);
            Gamestate withPacksGS = new Gamestate(withPacks);
            Savegame savegame = new Savegame(withPacksGS);
            savegame.SaveTurn();
            Gamestate openFile = new Gamestate(withPacks, savegame.OpenFile(0));
            Assert.Equal(withPacks.dungeon.packs, openFile.g.dungeon.packs);
        }

        [Fact]
        public void GameStateIsSameAsBefore() {
            Game withItems = new Game(5, 2, 20);
            withItems.dungeon.player.PickUp(new HealingPotion("0"));
            withItems.dungeon.player.PickUp(new HealingPotion("1"));
            withItems.dungeon.player.PickUp(new HealingPotion("2"));
            withItems.dungeon.player.PickUp(new HealingPotion("3"));
            withItems.dungeon.turn = 5;
            Gamestate withItemsGS = new Gamestate(withItems);
            Savegame save = new Savegame(withItemsGS);
            save.SaveTurn();
            Gamestate fromFile = new Gamestate(g, save.OpenFile(5));
            Assert.Equal(withItemsGS.g.dungeon.player.location.id, fromFile.g.dungeon.player.location.id);
            Assert.Equal(withItemsGS.BagToString(g.dungeon.player.bag), fromFile.BagToString(g.dungeon.player.bag));
            
            Assert.Equal(withItemsGS.g.dungeon.turn, fromFile.g.dungeon.turn);
        }
	}
}
