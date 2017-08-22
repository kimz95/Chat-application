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

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form{
        bool successful = true;
        string uname;

        public Form1(){
            InitializeComponent();
            textBox2.PasswordChar = '*';
            connection.EstablishConnection();
        }

        private void LoginBtn_Click(object sender, EventArgs e){
            string username = textBox1.Text;
            string password = textBox2.Text;

            connection.bw.Write(1);
            connection.bw.Write(username);
            connection.bw.Write(password);
            successful = connection.br.ReadBoolean();
            
            if (successful){
                MessageBox.Show("Login successful");
                uname = username;
                Thread th = new Thread(RunForm2);
                th.Start();
                this.Close();
            }
            else
                MessageBox.Show("Login failed");
        }
        private void RegButton_Click(object sender, EventArgs e) {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (username.Length < 4) {
                MessageBox.Show("User name should be atleast 4 characters.");
                return;
            }
            if (password.Length < 6) {
                MessageBox.Show("Password should be atleast 6 characters.");
                return;
            }

            connection.bw.Write(0);
            connection.bw.Write(username);
            connection.bw.Write(password);
            successful=connection.br.ReadBoolean();

            if (successful)
                MessageBox.Show("Registration successful");
            else
                MessageBox.Show("Registration failed");
        }


        private void RunForm2(){
            Application.Run(new contactForm(uname));
        }

    }
}
