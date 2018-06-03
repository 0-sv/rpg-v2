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
		
		public Game(uint difficultyLevel, uint nodeCapacityMultiplier, uint numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");

			player = new Player("ShouldBeReplacedWithUI::EnterName");
            player.Move(dungeon.startNode);

			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier);
			amountOfMonstersPerLevel = new uint[difficultyLevel + 1];
			
			packs = addpacks(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters);
			items = additems(calculateTotalMonsterHP(), dungeon.bridges[dungeon.bridges.Length - 1], player.HPbase);
		}
		
		public void Update() {
			if (player.location.packs.Any()) {
				Rule alert = new RAlert(dungeon);
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
