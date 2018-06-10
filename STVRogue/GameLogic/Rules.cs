using System;
using System.Collections.Generic;
using STVRogue.Gamelogic;
using STVRogue.GameLogic;

namespace STVRogue.GameLogic {
    public abstract class Rule {
        public abstract bool IsActive();
    }

    public class BaseRule : Rule {
        protected Zone z;
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


    }

    public class RNode : BaseRule {
        public RNode(Zone z, Pack p) : base(z) {
            this.z = z;
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