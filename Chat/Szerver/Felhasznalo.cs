using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Szerver
{
    class Felhasznalo
    {
        public uint ID;
        public String Nev;

        public ConcurrentQueue<String> Uzisor;

        public TcpClient tcp;
        public Thread thread;
    }
}
