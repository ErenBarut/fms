using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fms.Models
{
    public class Dosyalar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DosyaId { get; set; }
        public string? DosyaName { get; set; }
        public string? DosyaDescription { get; set; }
        public string DosyaPath { get; set; }
        public DateTime DosyaTime { get; set; }
        public string UserId { get; set; }





    


    }
}
