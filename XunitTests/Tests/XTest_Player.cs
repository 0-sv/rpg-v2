using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
    public class XTest_Player
    {
        Player p = new Player("1");
        Item i = new Item("2");
        HealingPotion hp = new HealingPotion("3");
        Crystal c = new Crystal("4");

        [Fact]
        public void IfPlayerIsCreatedStatsAreOK()
        {
            Assert.Equal(10, p.HP);
            Assert.Equal(5, p.AttackRating);
        }

        [Fact]
        public void IfPlayerPicksUpItem_BagContainsItem()
        {
            p.PickUp(i);
            Assert.Equal(p.bag[0], i);
        }

        [Fact]
        public void XTest_Player_Attack()
        {
            Node node1 = new Node("node1");
            Pack pack1 = new Pack("pack", 2);
            Assert.Equal("pack_0", pack1.members[0].id);
            Assert.Equal("pack_1", pack1.members[1].id);
            pack1.location = node1;
            pack1.members[0].location = node1;
            pack1.members[1].location = node1;
            pack1.members[0].HP = 10;
            pack1.members[1].HP = 7;
            node1.packs.Add(pack1);
            Player player = new Player("player1");
            player.location = node1;
            player.Attack(pack1.members[0]);
            Assert.Equal(pack1.members[0].HP, 10 - player.AttackRating);
            player.accelerated = true;
            player.Attack(pack1.members[1]);
            Assert.True(pack1.members.Count == 1);
            Assert.True(pack1.members[0].id == "pack_1");
            Assert.Equal(pack1.members[0].HP, 7 - player.AttackRating);
        }
    }
}
