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

    public class Unless : Specification
    {
        private Predicate<Game> p1;
        private Predicate<Game> p2;
        public Unless(Predicate<Game> p1, Predicate<Game> p2) { this.p1 = p1; this.p2 = p2; }
        public bool test(Game G) { return p1(G) || p2(G); }
    }
}
