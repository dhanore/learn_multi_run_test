using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yoyo_web_app.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool InStock { get; set; }
        public string Secret { get; set; }
    }

    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool InStock { get; set; }
    }
}
