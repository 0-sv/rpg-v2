using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
    public class Item
    {
        public String id;
        protected Boolean used = false;
		public Node location;
        public bool IsHealingPotion => false;
        public bool IsCrystal => false;
        public Item() { }
        public Item(String id) { this.id = id; }

        public bool IsUsed() => used;

        virtual public void Use(Player player)
        {
            if (used) {
                Logger.log("" + player.id + " is trying to use an expired item: "
                              + this.GetType().Name + " " + id
                              + ". Rejected.");
                throw new Exception();
            }
            else {
                Logger.log("" + player.id + " uses " + this.GetType().Name + " " + id);
                used = true;
            }
        }
    }

    public class HealingPotion : Item
    {
        public int HPvalue;
        new public bool IsHealingPotion => true;

        /* Create a healing potion with random HP-value */
        public HealingPotion(String id)
            : base(id) {
            HPvalue = 3;
        }

        override public void Use(Player player)
        {
            base.Use(player);
            player.HP = Math.Min(player.HPbase, player.HP + HPvalue);
        }
    }

    public class Crystal : Item
    {
        new public bool IsCrystal => true;
        public Crystal(String id) : base(id) { }
        override public void Use(Player player) {
            base.Use(player);
            player.accelerated = true;
            if (player.location is Bridge) 
                player.location.Disconnect(player.location as Bridge);
        }
    }
}
