using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asgore
{
    public class DatabaseDeck
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        public ushort card0id { get; set; }
        public ushort card1id { get; set; }
        public ushort card2id { get; set; }
        public ushort card3id { get; set; }
        public ushort card4id { get; set; }
        public ushort card5id { get; set; }
        public ushort card6id { get; set; }
        public ushort card7id { get; set; }
        public ushort card8id { get; set; }
        public ushort card9id { get; set; }

        public DatabaseDeck(int playerId, List<ushort> cards)
        {
            PlayerId = playerId;
            card0id = cards[0];
            card1id = cards[1];
            card2id = cards[2];
            card3id = cards[3];
            card4id = cards[4];
            card5id = cards[5];
            card6id = cards[6];
            card7id = cards[7];
            card8id = cards[8];
            card9id = cards[9];
        }

    }
}
