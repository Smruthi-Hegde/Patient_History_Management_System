using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;
using PatientHistoryAPI.DTOs;

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

        // Get all diagnoses with associated visit details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetAll()
        {
            var diagnoses = await _context.Diagnoses
                .Include(d => d.Visit)  // Include the related Visit entity
                .ToListAsync();

            return diagnoses;
        }

        // Get a single diagnosis by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnosis>> GetById(int id)
        {
            var diagnosis = await _context.Diagnoses
                .Include(d => d.Visit)  // Include the related Visit entity
                .FirstOrDefaultAsync(d => d.DiagnosisId == id);

            if (diagnosis == null)
                return NotFound();

            return diagnosis;
        }

        // Create a new diagnosis record
        [HttpPost]
        public async Task<ActionResult<Diagnosis>> Create(DiagnosisCreateDto dto)
        {
            // Check if the related Visit exists
            var visit = await _context.Visits.FindAsync(dto.VisitId);
            if (visit == null)
                return BadRequest("Invalid Visit ID");

            var diagnosis = new Diagnosis
            {
                VisitId = dto.VisitId,
                Diagnosis1 = dto.Diagnosis1,
                Description = dto.Description
            };

            _context.Diagnoses.Add(diagnosis);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = diagnosis.DiagnosisId }, diagnosis);
        }

        // Update an existing diagnosis record
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DiagnosisCreateDto dto)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
                return NotFound();

            // Check if the related Visit exists
            var visit = await _context.Visits.FindAsync(dto.VisitId);
            if (visit == null)
                return BadRequest("Invalid Visit ID");

            diagnosis.VisitId = dto.VisitId;
            diagnosis.Diagnosis1 = dto.Diagnosis1;
            diagnosis.Description = dto.Description;

            _context.Entry(diagnosis).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a diagnosis record
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
