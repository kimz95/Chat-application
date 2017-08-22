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
    public partial class contactForm : Form{
        private List<user> userList = new List<user>();
        public static SortedSet<string> inchat = new SortedSet<string>();
        private Thread accept;
        private Thread refresh;
        private TcpListener listener;
        private BinaryReader r = connection.br;

        public contactForm(string username){
            InitializeComponent();
            this.Text = username;

            accept = new Thread(receiveRequests);
            accept.Start();

            refresh = new Thread(receiveList);
            refresh.Start();
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
                        userList.Add(new user(name, ip));
                    }

                    ContactList.DataSource = null;
                    ContactList.DataSource = userList;
                }
            }
            catch (ThreadAbortException e) {
                Thread.ResetAbort();
            }
            catch (Exception e) {
            }
        }

        //Sends chat requests to other clients
        public void sendRequest() {
            user u = (user)ContactList.SelectedItem;
            try {
                if (inchat.Contains(u.ToString()))
                    return;
                else
                    inchat.Add(u.ToString());

                TcpClient l = new TcpClient(u.getIp(), 5556);
                BinaryWriter w = new BinaryWriter(l.GetStream());
                BinaryReader r = new BinaryReader(l.GetStream());

                w.Write(this.Text);
                bool reply = r.ReadBoolean();
                if (reply) {
                    Application.Run(new WindowsFormsApplication2.chatForm(this.Text, u.ToString(), w, r));
                }

                inchat.Remove(u.ToString());
                w.Close();
                r.Close();
                l.Close();
                Application.ExitThread();
            }
            catch (Exception ex) {
            }
        }

        //Listens for chat requests
        public void receiveRequests() {
            listener = new TcpListener(IPAddress.Any, 5556);
            try {
                listener.Start();

                while (true) {
                    Socket peer = listener.AcceptSocket();
                    RequestHandler h = new RequestHandler(this.Text, peer);
                }
            }
            catch (Exception e) {
            }
        }

        private void contactForm_FormClosed(object sender, FormClosedEventArgs e) {
            listener.Stop();
            refresh.Abort();
            connection.EndConnection();
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
    private Socket socket;
    private BinaryWriter w;
    private BinaryReader r;
    private string title;

    public RequestHandler(string title,Socket socket) {
        this.socket = socket;
        w = new BinaryWriter(new NetworkStream(socket));
        r = new BinaryReader(new NetworkStream(socket));
        this.title = title;

        Thread t = new Thread(runner);
        t.Start();
    }

    private void runner() {
        try {
            string name = r.ReadString();
            SortedSet<string> set = WindowsFormsApplication2.contactForm.inchat;
            if (set.Contains(name))
                return;
            else
                set.Add(name);

            DialogResult reply = MessageBox.Show("Do you want to start conversation with " + name + " ?","",MessageBoxButtons.YesNo);
            if (reply.ToString() == "Yes") {
                w.Write(true);
                Application.Run(new WindowsFormsApplication2.chatForm(title, name, w, r));
            }
            else {
                w.Write(false);
            }
            set.Remove(name);
            w.Close();
            r.Close();
            socket.Close();
            Application.ExitThread();
        }
        catch (Exception e) {
        }
    }
}