using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;

        public FilmeController(FilmeContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] Filme filme)
        {
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { id = filme.Id }, filme);
        }

        [HttpGet]
        [Route("RecuperaFilmes")]
        public IEnumerable<Filme> RecuperaFilmes()
        {
            return _context.Filmes;
        }

        [HttpGet()]
        [Route("RecuperaFilmesPaginados")]
        public IEnumerable<Filme> RecuperaFilmesPaginados([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return _context.Filmes.Skip(skip).Take(take);
        }

        [HttpGet()]
        [Route("RecuperaFilmesPorId")]
        public IActionResult RecuperaFilmesPorId(Guid filmeId)
        {
            try
            {
                var filme = _context.Filmes.FirstOrDefault(f => f.Id == filmeId);

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
