using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STVRogue.Gamelogic;
using STVRogue.GameLogic;
namespace STVRogue
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();


			Game game = new Game(5, 2, 10);
			UpdateGame(game);




			//		for (int i = 0; i < game.player.location.neighbors.Count; i++)
			//		{
			//		button1.Text = "Go to Node " + game.dungeon.nodeList[i].id;
			//	}
			//		while (game.player.HP != 0)
			//	{
			//		game.Update();

			//	}
		}

		public void UpdateGame(Game game)
		{
			if (game.player.HP == 0)
			{
			}
			else
			{
				if (game.player.location.packs.Any())
				{
					Zone z = new Zone(game.dungeon);
					RAlert alert = new RAlert(z);
					alert.AlertMonsters();
					game.player.location.Combat(game.player);
					alert.DeAlertMonsters();
				}
				else
				{
					UpdateUI(game.player);
				}
			}
		}
		public void UpdateUI(Player player)
		{
			button1.Text = "Go to Node " + Int32.Parse(player.location.neighbors[0].id);
			button2.Text = "Go to Node " + Int32.Parse(player.location.neighbors[1].id);
			if (player.location.neighbors.Count > 2)
			{
				button3.Text = "Go to Node " + Int32.Parse(player.location.neighbors[2].id);
			}
			else
			{
				button3.Text = "";
			}
			if (player.location.neighbors.Count > 3)
			{
				button4.Text = "Go to Node " + Int32.Parse(player.location.neighbors[3].id);
			}
			else
			{
				button4.Text = "";
			}
			label5.Text = "Node " + player.location.id;
		//	label3.Text = ;
			for(int i = 0; i<player.bag.Count;i++)
			{
			//	if(i is HealingPotion)
			}

		}

		private void button1_Click(object sender, EventArgs e)
		{
			string x = button1.Text.Split(' ')[3];



		}
		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{

		}
		private void button4_Click(object sender, EventArgs e)
		{

		}

		private void button5_Click(object sender, EventArgs e)
		{
			// player.Heal();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

	}
}
