using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STVRogue.GameLogic {
    public class Specification {
        public Specification() { }

        public bool test(Gamestate currentGame) {
            throw new NotImplementedException();
        }
    }
	public class Always : Specification
	{
		private Predicate<Game> p;
		public Always(Predicate<Game> p) { this.p = p; }
		public bool test(Game G) { return p(G); }
	}
}
