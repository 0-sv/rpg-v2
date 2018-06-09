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
		public Game game;
		public bool inCombat = false;
		public bool packChosen = false;
		int packBeingAttacked = 0;
		public Form1()
		{
			InitializeComponent();


			game = new Game(5, 2, 10);
			UpdateGame();
		}

		public void UpdateGame()
		{
			if (game.player.HP == 0)
			{
			}
			else
			{
				if (game.player.location.packs.Any())
				{
					//		inCombat = true;
					CombatUI();
					inCombat = true;
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
				button3.Show();
				button3.Text = "Go to Node " + Int32.Parse(player.location.neighbors[2].id);
			}
			else
			{
				button3.Hide();
			}
			if (player.location.neighbors.Count > 3)
			{
				button4.Show();
				button4.Text = "Go to Node " + Int32.Parse(player.location.neighbors[3].id);
			}
			else
			{
				button4.Hide();
			}
			label5.Text = "Node " + player.location.id;
			label6.Text = game.player.bag.OfType<HealingPotion>().Count().ToString();
			label7.Text = game.player.bag.OfType<Crystal>().Count().ToString();
			button7.Hide();
			button8.Hide();
			button9.Hide();
			button10.Hide();
			button11.Hide();

			//	button1.Hide();

		}

		public void CombatUI()
		{
			if (!packChosen)
			{
				button1.Text = "Attack pack 1";
				button2.Hide();
				if (game.player.location.packs.Count > 1)
				{
					button2.Text = "Attack pack 2";
					button2.Show();
				}
				button3.Hide();
				button4.Hide();
				button5.Hide();
				button6.Hide();
				button7.Hide();
				button8.Hide();
				button9.Hide();
				button10.Hide();
				button11.Hide();

			}
			else
			{
				button2.Show();
				button3.Show();
				button4.Show();
				button5.Show();
				button6.Show();
				button7.Show();
				button1.Text = "Attack monster 1 (HP " + game.player.location.packs[packBeingAttacked].members[0].HP + "/" + game.player.location.packs[packBeingAttacked].members[0].HPbase + ")";
				button2.Text = "Attack monster 2 (HP " + game.player.location.packs[packBeingAttacked].members[1].HP + "/" + game.player.location.packs[packBeingAttacked].members[1].HPbase + ")";

			}


		}

		private void button1_Click(object sender, EventArgs e)
		{

			if (!inCombat)
			{
				int x = Int32.Parse(button1.Text.Split(' ')[3]);
				game.player.location = game.dungeon.nodeList[x];
				UpdateGame();
			}
			else if (!packChosen)
			{

				packBeingAttacked = 0;
				packChosen = true;
				CombatUI();
			}
			else
			{
				game.player.Attack(game.player.location.packs[packBeingAttacked].members[0]);
				UpdateGame();
			}



		}
		private void button2_Click(object sender, EventArgs e)
		{
			if (!inCombat)
			{
				int x = Int32.Parse(button2.Text.Split(' ')[3]);
				game.player.location = game.dungeon.nodeList[x];
				UpdateGame();
			}
			else if (!packChosen)
			{

				packBeingAttacked = 1;
				packChosen = true;
				CombatUI();
			}
			else
			{
				game.player.Attack(game.player.location.packs[packBeingAttacked].members[1]);
				UpdateGame();
			}

		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (!inCombat)
			{
				int x = Int32.Parse(button3.Text.Split(' ')[3]);
				game.player.location = game.dungeon.nodeList[x];
				UpdateGame();
			}
			else if (!packChosen)
			{

				packBeingAttacked = 2;
				packChosen = true;
				CombatUI();
			}
			else
			{
				game.player.Attack(game.player.location.packs[packBeingAttacked].members[2]);
				packChosen = false;
				UpdateGame();
			}
		}
		private void button4_Click(object sender, EventArgs e)
		{
			if (!inCombat)
			{
				int x = Int32.Parse(button4.Text.Split(' ')[3]);
				game.player.location = game.dungeon.nodeList[x];
				UpdateGame();
			}
			else if (!packChosen)
			{

				packBeingAttacked = 3;
				packChosen = true;
				CombatUI();
			}
			else
			{
				game.player.Attack(game.player.location.packs[packBeingAttacked].members[3]);
				UpdateGame();
			}
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (game.player.bag.OfType<HealingPotion>().Any())
			{
				game.player.Heal();
			}
			UpdateGame();
		}
		private void button6_Click(object sender, EventArgs e)
		{
			if (inCombat)
			{
				if (game.player.bag.OfType<Crystal>().Any())
				{
					game.player.Accelerate();
				}
			}
			UpdateGame();
		}
		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void label1_Click_1(object sender, EventArgs e)
		{

		}

		private void button7_Click(object sender, EventArgs e)
		{

		}
	}
}
