using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime IssueDateTime { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int CreatorId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }   
        public List<Message> Messages { get; set; }
    }
}
