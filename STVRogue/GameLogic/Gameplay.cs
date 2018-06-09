using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace STVRogue {
    public class Gameplay {
        private DateTime dt;
        private FileStream fs;
        private GameState gs;

        public Gameplay(GameState gs) {
            dt = DateTime.Now;
            this.gs = gs;
        }

        public void CreateFile() {
            fs = new FileStream(Directory.GetCurrentDirectory() + "save_game " + dt.ToShortDateString() + ".txt",
                FileMode.CreateNew,
                FileAccess.Read,
                FileShare.Read);
        }

        public void SaveGameState () {
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(gs.ToString());
            sw.Close();
        }

        // Reset recorded game play back to turn 0
        public void Reset () {
        }

        public void ReplayTurn() {
        }

        public void GetState() {
        }

        public void Replay(Specification s) {
        }
    }

    public class GameState {
        private string playerName;
        private string playerItems;
        private string playerLocation; 
        private string turn;
        private string packLocations;
        // TO DO: how to represent dungeon? Maybe after UI implementation it is more clear.
    
        private Game g;
        public GameState(Game g) {
            this.g = g;
            this.playerName = g.player.id;
            this.playerItems = BagToString(g.player.bag);
            this.playerLocation = NodeToString(g.player.location);
            this.turn = g.turn.ToString();
            this.packLocations = PacksToString(g.packs);
        }

        public override string ToString() {
            return playerName + playerItems + playerLocation + turn + packLocations;
        }

        private string BagToString(List<Item> items) {
            string result = "Bag content: ";
            for(int index = 0; index < items.Count; ++index) {
                result += index.ToString() + ": " + (items[index].IsCrystal ? "crystal" : "hp_potion")
                + " , used: " 
                + (items[index].IsUsed() ? "yes" : "no")
                + "\n";
            }
            return result;
        }

        private string PacksToString(List<Pack> packs) {
            string result = "Monster locations: " + "\n";
            for (int index = 0; index < packs.Count; ++index) {
                result += "Location: " + index.ToString() + ": " + (packs[index].location.id)
                + ": #monsters: "
                + (packs[index].members.Count)
                + ", monster: ";
                for (int j = 0; j < packs[index].members.Count; ++j) {
                    Monster m = packs[index].members[j];
                    result += j.ToString()
                        + "with HP: " + m.GetHP().ToString() 
                        + " and attackrating: " + m.AttackRating;
                }
                result += "\n";
            }
            return result;
        }

        private string NodeToString(Node n) {
            return n.id;
        }
    }
}
