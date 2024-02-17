using Microsoft.EntityFrameworkCore;
using MediLink.Entities;
using MediLink.Models;

namespace MediLink.Services.Contract
{
    public interface IUserService
    {
        Task<Patient> GetUser(string correo, string clave);

        Task<Patient> GetUserByEmail(string correo);

        Task<Patient> ConfirmRegisterPatient(string token);

        Task<Patient> ResetPasswordPatient(string emai);

        Task<Patient> UpdatePasswordPatient(PatientNewRequest oPatient);

        Task<PatientNewRequest> SavePatient(PatientNewRequest oPatientNew);



        Task<Practitioner> GetPractitioner(string correo, string clave);

        Task<Practitioner> GetPractitionerByEmail(string correo);

        Task<Practitioner> ConfirmRegisterPractitioner(string token);

        Task<Practitioner> ResetPasswordPractitioner(string emai);

        Task<Practitioner> UpdatePasswordPractitioner(PractitionerNewRequest oDoctor);

        Task<PractitionerNewRequest> SavePractitioner(PractitionerNewRequest oDoctor);




    }
}
