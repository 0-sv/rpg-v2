using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace STVRogue {
    public class Gamestate {
        public string playerName;
        public string playerItems;
        public string playerLocation;
        public string turn;
        public string packLocations;

        public Game g;
        public string file;

        public const string START = "START";
        public const string ItemInBagPrefix = "Number of items: ";
        public const string PackCountPrefix = "Pack count: ";
        public const string playerNamePrefix = "Player name: ";
        public const string bagPrefix = "Bag content: ";
        public const string playerLocationPrefix = "Player location: ";
        public const string turnPrefix = "Turn: ";
        public const string packPrefix = "Packs: ";
        public const string END = "END";

        public Gamestate(Game g, string file) {
            this.file = file;
            this.g.dungeon.nodeList = g.dungeon.nodeList;
            this.g.dungeon.player.id = GetSingleValueFromFile(playerNamePrefix);
            this.g.dungeon.player.bag = ExtractBag();
            this.g.dungeon.turn = Int32.Parse(GetSingleValueFromFile(turnPrefix));
            this.g.dungeon.packs = ExtractPacks();
        }

        public Gamestate(Game g) {
            this.g = g;
            this.playerName = g.dungeon.player.id;
            this.playerItems = BagToString(g.dungeon.player.bag);
            this.playerLocation = NodeToString(g.dungeon.player.location);
            this.turn = g.dungeon.turn.ToString();
            this.packLocations = PacksToString(g.dungeon.packs);
            this.g.dungeon.nodeList = g.dungeon.nodeList;
        }

        public Game GetGame() {
            return this.g;
        }

        public string GetSingleValueFromFile(string keyword) {
            int startIndex = file.IndexOf(keyword) + keyword.Length;
            int endIndex = file.IndexOf(";", startIndex);
            int length = endIndex - startIndex;
            return file.Substring(startIndex, length);
        }

        public int GetVal(string keyword) {
            int result;
            Int32.TryParse(GetSingleValueFromFile(keyword), out result);
            return result;
        }

        public override string ToString() {
            return START + " " + turnPrefix + turn + "; \n"
                + playerNamePrefix + playerName + ";" + "\n"
                + bagPrefix + playerItems + ";" + "\n"
                + playerLocationPrefix + playerLocation + ";" + "\n"
                + packLocations
                + END + "\n";
        }

        public string BagToString(List<Item> items) {
            string result = ItemInBagPrefix + items.Count + ";\n";
            for (int i = 0; i < items.Count; ++i) {
                result += "Crystal " + i.ToString() + ": " + (items[i].isCrystal() ? "yes;" : "no;")
                + ", used: "
                + (items[i].IsUsed() ? "yes;" : "no;")
                + "\n";
            }
            return result;
        }

        public List<Item> ExtractBag() {
            List<Item> result = new List<Item>();

            for (int i = 0; i < GetVal(ItemInBagPrefix); ++i) {
                string crystal = "Crystal " + i.ToString() + ": ";
                if (GetSingleValueFromFile(crystal) == "yes") {
                    Crystal newItem = new Crystal(i.ToString());
                    if (GetSingleValueFromFile(crystal + "yes;, used: ") == "yes") {
                        newItem.used = true;
                    } else {
                        newItem.used = false;
                    }
                    result.Add(newItem);
                } else {
                    HealingPotion newItem = new HealingPotion(i.ToString());
                    if (GetSingleValueFromFile(crystal + "no;, used: ") == "yes") {
                        newItem.used = true;
                    } else {
                        newItem.used = false;
                    }
                    result.Add(newItem);
                }
            }
            return result;
        }

        public string PacksToString(List<Pack> packs) {
            string result = PackCountPrefix + packs.Count.ToString() + ";\n";
            for (int i = 0; i < packs.Count; ++i) {
                result += "Location: " + (packs[i].location.id) + ";\n"
                + "Monsters on this location: "
                + (packs[i].members.Count) + ";\n" + "------------------" + "\n";
                for (int j = 0; j < packs[i].members.Count; ++j) {
                    Monster m = packs[i].members[j];
                    result += "Monster: " + j.ToString()
                        + " with HP: " + m.GetHP().ToString() + ";"
                        + " and attackrating: " + m.AttackRating + ";\n";
                }
                result += "------------------\n";
            }
            return result;
        }

        public List<Pack> ExtractPacks() {
            List<Pack> result = new List<Pack>();

            for (int i = 0; i < GetVal(PackCountPrefix); ++i) {
                Pack newPack = new Pack(i.ToString(), 
                    GetVal("Monsters on this location:"), 
                    g.dungeon.nodeList[GetVal("Location:") - 1], 
                    true, 
                    g.dungeon);
                for (int j = 0; j < GetVal("numOfPacks:"); ++j) {
                    Monster newMonster = new Monster(j.ToString());
                    newMonster.HP = GetVal("Monster: " + j.ToString() + "with HP:");
                    newMonster.AttackRating = GetVal("Monster " + j.ToString() + "with HP:" + newMonster.HP + " and attackrating: ");
                    newPack.AddMonster(newMonster);
                }
            }
            return result;
        }

        public string NodeToString(Node n) {
            return n.id;
        }
    }
}