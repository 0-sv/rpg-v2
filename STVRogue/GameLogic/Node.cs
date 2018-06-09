using System;
using System.Collections.Generic;
using System.Linq;
using STVRogue.GameLogic;
using STVRogue.Utils;

namespace STVRogue.GameLogic {
    public class Node
	{
		public String id;
		public List<Node> neighbors = new List<Node>();
		public List<Pack> packs = new List<Pack>();
		public List<Item> items = new List<Item>();
        private bool IsNotPossibleToFlee = false;

        public Node() { }
		public Node(String id) { this.id = id; }

		public void Connect(Node nd)
		{
			neighbors.Add(nd); 
            nd.neighbors.Add(this);
		}

		public void Disconnect(Node nd)
		{
			neighbors.Remove(nd); 
            nd.neighbors.Remove(this);
		}

		public void Combat(Player player, int pack, int monster)
        {
      //      Command combatCommand = new Command(player, Console.ReadKey().Key);
       //     combatCommand.Execute();
			player.Attack(packs[pack].members[monster]);
			if (!packs[pack].members.Any())
			{
				packs.Remove(packs[pack]);
			}
			else
			{
				MonsterTurn(player);
			}
        }
        private void SelectMonsterAndAttack(Player player)
        {
            if (player.attacking)
            {
                ListPossiblePacks();
                int pack = ReadKey();
                ListPossibleMonsters(pack);
                int monster = ReadKey();

                try
                {
                    player.Attack(packs[pack].members[monster]);
                    player.attacking = false;
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                    SelectMonsterAndAttack(player);
                }
            }
        }

        private void ListPossiblePacks()
        {
            Console.WriteLine("Choose which pack to attack: ");
            int index = 1;
            foreach (Pack p in packs)
                Console.WriteLine(index.ToString() + ": " + p.id + " press key " + index++.ToString());
        }

        private void ListPossibleMonsters(int pack)
        {
            Console.WriteLine("Choose which pack to attack: ");
            int index = 1;
            foreach (Monster m in packs[pack].members)
                Console.WriteLine(index.ToString() + ": " + m.id + " press key " + index++.ToString());
        }


        private void MonsterTurn(Player player)
        {
            if (this.packs.Any() && IsNotPossibleToFlee)
            {
                ForcedMonsterTurn(player, packs[0]);
                return;
            }
                
            foreach (Pack p in packs)
            {
                float fleePossibility = p.CalculateFleePossibility();
                
                if (fleePossibility <= 0.5)
                {
                    p.members[RandomGenerator.rnd.Next(p.members.Count())].Attack(player);
                    IsNotPossibleToFlee = true;
                    return;
                }
                else
                {
                    p.Move(this.neighbors[0]);
                    ForcedMonsterTurn(player, p);
                }
            }
        }

        private void ForcedMonsterTurn(Player player, Pack p)
        {
            p.members[RandomGenerator.rnd.Next(p.members.Count())].Attack(player);
            IsNotPossibleToFlee = false;
        }

        private int ReadKey()
        {
            ConsoleKeyInfo cfi = Console.ReadKey();
            return Int32.Parse(cfi.Key.ToString());
        }
    }

	public class Bridge : Node
	{
		public Bridge(String id) : base(id) { }
        public Bridge () { }
	}
}


