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
        private readonly IUserRepository _userRepository;
        public PatientService(IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Patient>> Get() => await _patientRepository.Get();
        public async Task<Patient> Get(string id) => await _patientRepository.Get(id);
        public async Task<Patient> Create(Patient patient)
        {
            var user = new User();

            user.FirstName = patient.FirstName;

            user.LastName = patient.LastName;

            user.FullName = patient.LastName;


            user.Sexe = patient.Sexe;

            user.JobTitle = patient.JobTitle;

            user.Email = patient.Email;

            user.Phone = patient.Phone;

            user.Role = "patient";

            user.CreatedAt = DateTime.Now.ToString();

            user.ChangePassword = false;

           var createdPatient =  await _userRepository.Create(user);
            patient.Id = createdPatient.Id;
          return  await _patientRepository.Create(patient);
        }
        public async Task Update(string id, Patient patient) => await _patientRepository.Update(id, patient);
        public async Task Remove(Patient patient) => await _patientRepository.Remove(patient);
        public async Task Remove(string id) => await _patientRepository.Remove(id);
        public async Task<Visit> GetPatientOpenVisitByPatientId (string patientId) => await _patientRepository.GetPatientOpenVisitByPatientId(patientId);
        public async Task UpdatePatientOpenVisit(Visit visit) => await _patientRepository.UpdatePatientOpenVisit(visit);
    }
}
