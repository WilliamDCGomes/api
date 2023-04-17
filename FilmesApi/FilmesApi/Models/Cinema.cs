using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Cinema
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O nome do cinema é obrigatório!")]
        public String Nome { get; set; }
    }
}
