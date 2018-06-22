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
namespace STVRogue {
    public partial class RogueUI : Form {
        public Game game;

        public bool inCombat = false;
        public bool packChosen = false;
        int packBeingAttacked = 0;
        public RogueUI() {
            InitializeComponent();
            game = new Game(5, 2, 30, false);
            UpdateGame();
        }

        public void UpdateGame() {
            if (game.dungeon.player.HP == 0) {
                button1.Hide();
                button2.Hide();
                button5.Hide();
                button6.Hide();
                button7.Hide();
                HideButtons();
                label8.Text = "Player HP: " + game.dungeon.player.HP + "/" + game.dungeon.player.HPbase;
                label10.Text = "You died";

            } else if (game.dungeon.player.location == game.dungeon.exitNode && !game.dungeon.player.location.packs.Any()) {
                button1.Hide();
                button2.Hide();
                button5.Hide();
                button6.Hide();
                button7.Hide();
                HideButtons();
                label8.Text = "Player HP: " + game.dungeon.player.HP + "/" + game.dungeon.player.HPbase;
                label10.Text = "You win";
            } else {
                PackMove();
                if (game.dungeon.player.location.packs.Any()) {
                    CombatUI();
                    inCombat = true;
                    Zone z = new Zone(game.dungeon, game.dungeon.player);
                    RAlert alert = new RAlert(z);
                    alert.AlertMonsters();
                    alert.DeAlertMonsters();
                } else {
                    UpdateUI(game.dungeon.player);
                }
                game.dungeon.turn++;

            }
        }
        public void UpdateUI(Player player) {
            inCombat = false;
            button1.Text = "Go to Node " + Int32.Parse(player.location.neighbors[0].id);
            button2.Show();
            button2.Text = "Go to Node " + Int32.Parse(player.location.neighbors[1].id);
            if (player.location.neighbors.Count > 2) {
                button3.Show();
                button3.Text = "Go to Node " + Int32.Parse(player.location.neighbors[2].id);
            } else {
                button3.Hide();
            }
            if (player.location.neighbors.Count > 3) {
                button4.Show();
                button4.Text = "Go to Node " + Int32.Parse(player.location.neighbors[3].id);
            } else {
                button4.Hide();
            }
            label4.Text = "Current location: Node " + player.location.id;
            label6.Text = game.dungeon.player.bag.OfType<HealingPotion>().Count().ToString();
            label7.Text = game.dungeon.player.bag.OfType<Crystal>().Count().ToString();
            label8.Text = "Player HP: " + game.dungeon.player.HP + "/" + game.dungeon.player.HPbase;
            button6.Text = "Do nothing";
            button7.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            button11.Hide();

        }

