using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Szerver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string Szerver_IP;
        static uint Felhasznalo_ID_Szamlalo = 1;
        static Dictionary<uint, Felhasznalo> Felhasznalok = new Dictionary<uint, Felhasznalo>();

        public static string IPAddress()
        {
            string externalIP = "";
            externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
            externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")).Matches(externalIP)[0].ToString();
            return externalIP;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Szerver_IP = IPAddress();
            textBox1.Text = Szerver_IP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            TcpListener tl = new TcpListener(60000);
            tl.Start();

            while (true)
            {
                if (tl.Pending())
                {
                    Felhasznalo f = new Felhasznalo()
                    {
                        ID = Felhasznalo_ID_Szamlalo++,
                        Nev = "",
                        Uzisor = new ConcurrentQueue<String>(),
                        tcp = tl.AcceptTcpClient(),
                        thread = new Thread(new ParameterizedThreadStart(Felhasznalo_szal))
                    };
                    f.Uzisor.Enqueue("Üdv a szerveren!");
                    Felhasznalok.Add(f.ID, f);

                    f.thread.Start(f);
                }
            }
        }

        static void uzi_szoras(String Uzi)
        {
            Console.WriteLine(Uzi);
            foreach (Felhasznalo f in Felhasznalok.Values.ToList())
                f.Uzisor.Enqueue(Uzi);
        }

        static void Felhasznalo_szal(Object param)
        {
            Felhasznalo f = (Felhasznalo)param;

            try
            {
                using (BinaryWriter bw = new BinaryWriter(f.tcp.GetStream()))
                {
                    using (BinaryReader br = new BinaryReader(f.tcp.GetStream()))
                    {
                        bool Bemutatkozott = false;

                        while (true)
                        {
                            if (f.tcp.Available > 0)
                            {

                            }

                            String uzi;

                            /*if (f.Uzisor.TryDequeue(out uzi))
                            {
                                bw.Write((byte)Server_Uzi_Tipusok.Chat);
                                bw.Write(uzi);
                                bw.Flush();
                            }

                            bw.Write((byte)Server_Uzi_Tipusok.Felhasznalok_Pozicioja);

                            foreach (Felhasznalo jj in Felhasznalok.Values.ToList())
                            {
                                bw.Write(jj.ID);
                                bw.Write(jj.Nev);
                            }*/

                            System.Threading.Thread.Sleep(25);
                        }
                    }
                }
            }
            catch
            {
                uzi_szoras(String.Format("{0}({1}):{2}", f.Nev, f.ID, "***KILLED***"));
            }
            finally
            {
                Felhasznalok.Remove(f.ID);
                uzi_szoras(String.Format("{0}({1}):{2}", f.Nev, f.ID, "***CLOSED***"));
            }
        }
    }
}
