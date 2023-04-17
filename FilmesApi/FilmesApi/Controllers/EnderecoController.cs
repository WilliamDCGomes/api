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
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um endereco ao banco de dados
        /// </summary>
        /// <param name="enderecoDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("AdicionaEndereco")]
        public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            Endereco endereco = _mapper.Map<Endereco>(enderecoDto);

            _context.Enderecos.Add(endereco);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaEnderecosPorId), new { id = endereco.Id }, enderecoDto);
        }

        /// <summary>
        /// Recupera um ou mais enderecos do banco de dados, com opção de paginamento
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaEnderecos")]
        public IEnumerable<ReadEnderecoDto> RecuperaEnderecos([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos.Skip(skip).Take(take));
        }

        /// <summary>
        /// Recupera um endereco do banco de dados pelo id
        /// </summary>
        /// <param name="enderecoId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso a requisição seja feita com sucesso</response>
        [HttpGet()]
        [Route("RecuperaEnderecosPorId")]
        public IActionResult RecuperaEnderecosPorId(Guid enderecoId)
        {
            try
            {
                var endereco = _context.Enderecos.FirstOrDefault(f => f.Id == enderecoId);

                if (endereco == null) return NotFound();

                var enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
                return Ok(enderecoDto);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Atualiza um endereco do banco de dados pelo id
        /// </summary>
        /// <param name="enderecoId"></param>
        /// <param name="enderecoDto"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpPut]
        [Route("AtualizaEndereco")]
        public IActionResult AtualizaEndereco(Guid enderecoId, [FromBody] UpdateEnderecoDto enderecoDto)
        {
            try
            {
                var endereco = _context.Enderecos.FirstOrDefault(cin => cin.Id == enderecoId);
                if (endereco == null) return NotFound();
                _mapper.Map(enderecoDto, endereco);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Atualiza partes de um endereco do banco de dados pelo id
        /// </summary>
        /// <param name="enderecoId"></param>
        /// <param name="patch"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpPatch]
        [Route("AtualizaEnderecoParcial")]
        public IActionResult AtualizaEnderecoParcial(Guid enderecoId, JsonPatchDocument<UpdateEnderecoDto> patch)
        {
            try
            {
                var endereco = _context.Enderecos.FirstOrDefault(cin => cin.Id == enderecoId);
                if (endereco == null) return NotFound();

                var enderecoParaAtualizar = _mapper.Map<UpdateEnderecoDto>(endereco);

                patch.ApplyTo(enderecoParaAtualizar, ModelState);
                if (!TryValidateModel(enderecoParaAtualizar)) return ValidationProblem(ModelState);

                _mapper.Map(enderecoParaAtualizar, endereco);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deleta um endereco do banco de dados pelo id
        /// </summary>
        /// <param name="enderecoId"></param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a requisição seja feita com sucesso</response>
        [HttpDelete]
        [Route("DeletaEndereco")]
        public IActionResult DeletaEndereco(Guid enderecoId)
        {
            try
            {
                var endereco = _context.Enderecos.FirstOrDefault(fil => fil.Id == enderecoId);
                if (endereco == null) return NotFound();

                _context.Remove(endereco);
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
