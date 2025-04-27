using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reLIFE.Core.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public int EventId { get; set; } // Foreign Key to the Event table
        public int MinutesBefore { get; set; } // How many minutes before the event start time
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; } // When the reminder was created
    }
}
