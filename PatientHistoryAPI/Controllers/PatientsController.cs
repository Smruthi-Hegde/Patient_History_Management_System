using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;
using PatientHistoryAPI.DTOs;

namespace PatientHistoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientHistoryDbContext _context;

        public PatientsController(PatientHistoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        {
            // Include visits but avoid circular references
            var patients = await _context.Patients
                .Include(p => p.Visits)
                .ToListAsync();

            return patients;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetById(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Visits)
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
                return NotFound();

            return patient;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> Create(PatientCreateDto dto)
        {
            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                Gender = dto.Gender,
                ContactNumber = dto.ContactNumber,
                Address = dto.Address
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PatientCreateDto dto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth);
            patient.Gender = dto.Gender;
            patient.ContactNumber = dto.ContactNumber;
            patient.Address = dto.Address;

            _context.Entry(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
