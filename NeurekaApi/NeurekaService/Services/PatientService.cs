using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;
using NeurekaDAL.Repositories;
namespace NeurekaService.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<Patient>> Get() => await _patientRepository.Get();
        public async Task<Patient> Get(string id) => await _patientRepository.Get(id);
        public async Task<Patient> Create(Patient patient) => await _patientRepository.Create(patient);
        public async Task Update(string id, Patient patient) => await _patientRepository.Update(id, patient);
        public async Task Remove(Patient patient) => await _patientRepository.Remove(patient);
        public async Task Remove(string id) => await _patientRepository.Remove(id);

    }
}
