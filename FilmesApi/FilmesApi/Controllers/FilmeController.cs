using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="filmeDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("AdicionaFilme")]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);

            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { id = filme.Id }, filme);
        }

        /// <summary>
        /// Recupera um ou mais filmes do banco de dados, com opção de paginamento
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="nomeCinema"></param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaFilmes")]
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 10, [FromQuery] string? nomeCinema = null)
        {
            if(string.IsNullOrEmpty(nomeCinema)) return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());

            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).Where(filme => filme.Sessoes.Any(sessao => sessao.Cinema.Nome.StartsWith(nomeCinema))).ToList());
        }

        /// <summary>
        /// Recupera um filme do banco de dados pelo id
        /// </summary>
        /// <param name="filmeId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaFilmesPorId")]
        public IActionResult RecuperaFilmesPorId(Guid filmeId)
        {
            try
            {
                var filme = _context.Filmes.FirstOrDefault(f => f.Id == filmeId);

                if (filme == null) return NotFound();

                var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
                return Ok(filmeDto);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Atualiza um filme do banco de dados pelo id
        /// </summary>
        /// <param name="filmeId"></param>
        /// <param name="filmeDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpPut]
        [Route("AtualizaFilme")]
        public IActionResult AtualizaFilme(Guid filmeId, [FromBody] UpdateFilmeDto filmeDto)
        {
            try
            {
                var filme = _context.Filmes.FirstOrDefault(fil => fil.Id == filmeId);
                if (filme == null) return NotFound();
                _mapper.Map(filmeDto, filme);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Atualiza partes de um filme do banco de dados pelo id
        /// </summary>
        /// <param name="filmeId"></param>
        /// <param name="patch"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpPatch]
        [Route("AtualizaFilmeParcial")]
        public IActionResult AtualizaFilmeParcial(Guid filmeId, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            try
            {
                var filme = _context.Filmes.FirstOrDefault(fil => fil.Id == filmeId);
                if (filme == null) return NotFound();

                var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

                patch.ApplyTo(filmeParaAtualizar, ModelState);
                if (!TryValidateModel(filmeParaAtualizar)) return ValidationProblem(ModelState);

                _mapper.Map(filmeParaAtualizar, filme);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deleta um filme do banco de dados pelo id
        /// </summary>
        /// <param name="filmeId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpDelete]
        [Route("Deletafilme")]
        public IActionResult DeletaFilme(Guid filmeId)
        {
            try
            {
                var filme = _context.Filmes.FirstOrDefault(fil => fil.Id == filmeId);
                if (filme == null) return NotFound();

                _context.Remove(filme);
                _context.SaveChanges();
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
