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
        private string path = @"C:\temp\";

        public Gameplay(string file) {

        }

        public Gameplay(GameState gs) {
            dt = DateTime.Now.ToString().Replace(':', '-');
            this.gs = gs;
        }

        public void CreateFile() {
            using (FileStream fs = File.Create(path + "save_game " + dt + ".txt")) {
                byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        public string SaveFile() {
            string file = "";
            using (FileStream fs = File.OpenRead(path)) {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0) {
                    file += temp.GetString(b);
                }
            }
            return file;
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
        // Dungeon representation to do: 
        // - restore original nodelist:
        //      - #nodes and their connectivity
        private Game g;
        private string file;
        private string difficultyLevel;
        private string nodeCapacityMultiplier;
        private string numberOfMonsters;


        public GameState(string file) {
            this.file = file;
        }    

        public GameState(Game g) {
            this.g = g;
            this.playerName = g.player.id;
            this.playerItems = BagToString(g.player.bag);
            this.playerLocation =  NodeToString(g.player.location);
            this.turn =  g.turn.ToString();
            this.packLocations = PacksToString(g.packs);
            this.difficultyLevel = g.difficultyLevel.ToString();
            this.nodeCapacityMultiplier = g.nodeCapacityMultiplier.ToString();
            this.numberOfMonsters = g.numberOfMonsters.ToString();
        }

        public Game ToGame() {
            return new Game(1, 1, 1);
        }

        public override string ToString() {
            return "Player name: " + playerName + "\n" 
                + "Bag content: " + playerItems + "\n" 
                + "Player Location: " + playerLocation + "\n" 
                + "Turn: " + turn + "\n" 
                + "Pack locations: " + packLocations
                + "Difficulty level :" + difficultyLevel
                + "Node capacity multiplier: " + nodeCapacityMultiplier
                + "Number of monsters: " + numberOfMonsters;

        }

        private string BagToString(List<Item> items) {
            string result = "";
            for(int i = 0; i < items.Count; ++i) {
                result += i.ToString() + ": " + (items[i].IsCrystal ? "crystal" : "hp_potion")
                + " , used: " 
                + (items[i].IsUsed() ? "yes" : "no")
                + "\n";
            }
            return result;
        }

        private string PacksToString(List<Pack> packs) {
            string result = "";
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
