using Microsoft.AspNetCore.Mvc;
using FazendaBackEnd.Models;
using FazendaBackEnd_MySQL.Data;

namespace FazendaBackEnd.Controllers
{
    [Route("api/fazenda/sensor")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private FazendaContext _context;
        public SensorController(FazendaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet("getAll")]
        public ActionResult<List<Sensor>> GetAll()
        {
            return _context.Sensor.ToList();

        }

        [HttpGet("{SensorId}")]
        public ActionResult<Sensor> Get(int SensorId)
        {
            try
            {
                var result = _context.Sensor.Find(SensorId);
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

        [HttpPost("add-sensor")]
        public async Task<ActionResult> Post(Sensor model)
        {
            try
            {
                _context.Sensor.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/fazenda/sensor/{model.id}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpDelete("{SensorId}")]
        public async Task<ActionResult> Delete(int SensorId)
        {
            try
            {
                //verifica se existe Sensor a ser excluída
                var Sensor = await _context.Sensor.FindAsync(SensorId);
                if (Sensor == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(Sensor);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPut("{SensorId}")]
        public async Task<IActionResult> Put(int SensorId, Sensor dadosSensorAlt)
        {
            try
            {
                //verifica se existe Sensor a ser alterado
                var result = await _context.Sensor.FindAsync(SensorId);
                if (SensorId != result.id)
                {
                    return BadRequest();
                }
                result.id = dadosSensorAlt.id;
                result.descricao = dadosSensorAlt.descricao; ;
                await _context.SaveChangesAsync();
                return Created($"/api/fazenda/sensor/{dadosSensorAlt.id}", dadosSensorAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }

    }
}
