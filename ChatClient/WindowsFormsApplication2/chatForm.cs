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
using System.IO;

namespace WindowsFormsApplication2{

    public partial class chatForm : Form{
        private BinaryWriter w;
        private BinaryReader r;
        private Thread writer;
        private string name;

        public chatForm(string name,string title,BinaryWriter w,BinaryReader r){
            InitializeComponent();
            this.w = w;
            this.r = r;
            this.name = name;
            this.Text = title;

            writer = new Thread(runner);
            writer.Start();
        }
        private void button1_Click(object sender, EventArgs e){
            try {
                if (textBox2.Text != "") {
                    string msg = textBox2.Text;
                    textBox1.AppendText(name + ": " + msg + "\n");
                    w.Write(msg);
                    textBox2.Text = null;
                }
            }
            catch (Exception ex) {
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Enter){
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
        private void runner() {
            try {
                while (true) {
                    string msg = r.ReadString();
                    textBox1.AppendText(this.Text + ": " + msg + "\n");
                }
            }
            catch (ThreadAbortException e) {

            }
            catch (Exception e) {
                textBox1.AppendText(this.Text + " has ended the chat.\n");
                Thread.Sleep(5000);
                Close();
            }
        }

        private void chatForm_FormClosed(object sender, FormClosedEventArgs e) {
            writer.Abort();
        }
    }
}
