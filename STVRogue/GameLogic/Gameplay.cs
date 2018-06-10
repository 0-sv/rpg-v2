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
            this.gs = new GameState(file);
        }

        public Gameplay(GameState gs) {
            dt = DateTime.Now.ToString().Replace(':', '-');
            this.gs = gs;
        }

        public void CreateFile() {
            using (FileStream fs = File.Create(path + "save_game " + dt + gs.GetGame().turn.ToString() + ".txt")) {
                byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        public string OpenFile() {
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

        public void Reset () {
            // To do: save all games to a single file with separate turns. Then you get all properties from that turn.
        }

        public void ReplayTurn() {
            // To do: same as Reset(), but with subtracting just one turn. 
        }

        public GameState GetState() {
            return gs; 
        }

        public void Replay(Specification s) {
        }
    }

    public class GameState {
        /* Variables that make up a GameState */
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
        private int difficultyLevel;
        private int nodeCapacityMultiplier;
        private int numberOfMonsters;

        /* Variables that make up the prefix, e.g.: "Difficulty level: " */
        private const string playerNamePrefix = "Player name: ";
        private const string bagPrefix = "Bag content: ";
        private const string playerLocationPrefix = "Player location: ";
        private const string turnPrefix = "Turn: ";
        private const string packPrefix = "Pack locations: ";
        private const string difficultyLevelPrefix = "Difficulty level: ";
        private const string nodeCapacityMultiplierPrefix = "Node capacity multiplier: ";
        private const string numberOfMonstersPrefix = "Number of monsters: ";


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
            this.difficultyLevel = g.difficultyLevel;
            this.nodeCapacityMultiplier = g.nodeCapacityMultiplier;
            this.numberOfMonsters = g.numberOfMonsters;
        }

        public Game GetGame() {
            return g;
        }

        public void ToGame() {
            Game result = new Game(Int32.Parse(GetSingleValueFromFile(difficultyLevelPrefix)),
                Int32.Parse(GetSingleValueFromFile(nodeCapacityMultiplierPrefix)),
                Int32.Parse(GetSingleValueFromFile(numberOfMonstersPrefix)));
            result.player.id = GetSingleValueFromFile(playerNamePrefix);
            result.player.bag = ExtractBag(GetSingleValueFromFile(bagPrefix));

            result.turn = Int32.Parse(GetSingleValueFromFile(turnPrefix));
            result.packs = ExtractPacks(GetSingleValueFromFile(packPrefix));
            
            g = result;
        }

        private List<Pack> ExtractPacks(string v) {
            throw new NotImplementedException();
        }

        private List<Item> ExtractBag(string v) {
            throw new NotImplementedException();
        }

        private string GetSingleValueFromFile(string keyword) {
            int whitespace1 = file.IndexOf(keyword + keyword.Length);
            int whitespace2 = file.IndexOf("!", whitespace1 + 1);
            return file.Substring(whitespace1, whitespace2);
        }


        public override string ToString() {
            return playerNamePrefix + playerName + "!"+ "\n" 
                + bagPrefix + playerItems + "!" + "\n" 
                + playerLocationPrefix + playerLocation + "!" + "\n" 
                + turnPrefix + turn + "!" + "\n" 
                + packPrefix + packLocations + "!" + "\n"
                + difficultyLevelPrefix + difficultyLevel.ToString() + "!" + "\n"
                + nodeCapacityMultiplierPrefix + nodeCapacityMultiplier.ToString() + "!" + "\n"
                + numberOfMonstersPrefix + numberOfMonsters.ToString() + "!" + "\n";
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
