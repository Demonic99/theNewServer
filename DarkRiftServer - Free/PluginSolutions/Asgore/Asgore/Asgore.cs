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
        public const byte LOGINTAG = 0;
        public const byte MATCHMAKING_TAG = 1;

        //login Subject!! :3
        public const byte LOGINSUBJECT = 0;
        public const byte LOGINSUCCESS = 1;
        public const byte LOGINFAILED = 2;
        public const byte REGISTER = 3;

        //matchmaking subjects
        public const byte OPPONENTSLIST = 0;
        public const byte FRIENDSLIST = 1;









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
        public override string author { get { return "Satan"; } }
        public override string supportEmail { get { return "example@example.com"; } }



        public List<Slaves> slaves = new List<Slaves>();
        public List<Game> games = new List<Game>();







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
            foreach (Slaves slave in slaves)
            {
                if (slave.con == con)
                {
                    disconnected = slave;
                }
            }
            if(disconnected != null)
                // wenn nicht null, prüfen ob der Spieler noch ingame war ;3
            {
                slaves.Remove(disconnected);
                Interface.Log("was disconnected while logged in");
            }
        }

        bool log;

        public void OnDataReceived(ConnectionService con, ref NetworkMessage msg)
        {
            if (log)
                Interface.Log("Received data from " + msg.senderID.ToString());
            if (msg.tag == LOGINTAG)
            {
                if (msg.subject == LOGINSUBJECT)
                {
                    Login(con, msg);
                }
                if (msg.subject == REGISTER)
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
            IQueryable<User> test;
            using (var db = new AsgoreDatabaseContext("UserDatabase.sql"))
            {
                db.Database.EnsureCreated();
                test = db.Users.Where(u => u.Name == name && u.Password == pw);



                if (test.Count() != 0)
                {
                    //check if player exists in DATENBANK!!
                    slaves.Add(new Slaves(con, name, test.First().Id));
                    using (DarkRiftWriter writer = new DarkRiftWriter())
                    {
                        writer.Write(test.First().Id);
                        con.SendReply(LOGINTAG, LOGINSUCCESS, writer);
                    }
                    List<string> usernames = new List<string>();
                    foreach (Slaves slave in slaves)
                    {
                        if (slave.nonavailable == false)
                        {
                            usernames.Add(slave.name);
                        }
                    }
                    string [] usernamearray = usernames.ToArray();
                    foreach (Slaves slave in slaves)
                    {
                        if(slave.nonavailable == true)
                        {
                            continue;
                        }
                        using (DarkRiftWriter writer = new DarkRiftWriter())
                        {
                            writer.Write(slaves.Count);
                            writer.Write(usernamearray);
                        }
                    }
                }
                else
                {
                    using (DarkRiftWriter writer = new DarkRiftWriter())
                    {
                        con.SendReply(LOGINTAG, LOGINFAILED, writer);
                    }
                }
            }
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

