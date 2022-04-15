using Lista10.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lista10.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "To long name, do not excced")]
        public string Name { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "To long surname, do not excced")]
        public string Surname { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "To long email, do not excced")]
        public string Email { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "To long address, do not excced")]
        public string Address { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "To long city, do not excced")]
        public string City { get; set; }
        [Required]
        [MaxLength(7, ErrorMessage = "To long postal code, do not excced")]
        public string PostalCode { get; set; }


        public string Payment { get; set;  }
        [Required]
        public PaymentType PaymentEnum { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
