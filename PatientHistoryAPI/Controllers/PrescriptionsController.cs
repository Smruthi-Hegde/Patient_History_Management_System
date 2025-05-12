using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;

namespace PatientHistoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly PatientHistoryDbContext _context;

        public PrescriptionsController(PatientHistoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetAll()
        {
            return await _context.Prescriptions.Include(p => p.Visit).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetById(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Visit)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null)
                return NotFound();

            return prescription;
        }

        [HttpPost]
        public async Task<ActionResult<Prescription>> Create(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = prescription.PrescriptionId }, prescription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Prescription prescription)
        {
            if (id != prescription.PrescriptionId)
                return BadRequest();

            _context.Entry(prescription).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
                return NotFound();

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
