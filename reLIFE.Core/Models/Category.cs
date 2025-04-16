using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reLIFE.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key to User
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty; // e.g., "#FF0000"
    }
}
