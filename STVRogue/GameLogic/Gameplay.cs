using STVRogue.GameLogic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace STVRogue
{
	public class Savegame
	{
		private int offset;
		private FileStream fs;
		private Gamestate gs;
		private string path = @"C:/Users/Gebruiker/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game9/game9_turn";

		public Savegame(Gamestate gs)
		{
			this.gs = gs;
			this.offset = 0;
		}

		public void SaveTurn()
		{
			string filename = path + gs.GetGame().dungeon.turn.ToString() + ".txt";
			using (FileStream fs = File.Create(filename))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(gs.ToString());
				fs.Write(info, offset, info.Length);
			}
			offset = filename.Length;
		}

		public string OpenFile(int turn, string filename)
		{
			if (filename == "")
			{
				filename = path + turn.ToString() + ".txt";
			}
			else
			{
				filename = filename + turn.ToString() + ".txt";
			}
			string result = "";
			using (FileStream fs = File.OpenRead(filename))
			{
				byte[] b = new byte[1024];
				UTF8Encoding temp = new UTF8Encoding(true);
				while (fs.Read(b, 0, b.Length) > 0)
				{
					result += temp.GetString(b);
				}
				fs.Close();
			}
			return result;
		}
	}

	public class Gameplay
	{
		public List<Gamestate> states;
		private int ptr;

		public Gameplay(int currentTurn)
		{
			states = new List<Gamestate>();
			this.ptr = currentTurn;
		}

		public void AddGamestate(Gamestate gs)
		{
			int turn = gs.GetGame().dungeon.turn;
			if (turn == 0 || turn == ptr + 1)
			{
				states.Add(gs);
				ptr++;
			}
			else
			{
				Console.WriteLine("The next gamestate should be the next turn.");
			}
		}

		public void Reset()
		{
			ptr = 0;
		}

		public void ReplayTurn()
		{
			ptr++;
		}

		public Gamestate GetState()
		{
			return states[ptr];
		}

		public bool Replay(Specification s)
		{
			for (int i = 0; i < states.Count; ++i)
			{
				// To do: write s.test(G)
				if (!s.test(states[i]))
					return false;
			}
			return true;
		}
	}
}