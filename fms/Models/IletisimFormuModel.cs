using System.ComponentModel.DataAnnotations;

namespace fms.Models

{
    public class IletisimFormuModel
    {
        [Required(ErrorMessage = "Adınızı giriniz")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Email adresinizi giriniz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesajınızı giriniz")]
        public string Mesaj { get; set; }
    }
}
