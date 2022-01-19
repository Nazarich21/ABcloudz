using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}
