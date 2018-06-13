using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace STVRogue {
    public class GamePlay {
        private int offset; 
        private FileStream fs;
        private GameState gs;
        private string path = @"C:\Users\win7\Documents\GitHub\Software-Testing-Assignment-2\STVRogue\saved_game.txt";

        public GamePlay(string file) {
            this.gs = new GameState(file);
            this.offset = 0; 
        }

        public GamePlay(GameState gs) {
            this.gs = gs;
        }

        /* Usage: use this method only once, else the file gets overwritten */
        public void CreateSaveGameFile() {
            using (FileStream fs = File.Create(path)) {
                byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
                fs.Write(info, offset, info.Length);
            }
            offset = gs.ToString().Length;
        }

        /* Usage: use this method every turn so we have turns 1, 2, 3, 4 .. N */
        public void SaveTurnToSaveGameFile() {
            using (FileStream fs = File.OpenWrite(path)) {
                byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
                fs.Write(info, offset, info.Length);
            }
            offset += gs.ToString().Length;
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

        public Game Reset () {
            string file = OpenFile();
            file = GetTurnFromFile(file, 0);
            GameState openGS = new GameState(file);
            openGS.ToGame();
            return openGS.GetGame();
        }

        public Game ReplayTurn(int currentTurn) {
            string file = OpenFile();
            file = GetTurnFromFile(file, currentTurn - 1);
            GameState openGS = new GameState(file);
            openGS.ToGame();
            return openGS.GetGame();
        }
       
        private string GetTurnFromFile(string file, int turn) {
            string keyword = "START Turn: " + turn.ToString();
            int begin = file.IndexOf(keyword + keyword.Length);
            int end = file.IndexOf("END", begin + 1);
            return file.Substring(begin, end);
        }

        public GameState GetState() {
            return gs; 
        }

        /* Usage: first play a whole game, e.g. until the player dies. Then we have a range of turns: 1, 2, 3, 4 .. M to which you can test your specification */
        public bool Replay(Specification s) {
            Reset();
            for (int i = 0; i < GetState().GetGame().turn; ++i) {
                bool ok = s.test(GetState());
                if (ok) {
                    ReplayTurn(GetState().GetGame().turn);
                }
                else
                    return false;
            }
            return true;
        }
    }

    public class GameState {
        /* Variables that make up a GameState */
        private string playerName;
        private string playerItems;
        private string playerLocation; 
        private string turn;
        private string packLocations;
        private int difficultyLevel;
        private int nodeCapacityMultiplier;
        private int numberOfMonsters;
        
        /* These are used to go from save game to game, and vice versa */
        private Game g;
        private string file;

        /* It's impossible to reconstruct the same exact dungeon because of randomness, so use the previous structure. */
        private List<Node> nodeList;

        /* Variables that make up the prefix, e.g.: "Difficulty level: " */
        private const string START = "START";
        private const string PackCountPrefix = "Pack count: ";
        private const string playerNamePrefix = "Player name: ";
        private const string bagPrefix = "Bag content: ";
        private const string playerLocationPrefix = "Player location: ";
        private const string turnPrefix = "Turn: ";
        private const string packPrefix = "Pack locations: ";
        private const string difficultyLevelPrefix = "Difficulty level: ";
        private const string nodeCapacityMultiplierPrefix = "Node capacity multiplier: ";
        private const string numberOfMonstersPrefix = "Number of monsters: ";
        private const string END = "END";

        /* Assumption: you already have a game, so you can overwrite it with the details from the saved game, ie. all the variables */
        public GameState(List<Node> currentDungeonStructure, string file) {
            this.nodeList = currentDungeonStructure;
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
            result.dungeon.nodeList = nodeList; 
            result.player.id = GetSingleValueFromFile(playerNamePrefix);
            result.player.bag = ExtractBag(GetSingleValueFromFile(bagPrefix));

            result.turn = Int32.Parse(GetSingleValueFromFile(turnPrefix));
            result.packs = ExtractPacks(GetSingleValueFromFile(packPrefix));
            
            g = result;
        }

        private string GetSingleValueFromFile(string keyword) {
            int whitespace1 = file.IndexOf(keyword + keyword.Length);
            int whitespace2 = file.IndexOf("!", whitespace1 + 1);
            return file.Substring(whitespace1, whitespace2);
        }

        private int GetVal (string keyword) {
            return Int32.Parse(GetSingleValueFromFile(keyword));
        }

        public override string ToString() {
            return START + " " + turnPrefix + turn + "! \n"
                + playerNamePrefix + playerName + "!"+ "\n" 
                + bagPrefix + playerItems + "!" + "\n" 
                + playerLocationPrefix + playerLocation + "!" + "\n" 
                + packPrefix + packLocations + "!" + "\n"
                + difficultyLevelPrefix + difficultyLevel.ToString() + "!" + "\n"
                + nodeCapacityMultiplierPrefix + nodeCapacityMultiplier.ToString() + "!" + "\n"
                + numberOfMonstersPrefix + numberOfMonsters.ToString() + "!" + "\n"
                + END + "\n";
        }

        private string BagToString(List<Item> items) {
            string result = ItemInBagPrefix + items.Count + "!\n";
            for (int i = 0; i < items.Count; ++i) {
                result += i.ToString() + ": " + (items[i].IsCrystal ? "crystal" : "hp_potion")
                + " , used: " 
                + (items[i].IsUsed() ? "yes" : "no")
                + "\n";
            }
            return result;
        }

        private List<Item> ExtractBag(string v) {
            List<Item> result = new List<Item>();

            for (int i = 0; i < GetVal(ItemInBagPrefix); ++i) {
                Item newItem; 
                GetSingleValueFromFile(i.ToString() + ": ") == "crystal" ? newItem = new Crystal(i.ToString()) : newItem = new Healingpotion(i.ToString);
                GetSingleValueFromFile(i.ToString() + ": ")
            }
        }

        private string PacksToString(List<Pack> packs) {
            string result = PackCountPrefix + packs.Count.ToString() + "!\n";
            for (int i = 0; i < packs.Count; ++i) {
                result += "Location " + (packs[i].location.id) + ": " + "!\n"
                + " numOfPacks: "
                + (packs[i].members.Count) + "!\n" + "------------------" + "\n";
                for (int j = 0; j < packs[i].members.Count; ++j) {
                    Monster m = packs[i].members[j];
                    result += "Monster: " + j.ToString()
                        + " with HP: " + m.GetHP().ToString() + "!"
                        + " and attackrating: " + m.AttackRating + "!\n";
                }
                result += "\n";
            }
            return result;
        }

        private List<Pack> ExtractPacks(string s) {
            List<Pack> result = new List<Pack>();

            for (int i = 0; i < GetVal(PackCountPrefix); ++i) {
                Pack newPack = new Pack(i.ToString(), GetVal("numOfPacks:"), new Node(GetVal("Location" + i.ToString())));
                for (int j = 0; j < GetVal("numOfPacks:"); ++j) {
                    Monster newMonster = new Monster(j.ToString());
                    newMonster.HP = GetVal("Monster: " + j.ToString() + "with HP:");
                    newMonster.AttackRating = GetVal("Monster " + j.ToString() + "with HP:" + newMonster.HP + " and attackrating: ");
                    newPack.Add(newMonster);
                } 
            }
            return result;
        }

        private string NodeToString(Node n) {
            return n.id;
        }
    }
}
