using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressTrack.Models {
    [Table("express")]
    public class Express {
        [Key]
        [StringLength(12)]
        public string Coding { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Start { get; set; }
        [StringLength(50)]
        public string Destination { get; set; }
        [StringLength(30)]
        public string StartDate { get; set; }
    }
}
