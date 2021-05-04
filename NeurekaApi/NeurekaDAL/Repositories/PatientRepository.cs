using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;
using MongoDB.Driver;
using System.Linq;

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

        public async Task<Visit> GetPatientOpenVisitByPatientId(string patientId)
        {
            var visit = new Visit();
            var result = await _context.Visits.FindAsync(x => x.PatientId == patientId && x.Closed == false).Result.FirstOrDefaultAsync();
            if (result != null)
            {
                visit.Id = result.Id;
                visit.PatientId = result.PatientId;
                visit.DoctorId = result.DoctorId;
                visit.Title = result.Title;
                visit.StartDate = result.StartDate;
                visit.EndDate = result.EndDate;
                visit.StartTime = result.StartTime;
                visit.EndTime = result.EndTime;
                visit.Color = result.Color;
                visit.Closed = result.Closed;
                foreach (var session in result.Fields)
                {
                    if (session.IsVisible)
                    {
                        var s = new Field();
                        s.Id = session.Id;
                        s.Col = session.Col;
                        s.Icon = session.Icon;
                        s.Title = session.Title;
                        s.InputType = session.InputType;
                        s.Type = session.Type;
                        s.Model = session.Model;
                        s.Description = session.Description;
                        s.Models = session.Models;
                        s.RadioDirection = session.RadioDirection;
                        s.IsVisible = session.IsVisible;
                        s.Multiple = session.Multiple;
                        s.Readonly = session.Readonly;
                        s.Disabled = session.Disabled;
                        s.Maxlength = session.Maxlength;
                        s.Option = session.Option;
                        s.Radios = session.Radios;
                        s.Selects = session.Selects;
                        s.FileInfos = session.FileInfos;
                        foreach (var field in session.Fields)
                        {
                            var f = new Field();
                            if (field.IsVisible)
                            {
                                f.Id = field.Id;
                                f.Col = field.Col;
                                f.Icon = field.Icon;
                                f.Title = field.Title;
                                f.InputType = field.InputType;
                                f.Type = field.Type;
                                f.Model = field.Model;
                                f.Description = field.Description;
                                f.Models = field.Models;
                                f.RadioDirection = field.RadioDirection;
                                f.IsVisible = field.IsVisible;
                                f.Multiple = field.Multiple;
                                f.Readonly = field.Readonly;
                                f.Disabled = field.Disabled;
                                f.Maxlength = field.Maxlength;
                                f.Option = field.Option;
                                f.Radios = field.Radios;
                                f.Selects = field.Selects;
                                f.Fields = field.Fields;
                                f.FileInfos = field.FileInfos;
                                s.Fields.Add(f);
                            }

                        }

                        visit.Fields.Add(s);
                    }
                }
            }
            return visit;

        }

        public async Task UpdatePatientOpenVisit(Visit visit)
        {
            var visitEntity = await _context.Visits.FindAsync(x => x.Id == visit.Id).Result.FirstOrDefaultAsync();
            var sessionIndex = 0;
            foreach (var session in visit.Fields)
            {
                foreach (var field in session.Fields)
                {
                    var fieldIndex =  visitEntity.Fields[sessionIndex].Fields.FindIndex(x=> x.Id == field.Id);
                    visitEntity.Fields[sessionIndex].Fields[fieldIndex].Model = field.Model;
                    fieldIndex++;
                }
                sessionIndex++;
            }
            await _context.Visits.ReplaceOneAsync<Visit>(v => v.Id == visit.Id, visitEntity);

        }

        public async Task Update(string id, Patient patient) => await _context.Patients.ReplaceOneAsync<Patient>(p => p.Id == id, patient);
        public async Task Remove(Patient patient) => await _context.Patients.DeleteOneAsync<Patient>(p => p.Id == patient.Id);
        public async Task Remove(string id) => await _context.Patients.DeleteOneAsync<Patient>(p => p.Id == id);

    }
}
