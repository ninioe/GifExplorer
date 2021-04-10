using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GifExplorer.Models
{
    public class GifRequestHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ReqURL { get; set; }
        [Required]
        public string ResJSON { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
