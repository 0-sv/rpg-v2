using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.GameLogic;
using System.Windows.Forms;

namespace STVRogue {
    class Program {
        static void Main(string[] args) {
            Game game = new Game(5, 2, 10);

            while (game.player.HP != 0) {
                game.Update();
				Application.Run(new Form1());
			}
        }
    }
}
