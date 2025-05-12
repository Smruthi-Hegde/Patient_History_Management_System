using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;
using PatientHistoryAPI.DTOs;

namespace PatientHistoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly PatientHistoryDbContext _context;

        public DoctorsController(PatientHistoryDbContext context)
        {
            _context = context;
        }

        // Get all doctors with their associated visits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAll()
        {
            var doctors = await _context.Doctors
                .Include(d => d.Visits)  // Include the Visits collection
                .ToListAsync();

            return Ok(doctors);
        }

        // Get a single doctor by ID with associated visits
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetById(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Visits)  // Include the Visits collection
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        // Create a new doctor record
        [HttpPost]
        public async Task<ActionResult<Doctor>> Create(DoctorCreateDto dto)
        {
            // Ensure required fields are present
            if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName))
            {
                return BadRequest("First Name and Last Name are required.");
            }

            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Specialization = dto.Specialization
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = doctor.DoctorId }, doctor);
        }

        // Update an existing doctor record
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DoctorCreateDto dto)
        {
            if (id != dto.DoctorId)
                return BadRequest("Doctor ID mismatch");

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;
            doctor.Specialization = dto.Specialization;

            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete a doctor record
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
