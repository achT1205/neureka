﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;
using MongoDB.Driver;

namespace NeurekaDAL.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly INeurekaDBContext _context;
        public PatientRepository(INeurekaDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> Get() => await _context.Patients.FindAsync(p => true).Result.ToListAsync();

        public async Task<Patient> Get(string id) => await _context.Patients.FindAsync<Patient>(p => p.Id == id).Result.FirstOrDefaultAsync();

        public async Task<Patient> Create(Patient patient)
        {
            await _context.Patients.InsertOneAsync(patient);
            return patient;
        }

        public async Task<Visit> CreatePatientVisit (Visit visit)
        {
            await _context.PatientVisits.InsertOneAsync(visit);
            return visit;
        }

        public async Task<Visit> GetPatientVisit(string id) => await _context.PatientVisits.FindAsync<Visit>(p => p.Id == id).Result.FirstOrDefaultAsync();
        public async Task<IEnumerable<Visit>> GetPatientVisitByPatientId(string patientId)
        {

            var condition = Builders<Visit>.Filter.Eq(v => v.PatientId, patientId);
            return await _context.PatientVisits.FindAsync(condition).Result.ToListAsync();

        }
        public async Task UpdatePatientVisit(string id, Visit visit) => await _context.PatientVisits.ReplaceOneAsync<Visit>(p => p.Id == id, visit);
        public async Task RemovePatientVisit(Visit visit) => await _context.PatientVisits.DeleteOneAsync<Visit>(p => p.Id == visit.Id);
        public async Task RemovePatientVisit(string id) => await _context.PatientVisits.DeleteOneAsync<Visit>(p => p.Id == id);
        public async Task Update(string id, Patient patient) => await _context.Patients.ReplaceOneAsync<Patient>(p => p.Id == id, patient);
        public async Task Remove(Patient patient) => await _context.Patients.DeleteOneAsync<Patient>(p => p.Id == patient.Id);
        public async Task Remove(string id) => await _context.Patients.DeleteOneAsync<Patient>(p => p.Id == id);

    }
}
