using AutoMapper;
using PatientHistoryAPI.Models;
using PatientHistoryAPI.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Visit mappings
        CreateMap<Visit, VisitReadDto>()
            .ForMember(dest => dest.PatientName, 
                       opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}")) // Combine FirstName and LastName
            .ForMember(dest => dest.DoctorName, 
                       opt => opt.MapFrom(src => src.Doctor.FirstName)) // Assuming Doctor has a Name property
            .ForMember(dest => dest.Diagnoses, 
                       opt => opt.MapFrom(src => src.Diagnoses.Select(d => d.Diagnosis1).ToList()))
            .ForMember(dest => dest.Medications, 
                       opt => opt.MapFrom(src => src.Prescriptions.Select(p => p.Medication).ToList()));

        CreateMap<VisitCreateDto, Visit>();
        CreateMap<VisitUpdateDto, Visit>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
