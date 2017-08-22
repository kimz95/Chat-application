using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace WindowsFormsApplication2
{
    public static class connection {

        static TcpClient Client;
        static Stream s;
        public static BinaryReader br;
        public static BinaryWriter bw;

        public static void EstablishConnection() {
            Client = new TcpClient("localhost", 5555); //"25.160.6.214" "127.0.0.1" "25.159.135.3"
            s = Client.GetStream();
            br = new BinaryReader(s);
            bw = new BinaryWriter(s);
        }

        public static void EndConnection(){
            s.Close();
            Client.Close();
            System.Environment.Exit(0);
        }

    }

    static class Program{
        [STAThread]
        static void Main(){
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}