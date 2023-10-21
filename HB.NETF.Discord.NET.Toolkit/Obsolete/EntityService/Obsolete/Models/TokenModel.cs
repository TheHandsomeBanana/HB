using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Obsolete.Models {
    public class TokenModel {
        public string Token { get; set; }
        public string Bot { get; set; }
        public DateTime CreatedOn { get; set; }

        public TokenModel(string botName, string token, DateTime? createdOn = null) {
            Bot = botName;
            Token = token;
            CreatedOn = createdOn ?? DateTime.Now;
        }
    }
}
