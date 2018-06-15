using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace STVRogue {
    public class Savegame {
        private int offset; 
        private FileStream fs;
        private Gamestate gs;
        private string path = @"C:\temp\saved_turn ";

        public Savegame(Gamestate gs) {
            this.gs = gs;
            this.offset = 0;
        }

        public void SaveTurn() {
            string filename = path + gs.GetGame().dungeon.turn.ToString() + ".txt";
            using (FileStream fs = File.Create(filename)) {
                byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
                fs.Write(info, offset, info.Length);
            }
            offset = filename.Length;
        }

        public string OpenFile(int turn) {
            string filename = path + turn.ToString() + ".txt";
            string result = "";
            using (FileStream fs = File.OpenRead(path)) {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0) {
                    result += temp.GetString(b);
                }
            }
            return filename;
        }
    }

    public class Gameplay {
        List<Gamestate> states;
        int ptr; 

        public Gameplay (int currentTurn) {
            states = new List<Gamestate>();
            this.ptr = currentTurn;
        }

        public void Reset() {
            ptr = 0;
        }

        public void ReplayTurn() {
            ptr -= 1;
        }

        public Gamestate GetState() {
            return states[ptr];
        }

        /* Usage: first play a whole game, e.g. until the player dies. Then we have a range of turns: 1, 2, 3, 4 .. M to which you can test your specification */
        public bool Replay(Specification s) {
            Reset();
            for (int i = 0; i < states.Count; ++i) {
                bool ok = s.test(states[ptr]);
                if (ok) 
                    ReplayTurn();
                else
                    return false;
            }
            return true;
        }
    }
}
