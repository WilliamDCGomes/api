using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);

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
