using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Gamelogic;
using STVRogue.Utils;
using System.Windows.Forms;

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
        public int turn;
		
		public Game(int difficultyLevel, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");
			
			player = new Player("ShouldBeReplacedWithUI::EnterName");
			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters, player);
			player.Move(dungeon.startNode);

			
			amountOfMonstersPerLevel = new int[difficultyLevel + 1];
			
			packs = dungeon.addpacks(numberOfMonsters);
			items = dungeon.additems(dungeon.calculateTotalMonsterHP(), dungeon.bridges[dungeon.bridges.Length - 1], player.HPbase);
            turn = 0;
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
			//	UpdateUI();
			//	Command normalCommand = new Command(player, Console.ReadKey().Key);
			//	normalCommand.Execute();
			}
            turn++;
		}
		/*
		public void UpdateUI()
		{
			for(int i = 0;i<player.location.neighbors.Count;i++)
			{
				form.button1.Text = "Go to Node " + dungeon.nodeList[i].id;
			}
		}
		*/
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
