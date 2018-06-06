using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Gamelogic;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
	public class Game
	{
		public Player player;
		public List<Pack> packs;
		public List<Item> items;
		public Dungeon dungeon;
		public int[] amountOfMonstersPerLevel;
		Random randomnum = new Random();
		
		public Game(int difficultyLevel, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");

			player = new Player("ShouldBeReplacedWithUI::EnterName");
            player.Move(dungeon.startNode);

			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters, player);
			amountOfMonstersPerLevel = new int[difficultyLevel + 1];
			
			packs = dungeon.addpacks(numberOfMonsters);
			items = dungeon.additems(dungeon.calculateTotalMonsterHP(), dungeon.bridges[dungeon.bridges.Length - 1], player.HPbase);
		}
		
		public void Update() {
			if (player.location.packs.Any()) {
                Zone z = new Zone(dungeon);
				RAlert alert = new RAlert(z);
				alert.AlertMonsters();
				player.location.Combat(player);
				alert.DeAlertMonsters();
			}
			else {
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
