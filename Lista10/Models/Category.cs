using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lista10.Models
{
    public class Category
    {

        public int CategoryId { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "To long name, do not excced")]
        public string CategoryName { get; set; }

        public List<Article> Articles { get; set; }
    }
}
