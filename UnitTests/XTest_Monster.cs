using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using FsCheck;
using FsCheck.Xunit;

namespace STVRogue.GameLogic
{
   public class XTest_Monster
    {
		[Fact]
		public void XTest_pack_move()
		{
			Dungeon dungeon = new Dungeon(5, 2, 5, new Player("test"));
			Node node = new Node("testnode");
			node.Connect(dungeon.nodeList[1]);
			dungeon.nodeList.Add(node);
			Pack pack = new Pack("testpack", 2);
			pack.location = dungeon.nodeList[1];
			pack.dungeon = dungeon;
			dungeon.nodeList[1].packs = new List<Pack>() { pack };
			pack.Move(node);
			Assert.Contains(pack, node.packs);
			Assert.DoesNotContain(pack, dungeon.nodeList[1].packs);
		}

        [Fact]
        public void XTest_pack_attack()
        {
            Player player = new Player("player");
            Pack pack = new Pack("pack", 6);
            pack.Attack(player);
            Assert.True(player.HP == 4);
            pack.Attack(player);
            Assert.True(player.HP == 0);
        }

        [Fact]
        public void XTest_pack_move_not_neighbor()
        {
            Node node1 = new Node("1");
            Node node2 = new Node("2");
            Pack pack = new Pack("pack", 5);
            pack.location = node1;
            node1.packs.Add(pack);
            Assert.Throws<ArgumentException>(() => pack.Move(node2));
        }
		
        [Fact]
        public void Xtest_pack_move_over_capacity()
        {
            using (StringWriter sw = new StringWriter())
            {
                TextWriter temp = Console.Out;
                Dungeon dungeon = new Dungeon(5, 0, 5, new Player("1"));
                Pack pack = new Pack("pack", 50);
                pack.dungeon = dungeon;
                dungeon.nodeList[1].packs.Add(pack);
                pack.location = dungeon.nodeList[1];
                Console.SetOut(sw);
                pack.Move(dungeon.nodeList[1].neighbors[0]);
                string expected = "** Pack " + pack.id + " is trying to move to a full node " + dungeon.nodeList[1].neighbors[0].id + ", but this would cause the node to exceed its capacity. Rejected.\r\n";
                Assert.Equal(expected, sw.ToString());
                Console.SetOut(temp);
                sw.Dispose();
            }
        }
		

        [Property]
        public void XTest_pack_flee_chance(int seed)
        {
            RandomGenerator.initializeWithSeed(seed);
            Pack pack = new Pack("pack", RandomGenerator.rnd.Next(1, 50));
            int totalBase = 0;
            int totalHP = 0;
            foreach (Monster m in pack.members)
            {
                m.HPbase = RandomGenerator.rnd.Next(1, 100);
                m.HP = RandomGenerator.rnd.Next(1, m.HPbase);
                totalBase += m.HPbase;
                totalHP += m.HP;
            }

            float expected = (1f - ((float)totalHP / (float)totalBase)) / 2f;
            float actual = pack.CalculateFleePossibility();
            Assert.Equal(expected, actual, 4);
        }
	}
}
