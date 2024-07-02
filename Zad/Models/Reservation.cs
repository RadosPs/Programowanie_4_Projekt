using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerSurname { get; set; }
        public string Phone {  get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

