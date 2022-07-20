using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Produces("application/json")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _service;
        public AlunosController(IAlunoService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _service.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter alunos");
            }
        }
        [HttpGet("AlunoNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByName([FromQuery] string nome)
        {
            try
            {
                var alunos = await _service.GetAlunosByNome(nome);
                if(alunos.Count() == 0)
                {
                    return NotFound($"Não há aluno com '{nome}' no nome");
                }
                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Request inválido.");
            }
        }
        [HttpGet("{id:int}", Name="GetAlunoById")]
        public async Task<ActionResult<Aluno>> GetAlunoById(int id)
        {
            var aluno = await _service.GetAlunoById(id);
            if(aluno == null)
            {
                return BadRequest("Aluno não encontrado");
            }
            return Ok(aluno);
        }
        [HttpPost]
        public async Task<ActionResult> Create(Aluno aluno)
        {
            await _service.CreateAluno(aluno);
            return CreatedAtRoute(nameof(GetAlunoById), new { id = aluno.Id }, aluno);
        }
    }
}
