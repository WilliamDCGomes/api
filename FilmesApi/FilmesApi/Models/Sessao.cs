using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Sessao
    {
        public Guid? FilmeId { get; set; }
        public virtual Filme Filme { get; set; }
        public Guid? CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; }
    }
}
