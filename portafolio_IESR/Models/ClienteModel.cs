using System.ComponentModel.DataAnnotations;

namespace portafolio_IESR.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreEmpresa { get; set; }

        [Required]
        [StringLength(15)]
        public string NumeroTelefono { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Correo { get; set; }
    }

}