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
    public class Asgore : Plugin
    {
        //Tags!! :-3
        public enum Tags
        {
            LOGINTAG = 0, MATCHMAKING_TAG = 1
        }

        //login Subject!! :3
        public enum LoginSubjects
        {
            LOGINSUBJECT = 0, LOGINSUCCESS = 1, LOGINFAILED = 2, REGISTER = 3
        }

        public enum MatchmakingSubjects
        {
            OPPONENTSLIST = 0, FRIENDSLIST = 1
        }
        









        public override string name { get { return "Asgore"; } }
        public override string version { get { return "1.0"; } }
        public override Command[] commands
        {
            get
            {
                return new Command[]{
                    new Command("SetLog", "Turns data loging on or off.", SetLog_Command)
                };
            }
        }
        public override string author { get { return "Satan and Junior"; } }
        public override string supportEmail { get { return "example@example.com"; } }



        public static Dictionary<int, Slaves> slaves = new Dictionary<int, Slaves>();
        public static Dictionary<ConnectionService, int> host = new Dictionary<ConnectionService, int>();
        public static List<Game> games = new List<Game>();







        //constructor
        public Asgore()
        {
            Interface.Log("Nyan! :3");
            ConnectionService.onData += OnDataReceived;
            ConnectionService.onPostPlayerConnect += OnPostPlayerConnected;
            ConnectionService.onPlayerDisconnect += OnPlayerDisconnected;

            /* using (var db = new AsgoreDatabaseContext("UserDatabase.sql"))
             {
                 db.Database.EnsureCreated();
                 User go = new User { Name = "Pleb", Password = "passwort", Email = "pleb.pleb@plebpl.eb" };
                 db.Users.Add(go);
                 db.SaveChanges();
             }*/

        }

        public void OnPostPlayerConnected(ConnectionService con)
        {


        }

        public void OnPlayerDisconnected(ConnectionService con)
        {
            Slaves disconnected = null;
            foreach (KeyValuePair <int, Slaves>kvp in slaves)
            {
                Slaves slave = kvp.Value;
                if (slave.con == con)
                {
                    disconnected = slave;
                }
            }
            if(disconnected != null)
                // wenn nicht null, prüfen ob der Spieler noch ingame war ;3
            {
                slaves.Remove(disconnected.PlayerID);
                Interface.Log("was disconnected while logged in");
            }
        }

        bool log;

        public void OnDataReceived(ConnectionService con, ref NetworkMessage msg)
        {
            if (log)
                Interface.Log("Received data from " + msg.senderID.ToString());
            if (msg.tag == (byte)Tags.LOGINTAG)
            {
                if (msg.subject == (ushort)LoginSubjects.LOGINSUBJECT)
                {
                    Login(con, msg);
                }
                if (msg.subject == (ushort)LoginSubjects.REGISTER)
                {
                    Register(con, msg);
                }
            }
        }

        public void Register(ConnectionService con, NetworkMessage msg)
        {
            string name, pw, email;
            msg.DecodeData();
            using (DarkRiftReader reader = msg.data as DarkRiftReader)
            {
                name = reader.ReadString();
                pw = reader.ReadString();
                email = reader.ReadString();
            }
            IQueryable<User> test;
            using (var db = new AsgoreDatabaseContext("UserDatabase.sql"))
            {
                db.Database.EnsureCreated();
                test = db.Users.Where(u => u.Name == name && u.Email == email);
                if (test.Count() == 0)
                {
                    User go = new User { Name = name, Password = pw, Email = email };
                    db.Users.Add(go);
                    db.SaveChanges();
                    //Mitteilung, hat geklappt :3

                }
                else
                {
                    //Mitteilung, Username schon vergeben : ^)
                    Interface.Log("The Username Is Taken");
                }
            }
        }


        public void Login(ConnectionService con, NetworkMessage msg)
        {
            string name, pw;
            msg.DecodeData();
            using (DarkRiftReader reader = msg.data as DarkRiftReader)
            {
                name = reader.ReadString();
                pw = reader.ReadString();
            }
            IQueryable<User> users;
            using (var db = new AsgoreDatabaseContext("UserDatabase.sql"))
            {
                db.Database.EnsureCreated();
                users = db.Users.Where(u => u.Name == name && u.Password == pw);

                //check if player exists in DATENBANK!!
                if (users.Any())
                {
                    slaves.Add(users.First().Id, new Slaves(con, name, users.First().Id));
                    using (DarkRiftWriter writer = new DarkRiftWriter())
                    {
                        writer.Write(users.First().Id);
                        con.SendReply((byte)Tags.LOGINTAG, (ushort)LoginSubjects.LOGINSUCCESS, writer);
                    }
                    List<string> usernames = new List<string>();
                    foreach (KeyValuePair<int, Slaves> kvp in slaves)
                    {
                        Slaves slave = kvp.Value;
                        if (!slave.nonavailable)
                        {
                            usernames.Add(slave.name);
                        }
                    }
                    string [] usernamearray = usernames.ToArray();
                    foreach (KeyValuePair<int, Slaves> kvp in slaves)
                    {
                        Slaves slave = kvp.Value;
                        if (slave.nonavailable)
                        {
                            continue;
                        }
                        using (DarkRiftWriter writer = new DarkRiftWriter())
                        {
                            writer.Write(slaves.Count);
                            writer.Write(usernamearray);
                            slave.con.SendReply((byte)Tags.MATCHMAKING_TAG, (ushort)MatchmakingSubjects.OPPONENTSLIST, writer);
                        }
                    }
                }
                else
                {
                    using (DarkRiftWriter writer = new DarkRiftWriter())
                    {
                        con.SendReply((byte)Tags.LOGINTAG, (ushort)LoginSubjects.LOGINFAILED, writer);
                    }
                }
            }
        }

        public void StartGame (ConnectionService con, NetworkMessage msg)
        {
            string name;

            using (DarkRiftReader reader = (DarkRiftReader)msg.data)
            {
                name = reader.ReadString();
            }
            Slaves opponent = Slaves.GetPlayerByName(name);
            Slaves player = slaves[host[con]];
            if (opponent == null) return;
            if (player == null) return;
            //tell player "opponent not found"
            Game game = new Game(player, opponent);
            games.Add(game);
        }

        public void DeckBuild(ConnectionService con, NetworkMessage msg)
        {
        }



        public void SetLog_Command(string[] parts)
        {
            //Validate
            if (parts.Length != 1)
            {
                Interface.LogError("SetLog Command should only have 1 argument: 'on' or 'off'!");
                return;
            }

            if (parts[0] != "on" && parts[0] != "off")
            {
                Interface.LogError("SetLog Command's argument should only ever be: 'on' or 'off'!");
                return;
            }

            log = (parts[0] == "on") ? true : false;
        }
    }
}

