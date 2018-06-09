using STVRogue;
using STVRogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
    public class XTest_Command
    {
        Player p = new Player("Test");

        [Fact]
        public void WrongCommandThrowsException()
        {
            Command c = new Command(p, ConsoleKey.B);
            Assert.Throws<NotSupportedException>(() => c.Execute());
        }

        [Fact]
        public void LeftArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.LeftArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void DownArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.DownArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void RightArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.RightArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void UpArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.UpArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void KeyH_Heals()
        {
            Command c = new Command(p, ConsoleKey.H);
            p.SetHP(7);
            p.PickUp(new HealingPotion("Testing Healingpotion"));

            c.Execute();

            Assert.Equal(p.GetHP(), p.HPbase);
        }

        [Fact]
        public void KeyC_Accelerates()
        {
            Command c = new Command(p, ConsoleKey.C);
            p.PickUp(new Crystal("Testing Crystal"));

            c.Execute(); 

            Assert.True(p.accelerated);
        }

        [Fact]
        public void KeyF_Flees()
        {
            Command c = new Command(p, ConsoleKey.F);
            Node firstNode = new Node();
            Node secondNode = new Node();
            firstNode.Connect(secondNode);
            p.location = firstNode;

            c.Execute();
            
            Assert.True(p.location == secondNode);
        }

        [Fact]
        public void KeyA_Attacks()
        {
            Command c = new Command(p, ConsoleKey.A);

            c.Execute();

            Assert.True(p.attacking);
        }
    }
}
