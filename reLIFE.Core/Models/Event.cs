using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reLIFE.Core.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int UserId { get; set; }       // Foreign Key to User
        public int? CategoryId { get; set; }  // Nullable Foreign Key to Category
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } // Nullable string
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; } // Nullable
    }
}
