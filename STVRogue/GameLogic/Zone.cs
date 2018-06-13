using System;
using System.Collections.Generic;
using STVRogue.GameLogic;
using STVRogue.Utils;

namespace STVRogue.Gamelogic {
    public class Zone { 
        private Dungeon d;

        public List<Node> nodes;

        public Zone (Dungeon d, Creature c) {
            this.d = d;
            this.nodes = d.nodeList;
            nodes = IsolateZone(c); 
        }

        private List<Node> IsolateZone (Creature c) {
            List<Node> result = new List<Node>();
            for (int index = 0; index < nodes.Count; index++)
                if (d.CurrentLevel(nodes[index]) == d.CurrentLevel(c.location))
                    result.Add(nodes[index]); 
            return result;
        }
    }
}