using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace STVRogue {
    public class Gameplay {
        private string dt;
        private FileStream fs;
        private GameState gs;

        public Gameplay(string file) {

        }

        public Gameplay(GameState gs) {
            dt = DateTime.Now.ToString().Replace(':', '-');
            this.gs = gs;
        }

        public void CreateFile() {
            string path = @"C:\temp\";
            using (FileStream fs = File.Create(path + "save_game " + dt + ".txt")) {
                
                byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        // Reset recorded game play back to turn 0
        public void Reset () {
        }

        public void ReplayTurn() {
        }

        public GameState GetState() {
            return gs; 
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

        public GameState(string save) {

        }    

        public GameState(Game g) {
            this.g = g;
            this.playerName = "Player name: " + g.player.id;
            this.playerItems = BagToString(g.player.bag);
            this.playerLocation = "Player Location: " + NodeToString(g.player.location);
            this.turn = "Turn: " + g.turn.ToString();
            this.packLocations = PacksToString(g.packs);
        }

        public override string ToString() {
            return playerName + "\n" + "------------------" + "\n"
                + playerItems + "\n" + "------------------" + "\n"
                + playerLocation + "\n" + "------------------" + "\n"
                + turn + "\n" + "------------------" + "\n"
                + packLocations;
        }

        private string BagToString(List<Item> items) {
            string result = "Bag content: ";
            for(int i = 0; i < items.Count; ++i) {
                result += i.ToString() + ": " + (items[i].IsCrystal ? "crystal" : "hp_potion")
                + " , used: " 
                + (items[i].IsUsed() ? "yes" : "no")
                + "\n";
            }
            return result;
        }

        private string PacksToString(List<Pack> packs) {
            string result = "Monster locations: " + "\n" + "------------------" + "\n";
            for (int i = 0; i < packs.Count; ++i) {
                result += "Location " + (packs[i].location.id) + ": " + "\n" + "------------------" + "\n"
                + " #monsters: "
                + (packs[i].members.Count) + "\n";
                for (int j = 0; j < packs[i].members.Count; ++j) {
                    Monster m = packs[i].members[j];
                    result += "Monster: " + j.ToString()
                        + " with HP: " + m.GetHP().ToString()
                        + " and attackrating: " + m.AttackRating + "\n";
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
