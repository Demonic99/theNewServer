using System;
using DarkRift;

namespace Asgore
{
	public class SlavesAtWork: Slaves
	{
		public Game game;
		public Slaves enemy;
		public Deck deck;

		public SlavesAtWork (ConnectionService con, string name, int PlayerID): base(con, name, PlayerID){
		}
	}
}

