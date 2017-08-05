using System;
using DarkRift;

namespace Asgore
{
	public class Game
	{
        public Slaves player1;
        public Slaves player2;
		public int gameID;
        private bool deckSelectScene;


		public Game (Slaves player, Slaves opponent)
		{
            player.game = this;
            opponent.game = this;
            player1 = player;
            player2 = opponent;
            deckSelectScene = true;
		}
	}
}

