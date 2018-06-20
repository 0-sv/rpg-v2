using STVRogue;
using STVRogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests {
	public class AlwaysTests {
		Game g;
		Gamestate gs;
		Savegame savegame;
		string data;
		string path;
		int[] gameturns;
		public AlwaysTests() {
			gameturns = new int[8];
			gameturns[0] = 23;
			gameturns[1] = 32;
			gameturns[2] = 34;
			gameturns[3] = 67;
			gameturns[4] = 18;
			gameturns[5] = 17;
			gameturns[6] = 40;
			gameturns[7] = 76;
		}
		/*
        [Fact]
        public void test_HPplayer_never_negative() {
            for (int k = 1; k < 6; k++) {
                path = @"C:/Users/win7/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game" + k + "/game" + k + "_turn";
                Always always = new Always((G => G.dungeon.player.HP >= 0));
                for (int i = 0; i < 26; i++) {
                    data = savegame.OpenFile(i, path);
                    Gamestate gamestate = new Gamestate(g, data);
                    Assert.True(always.test(g));
                }
            }
			*/

		[Fact]
		public void test_RZone() {
			int l, c, m;
			for (int k = 1; k < 9; k++) {
				if (k < 5)
				{
					l = 5;
				}
				else
				{
					l = 10;
				}
				if (k % 2 == 0)
				{
					c = 3;
				}
				else
				{
					c = 2;
				}
				if (k == 3 || k == 4 || k == 7 || k == 8)
				{
					m = 50;
				}
				else
				{
					m = 30;
				}
				g = new Game(l, c, m, true);
				gs = new Gamestate(g);
				savegame = new Savegame(gs);

				path = @"C:/Users/win7/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game" + k + "/game" + k + "_turn";
				for (int i = 0; i < gameturns[k - 1]; i++) {
					foreach (Pack pack in g.dungeon.packs) {
						Always always = new Always((G => pack.zone == G.dungeon.CurrentLevel(pack.location)));
						data = savegame.OpenFile(i, path);
						Gamestate gamestate = new Gamestate(g, data);
						Assert.True(always.test(g));

					}
				}
			}
		}
		
        [Fact]
        public void test_RNode() {
            int monstersOnNode = 0;
			int l, c, m;
			for (int k = 1; k < 9; k++)
			{
				if (k < 5)
				{
					l = 5;
				}
				else
				{
					l = 10;
				}
				if (k % 2 == 0)
				{
					c = 3;
				}
				else
				{
					c = 2;
				}
				if (k == 3 || k == 4 || k == 7 || k == 8)
				{
					m = 50;
				}
				else
				{
					m = 30;
				}
				g = new Game(l, c, m, true);
				gs = new Gamestate(g);
				savegame = new Savegame(gs);
				path = @"C:/Users/win7/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game" + k + "/game" + k + "_turn";
				for (int i = 0; i < gameturns[k - 1]; i++) {

                    foreach (Node node in g.dungeon.nodeList) {
                        monstersOnNode = 0;
                        foreach (Pack pack in node.packs) {
                            monstersOnNode += pack.members.Count();
                        }
                        Always always = new Always((G => monstersOnNode <= G.dungeon.multiplier * (G.dungeon.Level(node) + 1)));
                        data = savegame.OpenFile(i, path);
                        Gamestate gamestate = new Gamestate(g, data);
                        Assert.True(always.test(g));

                    }
                }
            }
			
	
        }
		

    }
}