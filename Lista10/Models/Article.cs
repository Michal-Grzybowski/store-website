using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lista10.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "To long name, do not excced")]
        public string ArticleName { get; set; }
        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public int Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string Picture { get; set; }
    }
}
