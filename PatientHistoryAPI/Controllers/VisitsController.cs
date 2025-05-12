using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;

namespace PatientHistoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly PatientHistoryDbContext _context;

        public VisitsController(PatientHistoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visit>>> GetAll()
        {
            return await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Diagnoses)
                .Include(v => v.Prescriptions)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetById(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Diagnoses)
                .Include(v => v.Prescriptions)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
                return NotFound();

            return visit;
        }

        [HttpPost]
        public async Task<ActionResult<Visit>> Create(Visit visit)
        {
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = visit.VisitId }, visit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Visit visit)
        {
            if (id != visit.VisitId)
                return BadRequest();

            _context.Entry(visit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
                return NotFound();

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
