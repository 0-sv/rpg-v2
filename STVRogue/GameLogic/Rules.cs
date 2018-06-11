using System;
using System.Collections.Generic;
using STVRogue.Gamelogic;
using STVRogue.Utils;

namespace STVRogue.GameLogic {
    public abstract class Rule {
        public abstract bool IsActive();
    }

    public class BaseRule : Rule {
        protected Zone z;
        Predicates p = new Predicates();
        public BaseRule (Zone z) {
            this.z = z;
        }

        public override bool IsActive() {
            return true;
        }

        public virtual void AlertMonsters () {
            foreach (Node n in z.nodes) {
                foreach (Pack p in n.packs) {
                    p.alerted = true;
                }
            }
        }

        public virtual void DeAlertMonsters () {
            foreach (Node n in z.nodes) {
                foreach (Pack p in n.packs) {
                    p.alerted = false;
                }
            }
        }
    }

    public class RZone : BaseRule {
        public RZone(Zone z) : base(z) {
            this.z = z;
        }

        public bool validMove(Node dest)
        {
            if (z.nodes.Contains(dest))
                return true;
            else
                return false;
        }
    }

    public class RNode : BaseRule {
        public RNode(Zone z) : base(z) {
            this.z = z;
        }

        public bool validMove(Pack pack, Node dest)
        {
            int count = 0;
            foreach (Pack p in dest.packs)
            {
                count += p.members.Count;
            }
            int capacity = pack.dungeon.multiplier * (pack.dungeon.Level(dest) + 1);
            // count monsters already in the node:
            foreach (Pack Q in dest.packs)
            {
                capacity = capacity - Q.members.Count;
            }

            if (capacity < pack.members.Count)
                return false;
            return true;
        }
    }

    public class RAlert : BaseRule {
        public RAlert(Zone z) : base(z) {
            this.z = z;
        }

        public override void AlertMonsters () {
            base.AlertMonsters();
        }

        public override void DeAlertMonsters () {
            base.DeAlertMonsters();
        }
    }

    public class REndzone : BaseRule {
        public REndzone(Zone z) : base(z) {
            this.z = z;
        }

        public override void AlertMonsters() {
            base.AlertMonsters();
        }
    }
}