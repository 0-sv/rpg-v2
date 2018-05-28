using STVRogue.GameLogic;
using System;

namespace STVRogue
{
    public class Command
    {
        private Player player;
        private ConsoleKey key;

        public Command (Player player, ConsoleKey key) {
            this.key = key;
            this.player = player;
        }

        public void Execute() {
            switch (key) {
                case ConsoleKey.LeftArrow: 
                    throw new NotImplementedException();
                case ConsoleKey.DownArrow:
                    throw new NotImplementedException();
                case ConsoleKey.RightArrow:
                    throw new NotImplementedException();
                case ConsoleKey.UpArrow:
                    throw new NotImplementedException();
                case ConsoleKey.H:
                    player.Heal();
                    break;
                case ConsoleKey.C:
                    player.Accelerate();
                    break;
                case ConsoleKey.F:
                    player.Flee();
                    break;
                case ConsoleKey.A:
                    player.attacking = true;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
