using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using NeurekaDAL.Models;
using NeurekaDAL.Repositories;

namespace NeurekaService.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        public VisitService(IVisitRepository visitRepository)
        {
            _visitRepository = visitRepository;
        }

        public async Task<IEnumerable<Visit>> Get() => await _visitRepository.Get();
        public async Task Export() => await _visitRepository.Export();
        public async Task<Visit> Get(string id) => await _visitRepository.Get(id);
        public async Task<IEnumerable<Visit>> GetByPatient(string patientId) => await _visitRepository.GetByPatient(patientId);
        public async Task<Visit> Create(Visit visit) => await _visitRepository.Create(visit);
        public async Task Update(string id, Visit visit) => await _visitRepository.Update(id, visit);
        public async Task Remove(Visit visit) => await _visitRepository.Remove(visit);
        public async Task Remove(string id) => await _visitRepository.Remove(id);
        public async Task<FileInfomation> UploadFromStreamAsync(string fileName, Stream stream) => await _visitRepository.UploadFromStreamAsync(fileName, stream);
        public async Task<GridFSDownloadStream<ObjectId>> DownloadFromStream(string fileName) => await _visitRepository.DownloadFromStream(fileName);
        public async Task<bool> DeleteFile(string fileName) => await _visitRepository.DeleteFile(fileName);
        public async Task<IEnumerable<Visit>> GetEventsByDate(string date = null, string doctorId = null, string type = null) => await _visitRepository.GetEventsByDate(date, doctorId, type);

        public async Task<List<string>> GetReportingLabels(string patientId) => await _visitRepository.GetReportingLabels(patientId);

        public async Task<PatientDataSet> GetReportingData(string patientId, string label) => await _visitRepository.GetReportingData(patientId, label);
    }
}
