using STVRogue;
using STVRogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class UnlessTests
    {
        Game g;
        Gamestate gs;
        Savegame savegame;
        string data;
        string path;
        int[] gameturns;

        public UnlessTests()
        {
            gameturns = new int[8];
            gameturns[0] = 23;
            gameturns[1] = 32;
            gameturns[2] = 34;
            gameturns[3] = 67;
            gameturns[4] = 18;
            gameturns[5] = 17;
            gameturns[6] = 40;
            gameturns[7] = 76;
        }

        [Fact]
        public void test_monsters_never_increase()
        {
            int l, c, m;
            for (int k = 1; k < 9; k++)
            {
                if (k < 5)
                {
                    l = 5;
                }
                else
                {
                    l = 10;
                }
                if (k % 2 == 0)
                {
                    c = 3;
                }
                else
                {
                    c = 2;
                }
                if (k == 3 || k == 4 || k == 7 || k == 8)
                {
                    m = 50;
                }
                else
                {
                    m = 30;
                }
                g = new Game(l, c, m, true);
                gs = new Gamestate(g);
                savegame = new Savegame(gs);
                for (int j = 1; j < 6; j++)
                {
                    path = @"C:/Users/Gebruiker/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game" + k + "/game" + k + "_turn";
                    Unless unless = new Unless(G => G.dungeon.numberOfMonsters > G.dungeon.countMonsters(), G => G.dungeon.numberOfMonsters == G.dungeon.countMonsters());
                    for (int i = 0; i < gameturns[k - 1]; i++)
                    {
                        data = savegame.OpenFile(i, path);
                        Gamestate gamestate = new Gamestate(g, data);
                        Assert.True(unless.test(g));
                    }
                }
            }
        }

        [Fact]
        public void test_player_heal()
        {
            int l, c, m;
            for (int k = 1; k < 9; k++)
            {
                if (k < 5)
                {
                    l = 5;
                }
                else
                {
                    l = 10;
                }
                if (k % 2 == 0)
                {
                    c = 3;
                }
                else
                {
                    c = 2;
                }
                if (k == 3 || k == 4 || k == 7 || k == 8)
                {
                    m = 50;
                }
                else
                {
                    m = 30;
                }
                g = new Game(l, c, m, true);
                gs = new Gamestate(g);
                savegame = new Savegame(gs);
                for (int j = 1; j < 6; j++)
                {
                    int previousHP = g.dungeon.player.HPbase;
                    int previousPotions = 0;
                    path = @"C:/Users/Gebruiker/Documents/GitHub/Software-Testing-Assignment-2/STVRogue/Gameplays/game" + k + "/game" + k + "_turn";
                    Unless unless = new Unless(G => G.dungeon.player.HP <= previousHP, G => G.dungeon.player.HP > previousHP && G.dungeon.player.bag.OfType<HealingPotion>().Count() == previousPotions - 1);
                    for (int i = 0; i < gameturns[k - 1]; i++)
                    {
                        data = savegame.OpenFile(i, path);
                        Gamestate gamestate = new Gamestate(g, data);
                        Assert.True(unless.test(g));
                        previousHP = gamestate.g.dungeon.player.HP;
                        previousPotions = gamestate.g.dungeon.player.bag.OfType<HealingPotion>().Count();
                    }
                }
            }
        }
    }
}
