using System;
using DarkRift;

namespace Asgore
{
	public class Slaves
	{
		public string name;
		public int PlayerID;
		public ConnectionService con;
		public bool nonavailable;

		public Slaves (ConnectionService con, string name, int PlayerID)
		{
			this.con = con;
			this.name = name;
			this.PlayerID = PlayerID;
		}
	}
}

