using Microsoft.AspNetCore.Mvc;
using FazendaBackEnd.Models;
using FazendaBackEnd_MySQL.Data;

namespace FazendaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperaturaController : ControllerBase
    {
        private FazendaContext _context;
        public TemperaturaController(FazendaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Temperatura>> GetAll()
        {
            return _context.Temperatura.ToList();

        }

        [HttpGet("{TemperaturaId}")]
        public ActionResult<Temperatura> Get(int TemperaturaId)
        {
            try
            {
                var result = _context.Temperatura.Find(TemperaturaId);
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
        public async Task<ActionResult> Post(Temperatura model)
        {
            try
            {
                _context.Temperatura.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    var cultura = await _context.Cultura.FindAsync(model.culturaId);
                    var media = (model.Tmax - model.Tmin)/2; 
                    var gd = media - cultura.Tbasal;

                    cultura.SGD = cultura.SGD + gd;
                    await _context.SaveChangesAsync();

                    return Created($"/api/Temperatura/{model.id}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpDelete("{TemperaturaId}")]
        public async Task<ActionResult> Delete(int TemperaturaId)
        {
            try
            {
                //verifica se existe Temperatura a ser excluída
                var Temperatura = await _context.Temperatura.FindAsync(TemperaturaId);
                if (Temperatura == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(Temperatura);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPut("{TemperaturaId}")]
        public async Task<IActionResult> Put(int TemperaturaId, Temperatura dadosTemperaturaAlt)
        {
            try
            {
                //verifica se existe Temperatura a ser alterado
                var result = await _context.Temperatura.FindAsync(TemperaturaId);
                if (TemperaturaId != result.id)
                {
                    return BadRequest();
                }
                result.id = dadosTemperaturaAlt.id;
                result.data = dadosTemperaturaAlt.data;
                result.Tmax = dadosTemperaturaAlt.Tmax;
                result.Tmin = dadosTemperaturaAlt.Tmin;
                result.culturaId = dadosTemperaturaAlt.culturaId;
                await _context.SaveChangesAsync();
                return Created($"/api/Temperatura/{dadosTemperaturaAlt.id}", dadosTemperaturaAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

    }
}
