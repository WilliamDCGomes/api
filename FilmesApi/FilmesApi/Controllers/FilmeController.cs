using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        public static List<Filme> filmes = new List<Filme>();

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] Filme filme)
        {
            if(filme.Id == Guid.Empty)
            {
                filme.Id = Guid.NewGuid();
            }

            filmes.Add(filme);
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { id = filme.Id }, filme);
        }

        [HttpGet]
        [Route("RecuperaFilmes")]
        public IEnumerable<Filme> RecuperaFilmes()
        {
            return filmes;
        }

        [HttpGet()]
        [Route("RecuperaFilmesPaginados")]
        public IEnumerable<Filme> RecuperaFilmesPaginados([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return filmes.Skip(skip).Take(take);
        }

        [HttpGet()]
        [Route("RecuperaFilmesPorId")]
        public IActionResult RecuperaFilmesPorId(Guid filmeId)
        {
            try
            {
                var filme = filmes.FirstOrDefault(f => f.Id == filmeId);

                if (filme == null) return NotFound();

                return Ok(filme);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
