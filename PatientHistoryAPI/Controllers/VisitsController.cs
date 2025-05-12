using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;
using PatientHistoryAPI.Dtos;

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

        // Get all visits with associated details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitReadDto>>> GetAll()
        {
            var visits = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Diagnoses)
                .Include(v => v.Prescriptions)
                .ToListAsync();

            var visitDtos = visits.Select(visit => new VisitReadDto
            {
                VisitId = visit.VisitId,
                VisitDate = visit.VisitDate,
                Notes = visit.Notes,
                // Manually map patient name
                PatientName = $"{visit.Patient.FirstName} {visit.Patient.LastName}",
                DoctorName = visit.Doctor.FirstName,
                Diagnoses = visit.Diagnoses.Select(d => d.Diagnosis1).ToList(),
                Medications = visit.Prescriptions.Select(p => p.Medication).ToList()
            }).ToList();

            return Ok(visitDtos);
        }

        // Get a single visit by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VisitReadDto>> GetById(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Diagnoses)
                .Include(v => v.Prescriptions)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
                return NotFound();

            var visitDto = new VisitReadDto
            {
                VisitId = visit.VisitId,
                VisitDate = visit.VisitDate,
                Notes = visit.Notes,
                PatientName = $"{visit.Patient.FirstName} {visit.Patient.LastName}",
                DoctorName = visit.Doctor.FirstName,
                Diagnoses = visit.Diagnoses.Select(d => d.Diagnosis1).ToList(),
                Medications = visit.Prescriptions.Select(p => p.Medication).ToList()
            };

            return Ok(visitDto);
        }

        // Create a new visit record
   
        [HttpPost]
        public async Task<ActionResult<VisitReadDto>> Create(VisitCreateDto visitCreateDto)
        {
            if (visitCreateDto == null)
            {
                return BadRequest("Visit data is required.");
            }

            // Retrieve the patient and doctor
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == visitCreateDto.PatientId);

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == visitCreateDto.DoctorId);

            if (patient == null || doctor == null)
            {
                return NotFound("Patient or Doctor not found.");
            }

            // Manually map VisitCreateDto to Visit
            var visit = new Visit
            {
                VisitDate = visitCreateDto.VisitDate,
                Notes = visitCreateDto.Notes,
                Patient = patient,
                Doctor = doctor,
                Diagnoses = visitCreateDto.Diagnoses.Select(d => new Diagnosis { Diagnosis1 = d }).ToList(),
                Prescriptions = visitCreateDto.Medications.Select(m => new Prescription { Medication = m }).ToList()
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            // Create VisitReadDto to return as the response
            var createdVisitDto = new VisitReadDto
            {
                VisitId = visit.VisitId,
                VisitDate = visit.VisitDate,
                Notes = visit.Notes,
                PatientName = $"{visit.Patient.FirstName} {visit.Patient.LastName}",
                DoctorName = visit.Doctor.FirstName,
                Diagnoses = visit.Diagnoses.Select(d => d.Diagnosis1).ToList(),
                Medications = visit.Prescriptions.Select(p => p.Medication).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = visit.VisitId }, createdVisitDto);
        }



        // Update an existing visit record
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VisitUpdateDto visitUpdateDto)
        {
            if (id != visitUpdateDto.VisitId)
                return BadRequest();

            var visit = await _context.Visits
                .Include(v => v.Patient) // Include Patient to check for null reference
                .Include(v => v.Doctor)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
                return NotFound();

            // Ensure that the Patient exists in the database
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientId == visitUpdateDto.PatientId);

            if (patient == null)
                return NotFound("Patient not found.");

            // Update the visit details
            visit.VisitDate = visitUpdateDto.VisitDate;
            visit.Notes = visitUpdateDto.Notes;
            visit.PatientId = visitUpdateDto.PatientId; // Ensure PatientId is valid
            visit.DoctorId = visitUpdateDto.DoctorId; // Ensure DoctorId is valid

            // Optionally handle Diagnoses and Medications if needed
            visit.Diagnoses = visitUpdateDto.Diagnoses?.Select(d => new Diagnosis { Diagnosis1 = d }).ToList();
            visit.Prescriptions = visitUpdateDto.Medications?.Select(m => new Prescription { Medication = m }).ToList();

            // Mark the visit as modified and save changes
            _context.Entry(visit).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // Delete a visit record
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Diagnoses)
                .Include(v => v.Prescriptions)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
                return NotFound();

            // Remove related Diagnoses and Prescriptions first
            _context.Diagnoses.RemoveRange(visit.Diagnoses);
            _context.Prescriptions.RemoveRange(visit.Prescriptions);

            // Now remove the Visit
            _context.Visits.Remove(visit);

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
