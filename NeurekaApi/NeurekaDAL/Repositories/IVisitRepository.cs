using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using NeurekaDAL.Models;

namespace NeurekaDAL.Repositories
{
    public interface IVisitRepository
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
    }
}