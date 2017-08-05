using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asgore
{
    public class DatabaseDeck
    {
        public int deckId { get; set; }
        public int playerId { get; set; }
        public List<int> cardIds { get; set; } = new List<int>();
    }
}
