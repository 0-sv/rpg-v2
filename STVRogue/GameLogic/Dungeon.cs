using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
	public class Dungeon
	{
		private Predicates p = new Predicates();
		public Node startNode;
		public Node exitNode;
		public List<Node> nodeList;
		public int[] bridges;
		public uint difficultyLevel;
		/* a constant multiplier that determines the maximum number of monster-packs per node: */
		public uint M;
		Random randomnum = new Random();
		/* To create a new dungeon with the specified difficult level and capacity multiplier */
		public Dungeon(uint level, uint nodeCapacityMultiplier)
		{ // call functions to fill the nodeList and connect the nodes
			Logger.log("Creating a dungeon of difficulty level " + level + ", node capacity multiplier " + nodeCapacityMultiplier + ".");
            nodeList = new List<Node>();
			difficultyLevel = level;
			M = nodeCapacityMultiplier;
			nodeList = new List<Node>();
			PopulateNodeList(level);
			startNode = nodeList[0];
			exitNode = nodeList[nodeList.Count - 1];
		}

		private void PopulateNodeList(uint level)
        {
            InitializeBridges(level);
            ConnectNodes(nodeList);
            ConnectBridges(nodeList);
            FinalizeConnectionofNonBridgeNodes(nodeList);
        }

        private void InitializeBridges(uint level)
        {
            bridges = new int[level + 2];
            InitializeNodeList(difficultyLevel, bridges);
            bridges[level + 1] = nodeList.Count() - 1;
        }

        private void FinalizeConnectionofNonBridgeNodes(List<Node> nodeList)
        { // connect nodes on a level randomly
            int amountOfPassedBridges = 0;
            for (int i = 0; i < nodeList.Count - 1; i++)
            {
                if (i == bridges[amountOfPassedBridges + 1])
                {
                    amountOfPassedBridges++;
                }
                for (int j = i + 2; j < bridges[amountOfPassedBridges + 1]; j++)
                { // max 4 neighbors per node
                    if (nodeList[i].neighbors.Count < 4 && nodeList[j].neighbors.Count < 4)
                    {
                        if (randomnum.Next(1, 4) == 1)
                        {
                            nodeList[i].Connect(nodeList[j]);
                        }
                    }
                }
            }
        }

        private void ConnectBridges(List<Node> nodeList)
		{ // for now we have connected all bridges to ensure the dungeon is valid
			for (int i = 0; i < bridges.Length - 1; i++)
            {
                nodeList[bridges[i + 1]].Connect(nodeList[bridges[i]]);
            }
        }

        private static void ConnectNodes(List<Node> nodeList)
        { // connect every node to at least one node to ensure there are no disconnected nodes
            for (int i = 0; i < nodeList.Count - 1; i++)
            {
                nodeList[i].Connect(nodeList[i + 1]);
            }
        }

        private void InitializeNodeList(uint level, int[] bridges)
		{
			Random rnd = new Random();
			int nodesonthislevel;
			int node_id = 0;

			for (int i = 1; i <= level; i++)
			{ // between 2 and 4 nodes on each level excluding the bridges
				nodesonthislevel = rnd.Next(2, 5);
				for (int j = 0; j < nodesonthislevel; j++)
				{
					Node n = new Node(node_id.ToString());
					node_id++;
					nodeList.Add(n);
				}
				// add a bridge after each level
				Bridge b = new Bridge(node_id.ToString());
				nodeList.Add(b);
				bridges[i] = node_id++;
			}

			// a dungeon always ends with nodes
				nodesonthislevel = rnd.Next(3, 5);
				for (int j = 0; j < nodesonthislevel; j++)
				{
					Node n = new Node(node_id++.ToString());
					nodeList.Add(n);
				}
		}

		/* Return a shortest path between node u and node v */
		public List<Node> Shortestpath(Node u, Node v)
		{
			if (!p.isReachable(u, v))
				return new List<Node>() { u };
			return ShortestpathAlgorithm(u, v);
		}

        private static List<Node> ShortestpathAlgorithm(Node u, Node v)
        {
            List<string> closedSet = new List<string>();
            Queue<Node> nextnodes = new Queue<Node>();
            Dictionary<string, Node> shortest = new Dictionary<string, Node>();
            shortest.Add(v.id, v);
            closedSet.Add(v.id);
            foreach (Node n in v.neighbors)
            {
                nextnodes.Enqueue(n);
                closedSet.Add(n.id);
                shortest.Add(n.id, v);
            }
            while (nextnodes.Count > 0)
            {
                Node next = nextnodes.Dequeue();
                if (next.id == u.id)
                {
                    List<Node> shortestpath = new List<Node>();
                    string nextid = u.id;
                    while (true)
                    {
                        if (nextid == v.id)
                            return shortestpath;
                        Node nextnode = shortest[nextid];
                        shortestpath.Add(nextnode);
                        nextid = nextnode.id;
                    }
                }
                foreach (Node n in next.neighbors)
                {
                    if (closedSet.Contains(n.id))
                        continue;
                    if (!shortest.ContainsKey(n.id))
                    {
                        closedSet.Add(n.id);
                        nextnodes.Enqueue(n);
                        shortest.Add(n.id, next);
                    }
                }
            }
            return new List<Node>();
        }

        public uint Level(Node d)
		{
			return p.countNumberOfBridges(startNode, exitNode);
		}
	}

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

		public void Combat(Player player)
        {
            Command combatCommand = new Command(player, Console.ReadKey().Key);
            combatCommand.Execute();
            SelectMonsterAndAttack(player);
            MonsterTurn(player);
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
