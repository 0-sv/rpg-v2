using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace STVRogue
{
	class Program
	{
		
		[STAThread]
		static void Main(string[] args) {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new RogueUI());
		//	Game game = new Game(5, 2, 10);
			
		//	while (game.player.HP != 0) {
     //           game.Update();
				
			
        }
    }
}