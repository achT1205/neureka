using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;

namespace NeurekaService.Services
{
    public interface IPatientService
    {
        Task<Patient> Create(Patient patient);
        Task<IEnumerable<Patient>> Get();
        Task<Patient> Get(string id);
        Task Remove(Patient patient);
        Task Remove(string id);
        Task Update(string id, Patient patient);
        Task<Visit> GetPatientOpenVisitByPatientId(string patientId);
        Task UpdatePatientOpenVisit(Visit visit);
    }
}