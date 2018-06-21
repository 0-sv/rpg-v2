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
        private Predicate<Game> p;
        private Predicate<Game> q;
        public Unless(Predicate<Game> p, Predicate<Game> q) { this.p = p; this.q = q; }
        List<bool> history = new List<bool>();
        public bool test(Game G)
        {
            bool verdict;
            if (history.Count >= 1)
            {
                bool previous = history.Last();
                verdict = !previous || (previous && (p(G) || q(G)));
            }
            else
                verdict = true;
            history.Add(p(G) && !q(G));
            return verdict;
        }
    }
}
