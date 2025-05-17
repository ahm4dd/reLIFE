using System;

namespace reLIFE.Core.Models
{
    public class ReminderInfo
    {
        public int ReminderId { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime EventStartTime { get; set; }
        public int MinutesBefore { get; set; }
        public bool IsEnabled { get; set; } // Will typically be true for "active" reminders

        public DateTime ReminderTime => EventStartTime.AddMinutes(-MinutesBefore);
    }
}