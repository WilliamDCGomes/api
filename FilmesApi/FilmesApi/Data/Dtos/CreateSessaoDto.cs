namespace FilmesApi.Data.Dtos
{
    public class CreateSessaoDto
    {
        public Guid FilmeId { get; set; }
        public Guid CinemaId { get; set; }
    }
}
