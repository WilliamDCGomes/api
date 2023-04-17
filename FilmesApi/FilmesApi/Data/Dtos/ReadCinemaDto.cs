using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.Dtos
{
    public class ReadCinemaDto
    {
        public Guid Id { get; set; }
        public String Nome { get; set; }
    }
}
