using System;
using System.Collections.Generic;
using STVRogue.GameLogic;

namespace STVRogue.GameLogic {
    public abstract class Rule {
        public abstract bool IsActive();
    }

    public class BaseRule : Rule {
        protected Dungeon d;
        public BaseRule (Dungeon d) {
            this.d = d;
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
        public RZone(Dungeon d) : base(d) {}
    }

    public class RNode : BaseRule {
        public RNode(Dungeon d) : base(d) {}
    }

    public class RAlert : BaseRule {
        Zone z;
        public RAlert(Zone z) : base(d) {
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
        private Zone z;

        public REndzone(Zone z) : base(d) {
            this.z = z;
        }

        public void AlertMonsters() {
            if (z.d.Level(z.d.player.location) != z.d.Level(z.nodes[0])) {
                return;
            }
            else {
                base.AlertMonsters();
            }
        }
    }
}