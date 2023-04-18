using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessaoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public SessaoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um sessao ao banco de dados
        /// </summary>
        /// <param name="sessaoDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("AdicionaSessao")]
        public IActionResult AdicionaSessao([FromBody] CreateSessaoDto sessaoDto)
        {
            Sessao sessao = _mapper.Map<Sessao>(sessaoDto);

            _context.Sessoes.Add(sessao);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaSessaoPorId), new { filmeId = sessao.FilmeId, cinemaId = sessao.CinemaId }, sessaoDto);
        }

        /// <summary>
        /// Recupera um ou mais sessoes do banco de dados
        /// </summary>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaSessoes")]
        public IEnumerable<ReadSessaoDto> RecuperaSessoes()
        {
            return _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes.ToList());
        }

        /// <summary>
        /// Recupera um sessao do banco de dados pelo id
        /// </summary>
        /// <param name="filmeId"></param>
        /// <param name="cinemaId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaSessaoPorId")]
        public IActionResult RecuperaSessaoPorId(Guid filmeId, Guid cinemaId)
        {
            try
            {
                var sessao = _context.Sessoes.FirstOrDefault(f => f.FilmeId == filmeId && f.CinemaId == cinemaId);

                if (sessao == null) return NotFound();

                var sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);
                return Ok(sessaoDto);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
