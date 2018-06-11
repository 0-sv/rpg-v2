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
        public int difficultyLevel;
        public int nodeCapacityMultiplier;
        public int numberOfMonsters;
		
		public Game(int difficultyLevel, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");
			this.difficultyLevel = difficultyLevel;
            this.nodeCapacityMultiplier = nodeCapacityMultiplier;
            this.numberOfMonsters = numberOfMonsters;
			player = new Player("Bram");
			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters, player);
			player.Move(dungeon.startNode);
			
			amountOfMonstersPerLevel = new int[difficultyLevel + 1];

			packs = dungeon.packs;
			items = dungeon.items;
            turn = 0;
		}
		// Update() en turn++ verplaatst naar Form1.cs

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
