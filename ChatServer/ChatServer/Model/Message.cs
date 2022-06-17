using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer.Model
{
    public class Message
    {
        public string Id { get; set; }
        public string Msg{ get; set; }
        public virtual User FromUserId { get; set; }
        public virtual User ToUserId { get; set; }
        public DateTime Date { get; set; }

    }
}
