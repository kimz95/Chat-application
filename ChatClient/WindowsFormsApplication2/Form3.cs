using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication2{

    public partial class Form3 : Form{
        private BinaryWriter w;
        private BinaryReader r;

        public Form3(BinaryWriter w,BinaryReader r){
            InitializeComponent();
            this.w = w;
            this.r = r;
        }

        private void button1_Click(object sender, EventArgs e){
            if (textBox2.Text != ""){
                textBox1.AppendText(textBox2.Text + "\n");
                textBox2.Text = null;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}
