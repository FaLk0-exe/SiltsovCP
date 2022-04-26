using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiltsovCP
{
    [Serializable]
    public class Profile
    {
        public string login;
        public string password;
        public Profile()
        {
            login = "";
            password = "";
        }
    }
}
