using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;

namespace STVRogue {
    public class Gameplay {
        private DateTime dt;
        private FileStream fs;
        private GameState g;

        public Gameplay(GameState g) {
            dt = DateTime.Now;
            this.g = g;
        }

        public void WriteToFile() {
            fs = new FileStream("Save game " + dt.ToShortDateString() + ".txt",
                FileMode.CreateNew,
                FileAccess.Read,
                FileShare.Read);
        }

        // Reset recorded game play back to turn 0
        public void Reset () {
        }

        public void replayTurn() {
        }

        public void getState() {
        }

        public void Replay(Specification s) {
        }
    }

    public class GameState {
        private string playerName;
        private List<Item> playerItems;
        private Node playerLocation; 
        private int turn;
        private KeyValuePair<int, string> monsterLocations;
        // TO DO: how to represent dungeon? Maybe after UI implementation it is more clear.
    
        private Game g;
        public GameState(Game g) {
            this.g = g;
            this.playerName = g.player.id;
            this.playerItems = g.player.bag;
            this.playerLocation = g.player.location;
            this.turn = g.turn;
        }

    }
}
