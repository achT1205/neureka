using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using NeurekaDAL.Models;

namespace NeurekaService.Services
{
    public interface IVisitService
    {
        Task<Visit> Create(Visit visit);
        Task<bool> DeleteFile(string fileName);
        Task<GridFSDownloadStream<ObjectId>> DownloadFromStream(string fileName);
        Task Export();
        Task<IEnumerable<Visit>> Get();
        Task<Visit> Get(string id);
        Task<IEnumerable<Visit>> GetByPatient(string patientId);
        Task<IEnumerable<Visit>> GetEventsByDate(string date = null, string doctorId = null, string type = null);
        Task Remove(Visit visit);
        Task Remove(string id);
        Task Update(string id, Visit visit);
        Task<FileInfomation> UploadFromStreamAsync(string fileName, Stream stream);
        Task<List<string>> GetReportingLabels(string patientId);
        Task<LineSery> GetReportingData(string patientId, string label);
    }
}