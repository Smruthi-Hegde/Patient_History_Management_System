using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAll()
        {
            return await _context.Doctors.Include(d => d.Visits).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetById(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Visits)
                .FirstOrDefaultAsync(d => d.DoctorId == id);

            if (doctor == null)
                return NotFound();

            return doctor;
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> Create(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = doctor.DoctorId }, doctor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
                return BadRequest();

            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

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
