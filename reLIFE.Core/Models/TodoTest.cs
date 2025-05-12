using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reLIFE.Core.Models
{
    public class ToDo
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
    }
}