        public void CombatUI() {
            label4.Text = "Current location: Node " + game.dungeon.player.location.id;
            label6.Text = game.dungeon.player.bag.OfType<HealingPotion>().Count().ToString();
            label7.Text = game.dungeon.player.bag.OfType<Crystal>().Count().ToString();
            label8.Text = "Player HP: " + game.dungeon.player.HP + "/" + game.dungeon.player.HPbase;

            if (!packChosen) {
                button1.Text = "Attack pack 1";
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button8.Hide();
                button9.Hide();
                button10.Hide();
                button11.Hide();
                if (game.dungeon.player.location.packs.Count > 1) {
                    button2.Text = "Attack pack 2";
                    button2.Show();
                }
                if (game.dungeon.player.location.packs.Count > 2) {
                    button3.Text = "Attack pack 3";
                    button3.Show();
                }
                if (game.dungeon.player.location.packs.Count > 3) {
                    button4.Text = "Attack pack 4";
                    button4.Show();
                }
                if (game.dungeon.player.location.packs.Count > 4) {
                    button8.Text = "Attack pack 5";
                    button8.Show();
                }
                if (game.dungeon.player.location.packs.Count > 5) {
                    button9.Text = "Attack pack 6";
                    button9.Show();
                }
                if (game.dungeon.player.location.packs.Count > 6) {
                    button10.Text = "Attack pack 7";
                    button10.Show();
                }
                if (game.dungeon.player.location.packs.Count > 7) {
                    button11.Text = "Attack pack 8";
                    button11.Show();
                }

                button5.Show();
                button6.Show();
                button7.Show();
                button6.Text = "Use Crystal";



            } else {
                HideButtons();
                button1.Text = "Attack monster 1 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[0].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[0].HPbase + ")";
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 1) {
                    button2.Show();
                    button2.Text = "Attack monster 2 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[1].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[1].HPbase + ")";
                }
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 2) {
                    button3.Show();
                    button3.Text = "Attack monster 3 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[2].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[2].HPbase + ")";
                }
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 3) {
                    button4.Show();
                    button4.Text = "Attack monster 4 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[3].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[3].HPbase + ")";
                }
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 4) {
                    button8.Show();
                    button8.Text = "Attack monster 5 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[4].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[4].HPbase + ")";
                }
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 5) {
                    button9.Show();
                    button9.Text = "Attack monster 6 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[5].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[5].HPbase + ")";
                }
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 6) {
                    button10.Show();
                    button10.Text = "Attack monster 7 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[6].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[6].HPbase + ")";
                }
                if (game.dungeon.player.location.packs[packBeingAttacked].members.Count > 7) {
                    button10.Show();
                    button10.Text = "Attack monster 8 (HP " + game.dungeon.player.location.packs[packBeingAttacked].members[7].HP + "/" + game.dungeon.player.location.packs[packBeingAttacked].members[7].HPbase + ")";
                }

            }


        }

        public void HideButtons() {
            button2.Hide();
            button3.Hide();
            button4.Hide();
            button8.Hide();
            button9.Hide();
            button10.Hide();
            button11.Hide();
        }

        public void PlayerMove(Button button) {
            int x = Int32.Parse(button.Text.Split(' ')[3]);
            game.dungeon.player.Move(game.dungeon.nodeList[x]);
            label10.Text = "Player moved to Node " + x;
            UpdateGame();
        }

        private void PackMove() {
            foreach (Pack p in game.dungeon.packs) {
                if (p.members.Count() > 0) {
                    if (p.location == game.dungeon.player.location)
                        continue;
                    if (p.alerted) {
                        if (game.dungeon.rnd.Next(2) == 0)
                            p.MoveTowards(game.dungeon.player.location);
                    } else {
                        int nbs = p.location.neighbors.Count;
                        int i = game.dungeon.rnd.Next(nbs + 1);
                        if (i == nbs)
                            continue;
                        else
                            p.Move(p.location.neighbors[i]);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {

            if (!inCombat) {
                PlayerMove(button1);
            } else if (!packChosen) {

                packBeingAttacked = 0;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 1 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 0);

                packChosen = false;
                UpdateGame();
            }



        }
        private void button2_Click(object sender, EventArgs e) {
            if (!inCombat) {
                PlayerMove(button2);
            } else if (!packChosen) {

                packBeingAttacked = 1;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 2 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 1);
                packChosen = false;
                UpdateGame();
            }

        }

        private void button3_Click(object sender, EventArgs e) {
            if (!inCombat) {
                PlayerMove(button3);
            } else if (!packChosen) {

                packBeingAttacked = 2;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 1 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 2);
                packChosen = false;
                UpdateGame();
            }
        }
        private void button4_Click(object sender, EventArgs e) {
            if (!inCombat) {
                PlayerMove(button4);
            } else if (!packChosen) {

                packBeingAttacked = 3;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 1 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 3);
                packChosen = false;
                UpdateGame();
            }
        }

        private void button5_Click(object sender, EventArgs e) {

            if (game.dungeon.player.bag.OfType<HealingPotion>().Any()) {
                game.dungeon.player.Heal();
                label10.Text = "Player used a healing potion";
                if (inCombat) {
                    game.dungeon.player.location.Combat(game.dungeon.player, 0, 0);
                }
                packChosen = false;
                UpdateGame();
            } else {
                label10.Text = "No healing potions available!";
            }
        }
        private void button6_Click(object sender, EventArgs e) {
            if (inCombat) {
                if (game.dungeon.player.bag.OfType<Crystal>().Any()) {
                    if (game.dungeon.player.accelerated) {
                        label10.Text = "Player is already accelerated";
                    } else {
                        label10.Text = "Player is now accelerated";
                        game.dungeon.player.Accelerate();
                        game.dungeon.player.location.Combat(game.dungeon.player, 0, 0);
                    }
                } else {
                    label10.Text = "No crystals available!";
                }
            }

            packChosen = false;
            UpdateGame();
        }
        private void button7_Click(object sender, EventArgs e) {
            game.dungeon.player.Flee();
            label10.Text = "Player fled the battle";
            packChosen = false;
            UpdateGame();

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label1_Click_1(object sender, EventArgs e) {

        }

        private void button8_Click(object sender, EventArgs e) {
            if (!packChosen) {
                packBeingAttacked = 4;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 5 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 4);
                packChosen = false;
                UpdateGame();
            }
        }

        private void button9_Click(object sender, EventArgs e) {
            if (!packChosen) {

                packBeingAttacked = 5;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 6 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 5);
                packChosen = false;
                UpdateGame();
            }
        }

        private void button10_Click(object sender, EventArgs e) {
            if (!packChosen) {

                packBeingAttacked = 6;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 7 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 6);
                packChosen = false;
                UpdateGame();
            }
        }

        private void button11_Click(object sender, EventArgs e) {
            if (!packChosen) {

                packBeingAttacked = 7;
                packChosen = true;
                CombatUI();
            } else {
                game.dungeon.player.attacking = true;
                if (!game.dungeon.player.accelerated) {
                    label10.Text = "Player attacked monster 8 of pack " + (packBeingAttacked + 1);
                } else {
                    label10.Text = "Player attacked all monsters of pack " + (packBeingAttacked + 1);
                }
                game.dungeon.player.location.Combat(game.dungeon.player, packBeingAttacked, 7);
                packChosen = false;
                UpdateGame();
            }
        }
    }
}