using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;
using PatientHistoryAPI.DTOs; // Importing DTOs

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

        // GET: api/Prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetAll()
        {
            // Includes Visit data for each Prescription
            return await _context.Prescriptions.Include(p => p.Visit).ToListAsync();
        }

        // GET: api/Prescriptions/5
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

        // POST: api/Prescriptions
        [HttpPost]
        public async Task<ActionResult<Prescription>> Create([FromBody] PrescriptionCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid prescription data.");

            var prescription = new Prescription
            {
                VisitId = dto.VisitId,
                Medication = dto.Medication,
                Dosage = dto.Dosage,
                Duration = dto.Duration,
               
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = prescription.PrescriptionId }, prescription);
        }

        // PUT: api/Prescriptions/5
        [HttpPut("{id}")]
public async Task<IActionResult> Update(int id, PrescriptionCreateDto dto)
{
    if (id != dto.PrescriptionId)
        return BadRequest("Prescription ID mismatch.");

    // Check if the VisitId exists in the Visits table
    var visitExists = await _context.Visits.AnyAsync(v => v.VisitId == dto.VisitId);
    if (!visitExists)
        return NotFound($"Visit with ID {dto.VisitId} does not exist.");

    var prescription = await _context.Prescriptions.FindAsync(id);
    if (prescription == null)
        return NotFound();

    // Update the prescription fields
    prescription.VisitId = dto.VisitId;
    prescription.Medication = dto.Medication;
    prescription.Dosage = dto.Dosage;
    prescription.Duration = dto.Duration;


    // Save changes to the database
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        // Log the error details for debugging
        Console.WriteLine(ex.InnerException?.Message);
        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the prescription.");
    }

    return NoContent();
}


        // DELETE: api/Prescriptions/5
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
