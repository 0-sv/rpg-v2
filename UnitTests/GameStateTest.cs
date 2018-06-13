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
	public class GameStateTest
	{
		Game g = new Game(3, 3, 5);
		HealingPotion hp = new HealingPotion("1");
		Crystal c = new Crystal("2");

		[Fact]
		public void GamePlayCreatesSaveGame()
		{
			GameState gs = new GameState(g);
			GamePlay gp = new GamePlay(gs);

			g.player.bag.Add(hp);
			g.player.bag.Add(c);

			Assert.Contains(hp, g.player.bag);
			Assert.Contains(c, g.player.bag);

			gp.CreateSaveGameFile();
			gp.SaveTurnToSaveGameFile();
		}
		[Fact]
		public void TestRNode()
		{
			GameState gs = new GameState(g);
			GamePlay gp = new GamePlay(gs);
			int allmonsters = 0;
			foreach (Node node in g.dungeon.nodeList)
			{
				foreach (Pack pack in node.packs)
				{
					allmonsters += pack.members.Count();
				}
				Assert.True(allmonsters <= g.dungeon.multiplier * (g.dungeon.CurrentLevel(node) + 1));

			}
		}
		[Fact]
		public void TestRZone()
		{
			GameState gs = new GameState(g);
			GamePlay gp = new GamePlay(gs);
			foreach (Pack pack in g.dungeon.packs)
			{
				Assert.True(g.dungeon.CurrentLevel(pack.location) == pack.zone);
			}
		}
	}
}
