using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
	public class Game
	{
		public Player player;
		public List<Pack> packs;
		public List<Item> items;
		public Dungeon dungeon;
		public uint[] amountOfMonstersPerLevel;
		Random randomnum = new Random();
		/* This creates a player and a random dungeon of the given difficulty level and node-capacity
         * The player is positioned at the dungeon's starting-node.
         * The constructor also randomly seeds monster-packs and items into the dungeon. The total
         * number of monsters are as specified. Monster-packs should be seeded as such that
         * the nodes' capacity are not violated. Furthermore the seeding of the monsters
         * and items should meet the balance requirements stated in the Project Document.
         */
		public Game(uint difficultyLevel, uint nodeCapacityMultiplier, uint numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");
			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier);
			amountOfMonstersPerLevel = new uint[difficultyLevel + 1];
			packs = addpacks(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters);
			player = new Player("1");
            player.Move(dungeon.startNode);
			int totalMonsterHP = calculateTotalMonsterHP();
			items = additems(totalMonsterHP, dungeon.bridges[dungeon.bridges.Length - 1], player.HPbase);
		}
		public List<Pack> addpacks(uint difficultyLevel, uint nodeCapcityMultiplier, uint numberOfMonsters)
		{ // add packs to the dungeon
			uint maxMonstersOnThisLevel, monstersOnThisLevel = 0, monstersInDungeon = 0;
			int pack_id = -1, count = 0, min, numbers;
			List<int> nodesOnThisLevelInRandomOrder;

			int amountOfMonsters = 0;
			List<Pack> packs = new List<Pack>();
			for (uint i = 0; i < dungeon.bridges.Length - 1; i++)
			{
				min = dungeon.bridges[i] + 1;
				numbers = dungeon.bridges[i + 1] - min + 1;
				nodesOnThisLevelInRandomOrder = Enumerable.Range(min, numbers).OrderBy(x => randomnum.Next()).ToList();
				if (i < dungeon.bridges.Length - 2)
				{ // calculate the maximum amount of monsters allowed on this level
					maxMonstersOnThisLevel = (2 * (i + 1) * numberOfMonsters) / ((difficultyLevel + 2) * (difficultyLevel + 1));
				}
				else
				{// add all monsters that are left to the final level
					maxMonstersOnThisLevel = numberOfMonsters - monstersInDungeon;
				}
				count = 0;
				monstersOnThisLevel = 0;
				// add monsters to this level while the limit hasn't been reached yet
				while (monstersOnThisLevel < maxMonstersOnThisLevel)
				{

					amountOfMonsters = (int)Math.Min(maxMonstersOnThisLevel - monstersOnThisLevel, (nodeCapcityMultiplier * (i + 1)));

					monstersOnThisLevel += (uint)amountOfMonsters;
					// create a new pack and update its location
					Pack pack = new Pack(pack_id++.ToString(), (uint)amountOfMonsters);
					pack.location = dungeon.nodeList[nodesOnThisLevelInRandomOrder[count]];
					dungeon.nodeList[nodesOnThisLevelInRandomOrder[count++]].packs.Add(pack);
					packs.Add(pack);
					if (count > nodesOnThisLevelInRandomOrder.Count - 1)
					{ // throw exception if amount of monsters and nodeCapacityMultiplier are out of proportion
						throw new GameCreationException("Amount of monsters and nodeCapacityMultiplier are not compatible");
					}
				}
				amountOfMonstersPerLevel[i] = monstersOnThisLevel;

				monstersInDungeon += monstersOnThisLevel;
			}
			return packs;
		}

		public int calculateTotalMonsterHP()
		{ // calculate the hp of all monsters, this is later used to determine the amount of HealingPotions in the dungeon.
			int totalHP = 0;
			foreach (Pack p in packs)
			{
				foreach (Monster m in p.members)
				{
					totalHP += m.GetHP();
				}
			}
			return totalHP;
		}

		public List<Item> additems(int totalMonsterHP, int nodeMax, int playerHP)
		{ // add items to the dungeon
			List<Item> items = new List<Item>();
			// calculate the hp limit
			int HPlimit = (int)(totalMonsterHP * 0.8);
			int itemAndPlayerHP = playerHP;
			int item_id = -1;
			int count = 0;
			List<int> allNodesInRandomOrder = Enumerable.Range(1, nodeMax - 1).OrderBy(x => randomnum.Next()).ToList();
			while ((itemAndPlayerHP + 11) < HPlimit && count < allNodesInRandomOrder.Count)
			{ // add healingpotions until the limit is reached
				HealingPotion item = new HealingPotion(item_id++.ToString());
				item.location = dungeon.nodeList[allNodesInRandomOrder[count]];
				dungeon.nodeList[allNodesInRandomOrder[count++]].items.Add(item);
				itemAndPlayerHP += item.HPvalue;
				items.Add(item);
			}
			while (count < allNodesInRandomOrder.Count - 1)
			{ // for now we decided every node has a 1 in 20 chance to contain a Crystal
				if (randomnum.Next(1, 21) == 5)
				{
					Crystal item = new Crystal(item_id++.ToString());
					item.location = item.location = dungeon.nodeList[allNodesInRandomOrder[count]];
					items.Add(item);
				}
				count++;
			}
			return items;
		}

		public void Update()
		{
			if (player.location.packs.Any())
				player.location.Combat(player);
			else
			{
				Command normalCommand = new Command(player, Console.ReadKey().Key);
				normalCommand.Execute();
			}
		}
	}

	public class GameCreationException : Exception
	{
		public GameCreationException() { }
		public GameCreationException(String explanation) : base(explanation)
		{
			explanation = "The dungeon is not a valid dungeon!";
		}
	}
}
