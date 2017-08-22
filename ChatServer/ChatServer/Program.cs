using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ChatServer{
    class Program{
        static void Main(string[] args){
            Console.CursorVisible = false;
            Console.Title = "Chatting Server";
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Exit by Ctrl-C");
            SERVER.begin();
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e) {
            SERVER.end();
        }
    }
}

class user {
    private string name;
    private string pass;
    private string ip;
    private BinaryWriter writer;

    public user(string name,string pass) {
        this.name = name;
        this.pass = pass;
    }
    public string getName() {
        return name;
    }
    public string getPass() {
        return pass;
    }
    public string getIP() {
        return ip;
    }
    public void setIp(Socket socket) {
        ip = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
    }
    public void setWriter(BinaryWriter writer) {
        this.writer = writer;
    }
    public BinaryWriter getWriter() {
        return writer;
    }
    public bool match(string s) {
        return name == s;
    }
    public bool valid(string s, string p) {
        return (name == s) && (pass == p);
    }
}

class SERVER {
    private static List<user> Users;
    private static List<user> OnlineUsers;
    private static TcpListener server;
    private static Object o = new Object();

    public static void begin() {
        Users = load();
        OnlineUsers = new List<user>();

        try {
            server = new TcpListener(IPAddress.Any, 5555);
            server.Start();

            Console.WriteLine("Server Started on " + (server.LocalEndpoint as IPEndPoint).Address);

            while (true) {
                Socket client = server.AcceptSocket();
                Console.WriteLine((client.RemoteEndPoint as IPEndPoint).Address + " has connected.");
                Handler h = new Handler(client);
            }
        }
        catch (Exception e) {
        }
    }
    public static void end() {
        save();
        if (server != null)
            server.Stop();
    }

    private static void save() {
        Console.WriteLine("Saving Data...");
        FileStream f = new FileStream(".\\user.shit", FileMode.Create, FileAccess.Write);
        BinaryWriter w = new BinaryWriter(f);

        w.Write(Users.Count);
        foreach (user tmp in Users) {
            w.Write(tmp.getName());
            w.Write(tmp.getPass());
        }
        w.Close();
        f.Close();
    }
    private static List<user> load() {
        Console.WriteLine("Loading Data...");
        List<user> data = new List<user>();

        if (!File.Exists(".\\user.shit"))
            return data;
        FileStream f = new FileStream(".\\user.shit", FileMode.Open, FileAccess.Read);
        BinaryReader r = new BinaryReader(f);

        int size = r.ReadInt32();
        for (int i = 0; i < size; i++) {
            string name = r.ReadString();
            string pass = r.ReadString();
            data.Add(new user(name,pass));
        }

        r.Close();
        f.Close();
        return data;
    }

    public static user signup(string name,string pass) {
        foreach (user t in Users) {
            if (t.match(name))
                return null;
        }
        user u = new user(name, hash(pass));
        Users.Add(u);
        return u;
    }
    public static user login(string name, string pass,Socket s) {
        foreach (user u in Users) {
            if (u.valid(name, hash(pass))) {
                lock (o) {
                    u.setIp(s);
                    Console.WriteLine(u.getIP() + " has loggedin as " + u.getName() + ".");
                    OnlineUsers.Add(u);
                }
                return u;
            }
        }
        return null;
    }

    private static string hash(string str) {
        char[] tmp = new char[str.Length * 2];
        for (int i = 0; i < tmp.Length; i += 2) {
            int j = i / 2;
            tmp[i] = (char)(str[j] + 5);
            tmp[i + 1] = (char)(str[j] + tmp[i]);
        }
        return new string(tmp);
    }

    public static void remove(user u) {
        if (u == null)
            return;
        try {
            lock (o) {
                OnlineUsers.Remove(u);
            }
        }
        catch (Exception e) {
        }
        updateList();
    }
    public static void updateList() {
        Console.WriteLine(OnlineUsers.Count + " users online.");
        lock (o) {
            foreach (user i in OnlineUsers) {
                try {
                    BinaryWriter w = i.getWriter();

                    w.Write(OnlineUsers.Count - 1);
                    foreach (user j in OnlineUsers) {
                        if (i == j)
                            continue;
                        w.Write(j.getName());
                        w.Write(j.getIP());
                    }

                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }
    }
}

class Handler {
    private Socket s;
    private Thread t;
    private user u;

    public Handler(Socket s) {
        this.s = s;
        t = new Thread(runner);
        t.Start();
    }
    public void runner() {
        BinaryReader r = new BinaryReader(new NetworkStream(s));
        BinaryWriter w = new BinaryWriter(new NetworkStream(s));
        try {
            while (true) {
                int type = r.ReadInt32();
                string name = r.ReadString();
                string pass = r.ReadString();
                if (type == 0) {
                    u = SERVER.signup(name, pass);
                    Console.WriteLine((s.RemoteEndPoint as IPEndPoint).Address + " has registered as " + u.getName() + ".");
                    if (u == null)
                        w.Write(false);
                    else
                        w.Write(true);
                }
                else if (type == 1) {
                    u = SERVER.login(name, pass, s);
                    if (u == null)
                        w.Write(false);
                    else {
                        w.Write(true);
                        u.setWriter(w);
                        SERVER.updateList();
                        break;
                    }
                }
            }
            int junk = r.ReadInt32();//For detecting Closing the connection.
        }
        catch (Exception e) {
            if(u != null)
                Console.WriteLine(u.getName() + " has diconnected.");
            SERVER.remove(u);
            s.Close();
        }
    }
}