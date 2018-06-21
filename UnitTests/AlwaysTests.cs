using STVRogue;
using STVRogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
	public class AlwaysTests
	{
		Game g;
		Gamestate gs;
		Savegame savegame;
		string data;
		string path;
		int[] gameturns;
		public AlwaysTests()
		{
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
                path = @"C:/Users/Gebruiker/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game" + k + "/game" + k + "_turn";
                Always always = new Always((G => G.dungeon.player.HP >= 0));
                for (int i = 0; i < 26; i++) {
                    data = savegame.OpenFile(i, path);
                    Gamestate gamestate = new Gamestate(g, data);
                    Assert.True(always.test(g));
                }
            }
			*/

		[Fact]
		public void test_RZone()
		{
			int l, c, m;
			int counter = 0;
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
				for (int i = 0; i < gameturns[k - 1]; i++)
				{
					counter = 0;
					data = savegame.OpenFile(i, path);
					Gamestate gamestate = new Gamestate(g, data);
					foreach (Pack pack in gamestate.g.dungeon.packs)
					{
						Always always = new Always((G => G.dungeon.packs[counter++].zone == G.dungeon.CurrentLevel(pack.location)));				
						Assert.True(always.test(gamestate.g));

					}
				}
			}
		}

		[Fact]
		public void test_RNode()
		{
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
				for (int i = 0; i < gameturns[k - 1]; i++)
				{
					data = savegame.OpenFile(i, path);
					Gamestate gamestate = new Gamestate(g, data);
					foreach (Node node in gamestate.g.dungeon.nodeList)
					{
						monstersOnNode = 0;
						foreach (Pack pack in node.packs)
						{
							monstersOnNode += pack.members.Count();
						}
						Always always = new Always((G => monstersOnNode <= G.dungeon.multiplier * (G.dungeon.Level(node) + 1)));
						
						Assert.True(always.test(gamestate.g));
					}
				}
			}


		}
		[Fact]
		public void test_REndZone()
		{
			int counter = 0;
			int l, c, m;
			bool REndZone = false;
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
				for (int j = 0; j < gameturns[k - 1]-1; j++)
				{
					counter = 0;
					data = savegame.OpenFile(j, path);
					Gamestate gamestate1 = new Gamestate(g, data);
					data = savegame.OpenFile(j + 1, path);
					Gamestate gamestate = new Gamestate(g, data);
					if (gamestate1.g.dungeon.Level(gamestate1.g.dungeon.player.location) == l)
					{
						REndZone = true;
					}
					for (int i = gamestate1.g.dungeon.nodeList.Count() - 1; i > gamestate1.g.dungeon.nodeList.Count() - 6; i--)
					{
						counter = 0;
						if (REndZone)
						{ // check if packs do not move away from the player when the endzone has been reached
							foreach (Pack pack in gamestate1.g.dungeon.nodeList[i].packs)
							{								
								
								Always always = new Always((G => G.dungeon.Shortestpath(pack.location, G.dungeon.player.location).Count() <= G.dungeon.Shortestpath(gamestate.g.dungeon.nodeList[i].packs[counter++].location, G.dungeon.player.location).Count()));
							//	Console.WriteLine(gamestate1.g.dungeon.Shortestpath(pack.location, gamestate1.g.dungeon.player.location).Count());
								Assert.True(always.test(gamestate1.g));
							}
						}
						
					}
				}

			}

		}
		
		[Fact]
		public void test_RAlert()
		{
			int level, counter = 0;
			int[] turns = new int[4];
			// 4 turns wherein combat starts
			turns[0] = 0;
			turns[1] = 6;
			turns[2] = 31;
			turns[3] = 39;
				g = new Game(5,2,30, true);
				gs = new Gamestate(g);
				savegame = new Savegame(gs);
				path = @"C:/Users/win7/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game9/game9_turn";
			for (int j = 0; j < turns.Length; j++)
			{
				for (int h = j; h < j + 2; h++)
				{
					data = savegame.OpenFile(h, path);
					Gamestate gamestate1 = new Gamestate(g, data);
					data = savegame.OpenFile(h + 1, path);
					Gamestate gamestate = new Gamestate(g, data);
					level = gamestate1.g.dungeon.CurrentLevel(gamestate1.g.dungeon.player.location);
					for (int i = (level * 4); i < (level * 4) + 3; i++)
					{
						counter = 0;
						foreach (Pack pack in gamestate1.g.dungeon.nodeList[i].packs)
						{
							Always always = new Always((G => G.dungeon.Shortestpath(pack.location, G.dungeon.player.location).Count() <= G.dungeon.Shortestpath(gamestate.g.dungeon.nodeList[i].packs[counter++].location, G.dungeon.player.location).Count()));
							Assert.True(always.test(gamestate1.g));
						}
					}
				}
			}
			
		}
			



	}
}