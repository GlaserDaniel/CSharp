using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class Account
    {
        public string user { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public int port { get; set; }

        public Account(string user, string password, string server, int port)
        {
            this.user = user;
            this.password = password;
            this.server = server;
            this.port = port;
        }
    }
}
