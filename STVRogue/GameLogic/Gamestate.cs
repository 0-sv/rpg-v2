using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace STVRogue {
    public class Gamestate {
        /* Variables that make up a Game */
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
        private const string ItemInBagPrefix = "Numer of items: ";
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

        public Gamestate(Game g, string file) {
            this.file = file;
            this.g = new Game(Int32.Parse(GetSingleValueFromFile(difficultyLevelPrefix)),
                Int32.Parse(GetSingleValueFromFile(nodeCapacityMultiplierPrefix)),
                Int32.Parse(GetSingleValueFromFile(numberOfMonstersPrefix)));
            this.g.dungeon.nodeList = g.dungeon.nodeList;
            this.g.dungeon.player.id = GetSingleValueFromFile(playerNamePrefix);
            this.g.dungeon.player.bag = ExtractBag(GetSingleValueFromFile(bagPrefix));
            this.g.dungeon.turn = Int32.Parse(GetSingleValueFromFile(turnPrefix));
            this.g.dungeon.packs = ExtractPacks(GetSingleValueFromFile(packPrefix));
        }

        public Gamestate(Game g) {
            this.g = g;
            this.playerName = g.dungeon.player.id;
            this.playerItems = BagToString(g.dungeon.player.bag);
            this.playerLocation = NodeToString(g.dungeon.player.location);
            this.turn = g.dungeon.turn.ToString();
            this.packLocations = PacksToString(g.dungeon.packs);
            this.difficultyLevel = g.dungeon.difficultyLevel;
            this.nodeCapacityMultiplier = g.dungeon.multiplier;
            this.numberOfMonsters = g.dungeon.numberOfMonsters;
            this.nodeList = g.dungeon.nodeList;
        }

        public Game GetGame() {
            return this.g;
        }

        private string GetSingleValueFromFile(string keyword) {
            int whitespace1 = file.IndexOf(keyword + keyword.Length);
            int whitespace2 = file.IndexOf("!", whitespace1 + 1);
            return file.Substring(whitespace1, whitespace2);
        }

        private int GetVal(string keyword) {
            return Int32.Parse(GetSingleValueFromFile(keyword));
        }

        public override string ToString() {
            return START + " " + turnPrefix + turn + "! \n"
                + playerNamePrefix + playerName + "!" + "\n"
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
                result += i.ToString() + "Crystal: " + (items[i].IsCrystal ? "yes" : "no")
                + ", used: "
                + (items[i].IsUsed() ? "yes" : "no")
                + "\n";
            }
            return result;
        }

        private List<Item> ExtractBag(string v) {
            List<Item> result = new List<Item>();

            for (int i = 0; i < GetVal(ItemInBagPrefix); ++i) {
                if (GetSingleValueFromFile(i.ToString() + "Crystal: ") == "yes") {
                    Crystal newItem = new Crystal(i.ToString());
                    if (GetSingleValueFromFile("Crystal: yes, used: ") == "yes") {
                        newItem.used = true;
                    } else {
                        newItem.used = false;
                    }
                    result.Add(newItem);
                } else {
                    HealingPotion newItem = new HealingPotion(i.ToString());
                    result.Add(newItem);
                    if (GetSingleValueFromFile("Crystal: no, used: ") == "yes") {
                        newItem.used = true;
                    } else {
                        newItem.used = false;
                    }
                    result.Add(newItem);
                }
            }
            return result;
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
                Pack newPack = new Pack(i.ToString(), GetVal("numOfPacks:"), new Node(GetVal("Location" + i.ToString()).ToString()), true, g.dungeon);
                for (int j = 0; j < GetVal("numOfPacks:"); ++j) {
                    Monster newMonster = new Monster(j.ToString());
                    newMonster.HP = GetVal("Monster: " + j.ToString() + "with HP:");
                    newMonster.AttackRating = GetVal("Monster " + j.ToString() + "with HP:" + newMonster.HP + " and attackrating: ");
                    newPack.AddMonster(newMonster);
                }
            }
            return result;
        }

        private string NodeToString(Node n) {
            return n.id;
        }
    }
}