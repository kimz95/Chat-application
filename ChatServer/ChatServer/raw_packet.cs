using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum MSG {
    signup, //Signing UP (Client to Server)
    login,  //Logging in (Client to Server)
    text,   //Chat Format Message (Client to Client)
    request,//Chatting Request with another Client (Client to Client)
    reply,  //Pass or Fail for Logging in and Signing up (Server to Client)
    olist,  //List of Online Users (Server to Client)
}
//Protocol
class raw_packet {
    protected MSG type;
}
class signup : raw_packet {
    private string name;
    private string pass;

    public signup(string name, string pass) {
        type = MSG.signup;
        this.name = name;
        this.pass = pass;
    }

    public string getName() {
        return name;
    }
    public string getPass() {
        return pass;
    }
}
class login : raw_packet {
    private string name;
    private string pass;

    public login(string name, string pass) {
        type = MSG.login;
        this.name = name;
        this.pass = pass;
    }

    public string getName() {
        return name;
    }
    public string getPass() {
        return pass;
    }
}
class text : raw_packet {
    private string msg;

    public text(string msg) {
        this.msg = msg;
    }

    public string getText() {
        return msg;
    }
}
class request : raw_packet {
    public request() {
        type = MSG.request;
    }
}
class reply : raw_packet {
    private bool answer;

    public reply(bool answer) {
        type = MSG.reply;
        this.answer = answer;
    }

    public bool hasPassed() {
        return answer;
    }
}
class olist : raw_packet {
    private List<user> ousers;

    public olist(List<user> ousers) {
        type = MSG.olist;
        this.ousers = ousers;
    }

    public List<user> getList() {
        return ousers;
    }
}