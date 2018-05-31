using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
    public class Creature
    {
        public String id;
        public String name;
        public int HP;
        public int HPbase;
        public int AttackRating = 1;
        public Node location;
        protected Creature() { }
        virtual public void Attack(Creature foe)
        {
            foe.HP = (int)Math.Max(0, foe.HP - AttackRating);
            String killMsg = foe.HP == 0 ? ", KILLING it" : "";
            Logger.log("Creature " + id + " attacks " + foe.id + killMsg + ".");
        }
        public void SetHP(int HP) => this.HP = HP;
        public int GetHP() => HP;

        public Creature getCreature(string id) => return this;
    }

    public class Monster : Creature
    {
        private Pack pack;

        /* Create a monster with a random HP */
        public Monster(String id)
        {
            this.id = id; name = "Orc";
            this.HPbase = 1 + RandomGenerator.rnd.Next(6);
            this.HP = HPbase;
        }

        public Pack GetPack() => pack;
        public void SetPack(Pack p) => pack = p;
    }

    public class Player : Creature
    {
        public Boolean accelerated;
        public uint KillPoint;
        public List<Item> bag;
        public bool attacking;

        public Player(string id)
        {
            this.id = id;
            this.AttackRating = 5;
            this.HPbase = 10;
            this.HP = HPbase;
            this.KillPoint = 0;
            this.accelerated = false;
            this.bag = new List<Item>();
            this.attacking = false;
        }

        public void Move(Node n)
        {
            if (n.items.Any())
                foreach (Item i in n.items)
                    this.PickUp(i);
            this.location = n;
        }

        public void PickUp(Item item) => bag.Add(item);

        public void Heal()
        {
            HealingPotion p = bag.OfType<HealingPotion>().First();
            p.Use(this);
            bag.Remove(p);
        }

        public void Accelerate()
        {
            Crystal c = bag.OfType<Crystal>().First();
            c.Use(this);
            bag.Remove(c);
        }

        public void Flee()
        {
            Move(location.neighbors[0]);
        }

        override public void Attack(Creature foe)
        {
            if (!(foe is Monster)) throw new ArgumentException();
            Monster foe_ = foe as Monster;
            if (!accelerated)
            {
                base.Attack(foe);
                if (foe_.GetHP() == 0)
                {
                    foe_.GetPack().members.Remove(foe_);
                    KillPoint++;
                }
            }
            else
            {
                foreach (Monster target in foe_.GetPack().members)
                {
                    base.Attack(target);
                }
                int packCount = foe_.GetPack().members.Count;
                foe_.GetPack().members.RemoveAll(target => target.GetHP() <= 0);
                KillPoint += (uint)(packCount - foe_.GetPack().members.Count);
                accelerated = false;
            }
        }
    }
}

