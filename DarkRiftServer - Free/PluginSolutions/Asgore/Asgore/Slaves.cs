using System;
using DarkRift;
using System.Collections.Generic;
using DarkRift.ConfigTools;
using DarkRift.Storage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asgore
{
	public class Slaves
	{
		public string name;
		public int PlayerID;
		public ConnectionService con;
		public bool nonavailable;
        public Game game;

		public Slaves (ConnectionService con, string name, int PlayerID)
		{
			this.con = con;
			this.name = name;
			this.PlayerID = PlayerID;
            Asgore.host.Add(con, PlayerID);
		}

        public static Slaves GetPlayerByName(string name)
        {
            foreach (KeyValuePair<int, Slaves> kvp in Asgore.slaves)
            {
                if (kvp.Value.name == name)
                {
                    return kvp.Value;
                }
            }
            return null;
        }
	}
}

