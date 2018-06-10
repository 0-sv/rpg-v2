using System;
using System.Collections.Generic;
using STVRogue.GameLogic;
using STVRogue.Utils;

namespace STVRogue.Gamelogic {
    public class Zone { 
        private Dungeon d;

        public List<Node> nodes;

        public Zone (Dungeon d) {
            this.d = d;
            this.nodes = d.nodeList;
            nodes = IsolateZone(d.player); 
        }

        private List<Node> IsolateZone (Creature c) {
            List<Node> result = new List<Node>();
            for (int index = 0; index < nodes.Count; index++) {
                if (ReferenceEquals(nodes[index], new Bridge())) {
                    if (d.Level(nodes[index]) == d.Level(c.location)) {
                        result.Add(nodes[index]); 
                    } 
                }
            }
            return result;
        }
    }
}