using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Message
    {
        public int WriterId { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
    }
}
