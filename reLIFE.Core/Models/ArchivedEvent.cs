using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reLIFE.Core.Models
{
    public class ArchivedEvent
    {
        // Using original Event Id as the primary key in the archive table
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CategoryId { get; set; } // Nullable
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } // Nullable
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime CreatedAt { get; set; } // Original creation date
        public DateTime? LastModifiedAt { get; set; } // Original last modified date (nullable)
        public DateTime ArchivedAt { get; set; } // When it was archived
    }
}
