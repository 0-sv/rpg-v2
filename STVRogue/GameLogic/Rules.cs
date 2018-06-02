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

        protected List<Node> IsolateZone () {
            List<Node> result = new List<Node>();
            // WIP 
            for (int index = 0; index <= d.Level(d.player.location); index++) {
                if (ReferenceEquals(d.nodeList[index], new Bridge())) {
                    index++;
                }

            }
            return result;
        }
    }

    public class RZone : BaseRule {
        public RZone(Dungeon d) : base(d) {}
    }

    public class RNode : BaseRule {
        public RNode(Dungeon d) : base(d) {}
    }

    public class RAlert : BaseRule {
        public RAlert(Dungeon d) : base(d) {}
    }

    public class REndzone : BaseRule {
        public REndzone(Dungeon d) : base(d) {}
    }
}