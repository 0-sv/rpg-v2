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
		public int difficultyLevel;
		public int[] amountOfMonsters;
		public int numberOfMonsters;
		public int node;
		public Random rnd = new Random();
		public int multiplier;
		public bool testvar;
		public Player player;
		public List<Pack> packs;
		public List<Item> items;
		public int turn;

		public Dungeon(int difficultyLevel, int multiplier, int numberOfMonsters, bool testvar)
		{
			player = new Player("Bram");
			this.difficultyLevel = difficultyLevel;
			this.multiplier = multiplier;
			this.numberOfMonsters = numberOfMonsters;
			this.testvar = testvar;
			nodeList = new List<Node>();
			PopulateNodeList();
			startNode = nodeList[0];
			exitNode = nodeList[nodeList.Count - 1];
			amountOfMonsters = new int[difficultyLevel + 1];
			player.Move(startNode);
			if (testvar)
			{
				packs = new List<Pack>();
				items = new List<Item>();
			}
			else
			{
				packs = addpacks(numberOfMonsters);			
				int totalMonsterHP = calculateTotalMonsterHP();
				items = additems(totalMonsterHP, bridges[bridges.Length - 1], player.HPbase);
			}
		
			
			turn = 0;
		}

		private void PopulateNodeList()
		{
			if (testvar)
			{
				modifyDungeon();
			}
			else
			{
				InitializeBridges();
				ConnectNodes(nodeList);
				ConnectBridges(nodeList);
				FinalizeConnectionofNonBridgeNodes(nodeList);
			}
		}

		private void InitializeBridges()
		{
			bridges = new int[difficultyLevel + 2];
			InitializeNodeList();
			bridges[difficultyLevel + 1] = nodeList.Count() - 1;
		}

		private void FinalizeConnectionofNonBridgeNodes(List<Node> nodeList)
		{
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
						if (rnd.Next(1, 4) == 1)
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

		public void modifyDungeon()
		{
			int node_id = 0;
			bridges = new int[difficultyLevel + 2];
			for (int i = 1; i <= difficultyLevel; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Node n = new Node(node_id.ToString());
					node_id++;
					nodeList.Add(n);
				}
				Bridge b = new Bridge(node_id.ToString());
				nodeList.Add(b);
				bridges[i] = node_id++;
			}
			for (int j = 0; j < 5; j++)
			{
				Node n = new Node(node_id++.ToString());
				nodeList.Add(n);
			}
			bridges[difficultyLevel + 1] = nodeList.Count() - 1;


			for (int i = 0; i < nodeList.Count - 1; i++)
			{
				nodeList[i].Connect(nodeList[i + 1]);
			}
			for (int i = 0; i < bridges.Length - 1; i++)
			{
				nodeList[bridges[i + 1]].Connect(nodeList[bridges[i]]);
			}
		}

		private void InitializeNodeList()
		{
			int nodesonthislevel;
			int node_id = 0;

			for (int i = 1; i <= difficultyLevel; i++)
			{
				nodesonthislevel = rnd.Next(2, 5);
				for (int j = 0; j < nodesonthislevel; j++)
				{
					Node n = new Node(node_id.ToString());
					node_id++;
					nodeList.Add(n);
				}
				Bridge b = new Bridge(node_id.ToString());
				nodeList.Add(b);
				bridges[i] = node_id++;
			}

			nodesonthislevel = rnd.Next(3, 5);
			for (int j = 0; j < nodesonthislevel; j++)
			{
				Node n = new Node(node_id++.ToString());
				nodeList.Add(n);
			}
		}

		public List<Pack> addpacks(int numberOfMonsters)
		{ // add packs to the dungeon
			int maxMonstersOnThisLevel, monstersOnThisLevel = 0, monstersInDungeon = 0, monstersOnNode = 0;
			int pack_id = 0, count = 0, min, numbers;
			List<int> nodesOnThisLevelInRandomOrder;
			List<Pack> packs = new List<Pack>();
			for (int i = 0; i < bridges.Length - 1; i++)
			{
				min = bridges[i] + 1;
				numbers = bridges[i + 1] - min + 1;
				nodesOnThisLevelInRandomOrder = Enumerable.Range(min, numbers).OrderBy(x => rnd.Next()).ToList();
				if (i < bridges.Length - 2)
				{ // calculate the maximum amount of monsters allowed on this level
					maxMonstersOnThisLevel = (2 * (i + 1) * numberOfMonsters) / ((difficultyLevel + 2) * (difficultyLevel + 1));
				}
				else
				{// add all monsters that are left to the final level

					maxMonstersOnThisLevel = numberOfMonsters - monstersInDungeon;
				}
				count = 0;
				monstersOnThisLevel = 0;
				// add monsters to this level while the limit hasn't been reached yet
				while (monstersOnThisLevel < maxMonstersOnThisLevel)
				{
					if (i == bridges.Length - 2)
					{
						monstersOnNode = Math.Min(maxMonstersOnThisLevel - monstersOnThisLevel, (multiplier * (i + 1)));
					}
					else
					{
						monstersOnNode = Math.Min(maxMonstersOnThisLevel - monstersOnThisLevel, rnd.Next(1, (multiplier * (i + 1))));
					}

					monstersOnThisLevel += monstersOnNode;
					// create a new pack and update its location
					Pack pack = new Pack(pack_id++.ToString(), monstersOnNode, nodeList[nodesOnThisLevelInRandomOrder[count]], false, this);
					nodeList[nodesOnThisLevelInRandomOrder[count++]].packs.Add(pack);
					packs.Add(pack);

				}
				amountOfMonsters[i] = monstersOnThisLevel;

				monstersInDungeon += monstersOnThisLevel;
			}
			if (numberOfMonsters != monstersInDungeon)
			{ // throw exception if amount of monsters and nodeCapacityMultiplier are out of proportion
				throw new GameCreationException("Amount of monsters and nodeCapacityMultiplier are not compatible");
			}
			return packs;
		}

		public int calculateTotalMonsterHP()
		{ // calculate the hp of all monsters, this is later used to determine the amount of HealingPotions in the dungeon.
			int totalHP = 0;
			foreach (Pack p in packs)
			{
				foreach (Monster m in p.members)
				{
					totalHP += m.GetHP();
				}
			}
			return totalHP;
		}

		public List<Item> additems(int totalMonsterHP, int nodeMax, int playerHP)
		{ // add items to the dungeon
			List<Item> items = new List<Item>();
			// calculate the hp limit
			int HPlimit = (int)(totalMonsterHP * 0.8);
			int itemAndPlayerHP = playerHP;
			int item_id = 0;
			int count = 0;
			List<int> allNodesInRandomOrder = Enumerable.Range(1, nodeMax - 1).OrderBy(x => rnd.Next()).ToList();
			while ((itemAndPlayerHP + 11) < HPlimit && count < allNodesInRandomOrder.Count)
			{ // add healingpotions until the limit is reached
				if (rnd.Next(1, 8) == 1)
				{
					HealingPotion item = new HealingPotion(item_id++.ToString());
					item.location = nodeList[allNodesInRandomOrder[count]];
					nodeList[allNodesInRandomOrder[count]].items.Add(item);
					itemAndPlayerHP += item.HPvalue;
					items.Add(item);
				}
				count++;
			}
			count = 0;
			while (count < allNodesInRandomOrder.Count - 1)
			{ // for now we decided every node has a 1 in 20 chance to contain a Crystal
				if (rnd.Next(1, 10) == 1)
				{
					Crystal item = new Crystal(item_id++.ToString());
					item.location = nodeList[allNodesInRandomOrder[count]];
					nodeList[allNodesInRandomOrder[count]].items.Add(item);
					items.Add(item);
				}
				count++;
			}
			return items;
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

		public int Level(Node d)
		{
			return p.countNumberOfBridges(startNode, exitNode);
		}

		public int CurrentLevel(Node d)
		{
			return p.countNumberOfBridges(startNode, d);
		}

        public int countMonsters()
        {
            int count = 0;
            foreach (Pack p in packs)
                foreach (Monster m in p.members)
                    count++;
            return count;
        }
	}
}

