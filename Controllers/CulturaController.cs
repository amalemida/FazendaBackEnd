using Microsoft.AspNetCore.Mvc;
using FazendaBackEnd.Models;
using FazendaBackEnd_MySQL.Data;

namespace FazendaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FazendaController : ControllerBase
    {
        private FazendaContext _context;
        public FazendaController(FazendaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<Cultura>> GetAll()
        {
            return _context.Cultura.ToList();
        }
    }
}
