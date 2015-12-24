using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public class AccountInfo
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }
        public AccountInfo(int ID, string Login, string Password, int Type)
        {
            this.ID = ID;
            this.Login = Login;
            this.Password = Password;
            this.Type = Type;
        }
    }
}
