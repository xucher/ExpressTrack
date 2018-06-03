using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressTrack.Models {
    [Table("express")]
    public class Express {
        [Key]
		public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
	}
}
