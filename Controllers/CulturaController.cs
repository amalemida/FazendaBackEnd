using Microsoft.AspNetCore.Mvc;
using FazendaBackEnd.Models;
using FazendaBackEnd_MySQL.Data;

namespace FazendaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CulturaController : ControllerBase
    {
        private FazendaContext _context;
        public CulturaController(FazendaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Cultura>> GetAll()
        {
            return _context.Cultura.ToList();
        }

        [HttpGet("{CulturaId}")]
        public ActionResult<Cultura> Get(int CulturaId)
        {
            try
            {
                var result = _context.Cultura.Find(CulturaId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

         [HttpGet("{CulturaId}/Temperatura")]
        public ActionResult<Cultura> GetTemperatura(int CulturaId)
        {
            try
            {
                var result = _context.Temperatura.Where(t=> t.culturaId == CulturaId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cultura model)
        {
            try
            {
                _context.Cultura.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/Cultura/{model.id}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpDelete("{CulturaId}")]
        public async Task<ActionResult> Delete(int CulturaId)
        {
            try
            {
                //verifica se existe cultura a ser excluída
                var cultura = await _context.Cultura.FindAsync(CulturaId);
                if (cultura == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(cultura);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPut("{CulturaId}")]
        public async Task<IActionResult> Put(int CulturaId, Cultura dadosCulturaAlt)
        {
            try
            {
                //verifica se existe cultura a ser alterado
                var result = await _context.Cultura.FindAsync(CulturaId);
                if (CulturaId != result.id)
                {
                    return BadRequest();
                }
                result.id = dadosCulturaAlt.id;
                result.nome = dadosCulturaAlt.nome;
                result.GD = dadosCulturaAlt.GD;
                result.SGD = dadosCulturaAlt.SGD;
                result.sensorId = dadosCulturaAlt.sensorId;
                await _context.SaveChangesAsync();
                return Created($"/api/Cultura/{dadosCulturaAlt.id}", dadosCulturaAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

    }
}
