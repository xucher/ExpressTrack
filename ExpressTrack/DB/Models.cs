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
        public string PreTrack { get; set; }
        [StringLength(30)]
        public string StartDate { get; set; }
    }

    [Table("shipment")]
    public class Shipment {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ExpressCoding { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string CheckDate { get; set; }

        [ForeignKey("ExpressCoding")]
        public virtual Express Express { get; set; }
    }

    [Table("station")]
    public class Station {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AddressX { get; set; }
        public int? AddressY { get; set; }
    }

    [Table("user")]
    public class User {
        [Key]
        public string Coding { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
    }
}
