using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Items
	{
        int hp_before_hp_potion;

        Player p = new Player("1");
        Crystal c = new Crystal("2");
        HealingPotion hp_potion = new HealingPotion("3");
        Item i = new Item("4");

        [Fact]
        public void UseHealOnEmptyBag()
        {
            Assert.Throws<InvalidOperationException>(() => p.Heal());
        }

        [Fact]
        public void UseAccelerateOnEmptyBag()
        {
            Assert.Throws<InvalidOperationException>(() => p.Accelerate());
        }

        [Fact]
        public void IfPlayerUsesHPpotion_AndHPIsBase_ThenHpIsTheSame() {
            hp_before_hp_potion = p.HP;

            p.PickUp(hp_potion);
            p.Heal();

            Assert.Equal(p.HP, hp_before_hp_potion);
        }

        /* if the first test runs, then we can safely use an arbitrary value as HP as parameter */
        [Theory]
        [MemberData(nameof(HPData))]
        public void IfPlayerUsesHPpotion_AndHPIsLessThanBase_HPIsRestored (int value) {
            p.HP = value;

            p.PickUp(hp_potion);
            p.Heal();

            if (value >= 7)
                Assert.Equal(10, p.HP);
            else 
                Assert.NotEqual(10, p.HP);
        }

        [Fact]
        public void IfHealingPotion_IsHealingPotionReturnTrue ()
        {
            Assert.True(hp_potion.IsHealingPotion);
        }

        [Fact]
        public void IfCrystal_IsCrystalReturnTrue()
        {
            Assert.True(c.IsCrystal);
        }

        [Fact]
        public void XTest_Crystal_Usage_Causes_Acceleration()
        {
            p.PickUp(c);
            p.Accelerate();
            Assert.True(p.accelerated);
        }

        [Fact]
        public void IfItem_IsHealingPotionReturnFalse()
        {
            Assert.False(i.IsHealingPotion);
        }
        [Fact]
        public void IfItem_IsCrystal_ReturnFalse()
        {
            Assert.False(i.IsCrystal);
        }

        [Fact]
        public void IfItemIsUsedWithoutPlayerAndItemIsUsed_DoNothing ()
        {
            i.Use(p);
            Assert.Throws<Exception>(() => i.Use(p));
        }

        [Fact]
        public void IfItemIsUsedByPlayer_UsedIsTrue() {
            p.PickUp(hp_potion);
            p.Heal();

            Assert.True(hp_potion.IsUsed());
        }

        [Fact]
        public void IfPlayerUsesCrystal_ThenBridgeDisconnects() {
            Bridge b = new Bridge();
            Node n = new Node();

            b.Connect(n);
            p.Move(b);
            p.PickUp(c);
            p.Accelerate();

            Assert.True(b.neighbors.Any());
        }

        [Fact]
        public void ConstructorItemWorks()
        {
            Item i = new Item();

            p.PickUp(i);

            Assert.True(p.bag.Any());
        }
        public static IEnumerable<object[]> HPData =>
            new List<object[]> { 
                new object[] { 0 },
                new object[] { 1 },
                new object[] { 2 },
                new object[] { 3 },
                new object[] { 4 },
                new object[] { 5 },
                new object[] { 6 },
                new object[] { 7 },
                new object[] { 8 },
                new object[] { 9 },
            };
        }
}
