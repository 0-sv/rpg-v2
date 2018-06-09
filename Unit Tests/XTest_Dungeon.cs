using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FsCheck;
using FsCheck.Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Dungeon
	{
        [Property]
			public Property checkIfValidDungeonAutomated(int level, int nodeCapacityMultiplier, int numberOfMonsters)
			{
				if (level > 0 && nodeCapacityMultiplier > 0)
				{
					Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier, numberOfMonsters, new Player("test"));
					Predicates p = new Predicates();
					//	Assert.True(p.isValidDungeon(dungeon.startNode, dungeon.exitNode, dungeon.difficultyLevel));
					return p.isValidDungeon(dungeon.startNode, dungeon.exitNode, dungeon.difficultyLevel).ToProperty();
				}
				else return true.ToProperty();
			}
		[Property]
		public Property checkifLevelFunctionWorks(int level, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			if (level > 0 && nodeCapacityMultiplier > 0)
			{
                Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier, numberOfMonsters, new Player("test"));
                return (dungeon.Level(dungeon.exitNode) == level).ToProperty();
			}
			else return true.ToProperty();
		}

		[Property]
		public Property XTest_shortest_path(int level, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			if (level > 0 && nodeCapacityMultiplier > 0)
			{
                Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier, numberOfMonsters, new Player("test"));

                int i = 0;
                int length = 0;
                bool[] visited = new bool[dungeon.nodeList.Count];
                Queue<Node> nodequeue = new Queue<Node>();
                nodequeue.Enqueue(dungeon.startNode);
                Queue<Node> nextqueue = new Queue<Node>();
                while(nodequeue.Count > 0 || nextqueue.Count > 0)
                {
                    if (nodequeue.Count == 0)
                    {
                        while (nextqueue.Count > 0)
                        {
                            nodequeue.Enqueue(nextqueue.Dequeue());
                        }
                        length++;
                    }
                    Node next = nodequeue.Dequeue();
                    visited[int.Parse(next.id)] = true;
                    if (next.id == dungeon.exitNode.id)
                        return (length == dungeon.Shortestpath(dungeon.startNode, dungeon.exitNode).Count).ToProperty();
                    foreach (Node n in next.neighbors)
                    {
                        if (visited[int.Parse(n.id)])
                            continue;
                        visited[int.Parse(n.id)] = true;
                        if(i < dungeon.bridges.Length && n.id == dungeon.nodeList[dungeon.bridges[i]].id)
                        {
                            i++;
                            length++;
                            while (nodequeue.Count > 0)
                            {
                                Node temp = nodequeue.Dequeue();
                                visited[int.Parse(temp.id)] = true;
                            }
                            nextqueue.Enqueue(n);
                            break;
                        }
                        else
                        {
                            nextqueue.Enqueue(n);
                        }
                    }
                }
			}
			else return true.ToProperty();
            return false.ToProperty();
		}

		[Property]
		public Property checkIfDungeonIsSufficientlyRandom(int level, int nodeCapacityMultiplier, int numberOfMonsters)
		{
			if (level > 0 && nodeCapacityMultiplier > 0)
			{
                Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier, numberOfMonsters, new Player("test"));
                Dungeon dungeon2 = new Dungeon(level, nodeCapacityMultiplier, numberOfMonsters, new Player("test"));
                for (int i = 2;i<dungeon.bridges.Count()-2;i++)
				{
					if(dungeon.bridges[i]-dungeon.bridges[i-1] == dungeon2.bridges[i] - dungeon2.bridges[i - 1])
					{
						for(int j = dungeon.bridges[i - 1]+1;j<= dungeon.bridges[i];j++)
						{
							if(dungeon.nodeList[j].neighbors != dungeon2.nodeList[j].neighbors)
							{
								return true.ToProperty();
							}
						}
						return false.ToProperty();
					}
				}
				return true.ToProperty();
			}
			else return true.ToProperty();
		}

		[Fact]
		public void XTest_shortest_path_unreachable()
		{
			Node node1 = new Node("1");
			Node node2 = new Node("2");
			Node node3 = new Node("3");
			Node node4 = new Node("4");
			node1.Connect(node2);
			node3.Connect(node4);
            Dungeon d = new Dungeon(1, 2, 1, new Player("test"));
			d.nodeList = new List<Node>() { node1, node2, node3, node4 };
			Assert.Equal(d.Shortestpath(node1, node4), new List<Node>() { node1 });
		}
		[Fact]
		public void emptypath()
		{
			Dungeon d = new Dungeon(1, 2, 2, new Player("test"));
			Node node1 = new Node("1");
			d.nodeList = new List<Node>() { node1 };
			List<Node> l = d.Shortestpath(node1, node1);
			Assert.Empty(l);
		}
	}
}
