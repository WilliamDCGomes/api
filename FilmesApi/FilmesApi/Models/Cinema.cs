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
        public Guid EnderecoId { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual ICollection<Sessao> Sessoes { get; set; }
    }
}
