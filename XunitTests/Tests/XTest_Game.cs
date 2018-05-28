using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Game
	{
		[Fact]
		public void checkIfTooManyMonstersThrowsException()
		{
            Assert.Throws<GameCreationException> (() => new Game(5, 2, 300));
		}
		[Fact]
		public void checkIfMonstersSpawn()
		{
			Game game = new Game(10, 5, 50);
			foreach (Pack pack in game.packs)
			{
				Assert.NotEmpty(pack.members);
			}
		}
			
		[Fact]
		public void CheckIfMonsterBalancingHolds()
		{
			uint difficultyLevel = 10;
			uint nodeCapacityMultiplier = 5;
			uint numberOfMonsters = 50;
			uint monstersInDungeon = 0;
			uint maxMonstersOnThisLevel, amountOfMonstersOnThisLevel;
			Game game = new Game(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters);

			for (uint i = 0; i < game.dungeon.bridges.Length-1; i++)
			{
				amountOfMonstersOnThisLevel = game.amountOfMonstersPerLevel[i];
				if (i < game.dungeon.bridges.Length - 2)
				{
					maxMonstersOnThisLevel = (2 * (i + 1) * numberOfMonsters) / ((difficultyLevel + 2) * (difficultyLevel + 1));
					monstersInDungeon += maxMonstersOnThisLevel;
				}
				else
				{
					maxMonstersOnThisLevel = numberOfMonsters - monstersInDungeon;
				}
				Assert.True(amountOfMonstersOnThisLevel <= maxMonstersOnThisLevel);
			}
		}
		
		[Fact]
		public void CheckIfItemBalancingHolds()
		{
			Game game = new Game(10, 5, 50);
			int PlayerAndItemHP = game.player.HPbase;
			int MonsterHP = 0;
			for(int i = 0; i < game.items.Count; i++)
			{
				if(game.items[i] is HealingPotion)
				{
					HealingPotion potion = (HealingPotion)game.items[i];
					PlayerAndItemHP += potion.HPvalue;
				}


			}
			foreach (Pack p in game.packs)
			{
				foreach (Monster m in p.members)
				{
					MonsterHP += m.GetHP();
				}
			}
			MonsterHP = (int) (MonsterHP * 0.8);
			Assert.True(PlayerAndItemHP <= MonsterHP);

		}
		[Fact]

		public void CheckItemBalancingOnSmallDungeon()
		{
			Game game = new Game(2, 5, 1);
			for (int i = 0; i < game.items.Count; i++)
			{
				Assert.False(game.items[i] is HealingPotion);

			}
		}

		[Fact]
        public void XTest_disconnect_nodes()
        {
            Node node1 = new Node("1");
            Node node2 = new Node("2");
            Predicates p = new Predicates();
            node1.Connect(node2);
            Assert.True(p.isReachable(node1, node2));
            node1.Disconnect(node2);
            Assert.False(p.isReachable(node1, node2));
        }   
	}
}
