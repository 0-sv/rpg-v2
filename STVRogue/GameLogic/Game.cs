using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic {
	public class Game {
		private Player player;
		private Dungeon dungeon;
		
		public Game(int difficultyLevel, int nodeCapacityMultiplier, int numberOfMonsters) {
			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters, player);
		}

		public void Update() {
			if (player.location.packs.Any()) {
				player.location.Combat(player);
			}
			else {
				Command normalCommand = new Command(player, Console.ReadKey().Key);
				normalCommand.Execute();
			}
		}
	}

	public class GameCreationException : Exception {
		public GameCreationException() { }
		public GameCreationException(String explanation) : base(explanation) {
			explanation = "The dungeon is not a valid dungeon!";
		}
	}
}
