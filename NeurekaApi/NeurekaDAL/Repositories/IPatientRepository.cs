using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;

namespace NeurekaDAL.Repositories
{
    public interface IPatientRepository
    {
        Task<Patient> Create(Patient patient);
        Task<IEnumerable<Patient>> Get();
        Task<Patient> Get(string id);
        Task Remove(Patient patient);
        Task Remove(string id);
        Task Update(string id, Patient patient);
        Task<Visit> CreatePatientVisit(Visit visit);
        Task<Visit> GetPatientVisit(string id);
        Task UpdatePatientVisit(string id, Visit visit);
        Task RemovePatientVisit(Visit visit);
        Task RemovePatientVisit(string id);
        Task<IEnumerable<Visit>> GetPatientVisitByPatientId(string patientId);
    }
}