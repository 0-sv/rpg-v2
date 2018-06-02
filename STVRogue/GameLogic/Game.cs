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
		private Player player;
		private Dungeon dungeon;
		
		/* This creates a player and a random dungeon of the given difficulty level and node-capacity
         * The player is positioned at the dungeon's starting-node.
         * The constructor also randomly seeds monster-packs and items into the dungeon. The total
         * number of monsters are as specified. Monster-packs should be seeded as such that
         * the nodes' capacity are not violated. Furthermore the seeding of the monsters
         * and items should meet the balance requirements stated in the Project Document.
         */
		public Game(int difficultyLevel, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");
			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters, player);
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
