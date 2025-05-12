using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;

namespace PatientHistoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosesController : ControllerBase
    {
        private readonly PatientHistoryDbContext _context;

        public DiagnosesController(PatientHistoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetAll()
        {
            return await _context.Diagnoses.Include(d => d.Visit).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnosis>> GetById(int id)
        {
            var diagnosis = await _context.Diagnoses
                .Include(d => d.Visit)
                .FirstOrDefaultAsync(d => d.DiagnosisId == id);

            if (diagnosis == null)
                return NotFound();

            return diagnosis;
        }

        [HttpPost]
        public async Task<ActionResult<Diagnosis>> Create(Diagnosis diagnosis)
        {
            _context.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = diagnosis.DiagnosisId }, diagnosis);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Diagnosis diagnosis)
        {
            if (id != diagnosis.DiagnosisId)
                return BadRequest();

            _context.Entry(diagnosis).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
                return NotFound();

            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
