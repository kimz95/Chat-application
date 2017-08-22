using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication2{
    public partial class Form2 : Form{
        private List<user> userList = new List<user>();
        private Thread accept;
        private Thread refresh;
        private BinaryReader r = connection.br;

        public Form2(string username){
            InitializeComponent();
            this.Text = username;

            accept = new Thread(receiveRequests);
            accept.Start();

            refresh = new Thread(receiveList);
            refresh.Start();

            ContactList.DataSource = userList;
        }

        private void ChatBtn_Click(object sender, EventArgs e){
            Thread t = new Thread(sendRequest);
            t.Start();
        }

        //Update Online List
        public void receiveList() {
            try {
                while (true) {
                    int size = r.ReadInt32();
                    userList.Clear();
                    for (int i = 0; i < size; i++) {
                        string name = r.ReadString();
                        string ip = r.ReadString();
                        userList.Add(new user(name,ip));
                    }

                    ContactList.DataSource = null;
                    ContactList.DataSource = userList;
                }
            }
            catch (Exception e) {
            }
        }

        //Sends chat requests to other clients
        public void sendRequest() {
            user u = (user)ContactList.SelectedItem;
            try {
                TcpClient l = new TcpClient(u.getIp(), 5556);
                BinaryWriter w = new BinaryWriter(l.GetStream());
                BinaryReader r = new BinaryReader(l.GetStream());

                w.Write(this.Text);
                bool reply = r.ReadBoolean();
                if(reply)
                    Application.Run(new WindowsFormsApplication2.Form3(w, r));
            }
            catch (Exception ex) {
            }
        }

        //Listens for chat requests
        public void receiveRequests() {
            TcpListener listener = new TcpListener(IPAddress.Any, 5556);
            listener.Start();

            while (true) {
                Socket peer = listener.AcceptSocket();
                RequestHandler h = new RequestHandler(peer);
            }
        }
    }
}

class user {
    private string name;
    private string ip;

    public user(string name, string ip) {
        this.name = name;
        this.ip = ip;
    }
    override public string ToString() {
        return name;
    }
    public string getIp() {
        return ip;
    }
}
class RequestHandler {
    private BinaryWriter w;
    private BinaryReader r;

    public RequestHandler(Socket socket) {
        w = new BinaryWriter(new NetworkStream(socket));
        r = new BinaryReader(new NetworkStream(socket));

        Thread t = new Thread(runner);
        t.Start();
    }

    private void runner() {
        try {
            string name = r.ReadString();
            DialogResult reply = MessageBox.Show("Do you want to start conversation with " + name + " ?","",MessageBoxButtons.YesNo);
            if (reply.ToString() == "Yes") {
                w.Write(true);
                Application.Run(new WindowsFormsApplication2.Form3(w, r));
            }
            else
                w.Write(false);
        }
        catch (Exception e) {
        }
    }

}