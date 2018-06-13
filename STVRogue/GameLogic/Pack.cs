using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;
using STVRogue.Gamelogic;

namespace STVRogue.GameLogic
{
    public class Pack
    {
        public string id;
        public List<Monster> members = new List<Monster>();
        public int startingHP = 0;
        public Node location;
        public Dungeon dungeon;
		public int zone;
        public bool alerted = false; 

        public Pack(string id, int n, Node loc, bool gameplay, Dungeon thedungeon)
        {
            this.id = id;
            location = loc;
			dungeon = thedungeon;
			if (!gameplay)
			{
				zone = dungeon.CurrentLevel(loc);
				for (int i = 0; i < n; i++)
				{
					Monster m = new Monster("" + id + "_" + i);
					m.location = location;
					members.Add(m);
					startingHP += m.GetHP();
					m.SetPack(this);
				}
			}
        }

		public void AddMonster(Monster monster)
		{
			monster.location = location;
			members.Add(monster);
			startingHP += monster.GetHP();
			monster.SetPack(this);
		}

        public void Attack(Player p)
        {
            foreach (Monster m in members)
            {
                m.Attack(p);
                if (p.HP == 0) 
                    return;
            }
        }

        /* Move the pack to an adjacent node. */
        public void Move(Node u)
        {
            Zone z = new Zone(dungeon, members[0]);
            RNode rnode = new RNode(z);
            RZone rzone = new RZone(z);
            if (!rnode.validMove(this, u) || !rzone.validMove(u))
                return;
            if (!location.neighbors.Contains(u)) 
                throw new ArgumentException();
            int capacity = dungeon.multiplier * (dungeon.CurrentLevel(u) + 1);
            // count monsters already in the node:
            foreach (Pack Q in location.packs) {
                capacity = capacity - Q.members.Count;
            }
            // capacity now expresses how much space the node has left
            if (members.Count > capacity)
            {
                Logger.log("Pack " + id + " is trying to move to a full node " + u.id + ", but this would cause the node to exceed its capacity. Rejected.");
                return;
            }
            foreach (Monster m in members)
                m.location = u;
            location.packs.Remove(this);
            location = u;
            u.packs.Add(this);
            Logger.log("Pack " + id + " moves to an already full node " + u.id + ". Rejected.");
        }

        /* Move the pack one node further along a shortest path to u. */
        public void MoveTowards(Node u)
        {
            List<Node> path = dungeon.Shortestpath(location, u);
            Move(path[0]);
            path.Remove(path[0]);
        }

        public float CalculateFleePossibility()
        {
            int totalCurrentHP = 0, totalBaseHP = 0;
            foreach (Monster m in this.members)
            {
                totalCurrentHP += m.GetHP();
                totalBaseHP += m.HPbase;
            }
            return (1f - ((float)totalCurrentHP / (float)totalBaseHP)) / 2f;
        }
    }
}
