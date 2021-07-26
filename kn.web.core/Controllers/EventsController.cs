using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kn.web.core.Models;

namespace kn.web.core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : BaseController
    {
        private readonly EFContext _context;

        public EventsController(EFContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events
        [HttpPost]
        [Route("EventsFilter")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsFilter(Filter filter)
        {
            var obj = _context.Events.AsQueryable();

            DateTime temp;
            if (!DateTime.TryParse(filter.Fecha_Ini, out temp))
            {
                throw new Exception("Fecha_Ini invalida");
            }
            else if (filter.Fecha_Fin != null && filter.Fecha_Fin != "" && !DateTime.TryParse(filter.Fecha_Fin, out temp))
            {
                throw new Exception("Fecha_Fin invalida");
            }

            if (filter.Fecha_Ini != null)
                obj = obj.Where(o => o.fecha_evento >= DateTime.Parse(filter.Fecha_Ini));
            if (filter.Fecha_Fin != null && filter.Fecha_Fin != "")
                obj = obj.Where(o => o.fecha_evento <= DateTime.Parse(filter.Fecha_Fin));


            return await obj.ToListAsync();
        }


        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
