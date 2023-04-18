using AutoMapper;
using FilmesApi.Data.Dtos;
using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public CinemaController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um cinema ao banco de dados
        /// </summary>
        /// <param name="cinemaDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("AdicionaCinema")]
        public IActionResult AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaCinemasPorId), new { id = cinema.Id }, cinemaDto);
        }

        /// <summary>
        /// Recupera um ou mais cinemas do banco de dados, com opção de paginamento
        /// </summary>
        /// <param name="enderecoId"></param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaCinemas")]
        public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery] Guid? enderecoId = null)
        {
            if (enderecoId == null) return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.ToList());

            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw($"SELECT ID, NOME, ENDERECOID FROM CINEMAS WHERE CINEMA.ENDERECOID = {enderecoId}").ToList());
        }

        /// <summary>
        /// Recupera um cinema do banco de dados pelo id
        /// </summary>
        /// <param name="cinemaId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaCinemasPorId")]
        public IActionResult RecuperaCinemasPorId(Guid cinemaId)
        {
            try
            {
                var cinema = _context.Cinemas.FirstOrDefault(f => f.Id == cinemaId);

                if (cinema == null) return NotFound();

                var cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
                return Ok(cinemaDto);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Atualiza um cinema do banco de dados pelo id
        /// </summary>
        /// <param name="cinemaId"></param>
        /// <param name="cinemaDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpPut]
        [Route("AtualizaCinema")]
        public IActionResult AtualizaCinema(Guid cinemaId, [FromBody] UpdateCinemaDto cinemaDto)
        {
            try
            {
                var cinema = _context.Cinemas.FirstOrDefault(cin => cin.Id == cinemaId);
                if (cinema == null) return NotFound();
                _mapper.Map(cinemaDto, cinema);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Atualiza partes de um cinema do banco de dados pelo id
        /// </summary>
        /// <param name="cinemaId"></param>
        /// <param name="patch"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpPatch]
        [Route("AtualizaCinemaParcial")]
        public IActionResult AtualizaCinemaParcial(Guid cinemaId, JsonPatchDocument<UpdateCinemaDto> patch)
        {
            try
            {
                var cinema = _context.Cinemas.FirstOrDefault(cin => cin.Id == cinemaId);
                if (cinema == null) return NotFound();

                var cinemaParaAtualizar = _mapper.Map<UpdateCinemaDto>(cinema);

                patch.ApplyTo(cinemaParaAtualizar, ModelState);
                if (!TryValidateModel(cinemaParaAtualizar)) return ValidationProblem(ModelState);

                _mapper.Map(cinemaParaAtualizar, cinema);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deleta um cinema do banco de dados pelo id
        /// </summary>
        /// <param name="cinemaId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpDelete]
        [Route("DeletaCinema")]
        public IActionResult DeletaCinema(Guid cinemaId)
        {
            try
            {
                var cinema = _context.Cinemas.FirstOrDefault(fil => fil.Id == cinemaId);
                if (cinema == null) return NotFound();

                _context.Remove(cinema);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
