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
		public Dungeon dungeon;
		
		public Game(int difficultyLevel, int nodeCapacityMultiplier, int numberOfMonsters, bool testing) {
            Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
                                   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");
            dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters, testing);
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
