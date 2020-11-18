using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;
using NeurekaDAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ChoETL;
using System.IO;

namespace NeurekaDAL.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly INeurekaDBContext _context;
        private readonly IGridFSBucket _gridFSBucket;
        public VisitRepository(/*IClinicalAppDbConnetionSettings settings,*/ INeurekaDBContext context/* IGridFSBucket gridFSBucket*/)
        {
            //var client = new MongoClient(settings.ConnectionString);
            //var database = client.GetDatabase(settings.DatabaseName);
            _context = context;
            _gridFSBucket = new GridFSBucket(_context.Db);
        }

        public async Task<IEnumerable<Visit>> Get() => await _context.Visits.FindAsync(p => true).Result.ToListAsync();

        public async Task Export()
        {
            using (var w = new ChoCSVWriter("visits.csv").WithFirstLineHeader())
            {
                w.Write(await _context.Visits.FindAsync(p => true).Result.ToListAsync());
            }
        }

        public async Task<IEnumerable<Visit>> GetEventsByDate(string date = null, string doctorId = null, string type = null)
        {

            if (type != "day")
            {
                if (!string.IsNullOrEmpty(doctorId))
                {
                    var condition = Builders<Visit>.Filter.Eq(v => v.DoctorId, doctorId);
                    return await _context.Visits.FindAsync(condition).Result.ToListAsync();
                }
                else
                {
                    return await _context.Visits.FindAsync(p => true).Result.ToListAsync();
                }

            }
            else
            {
                var date_str = "";
                if (!string.IsNullOrEmpty(date))
                {
                    date_str = date;
                }
                else
                {
                    var today = DateTime.Now;
                    date_str = today.ToString("yyyy-MM-dd");
                }

                var condition = Builders<Visit>.Filter.Eq(v => v.StartDate, date_str);
                if (!string.IsNullOrEmpty(doctorId))
                {
                    condition = condition & Builders<Visit>.Filter.Eq(v => v.DoctorId, doctorId);
                }
                return await _context.Visits.FindAsync(condition).Result.ToListAsync();
            }
        }

        public async Task<Visit> Get(string id) => await _context.Visits.FindAsync<Visit>(p => p.Id == id).Result.FirstOrDefaultAsync();
        public async Task<IEnumerable<Visit>> GetByPatient(string patientId)
        {

            var condition = Builders<Visit>.Filter.Eq(v => v.PatientId, patientId);
            return await _context.Visits.FindAsync(condition).Result.ToListAsync();

        }
        public async Task<Visit> Create(Visit visit)
        {
            await _context.Visits.InsertOneAsync(visit);
            return visit;
        }
        public async Task Update(string id, Visit visit) => await _context.Visits.ReplaceOneAsync<Visit>(p => p.Id == id, visit);
        public async Task Remove(Visit visit) => await _context.Visits.DeleteOneAsync<Visit>(p => p.Id == visit.Id);
        public async Task Remove(string id) => await _context.Visits.DeleteOneAsync<Visit>(p => p.Id == id);

        public async Task<FileInfomation> UploadFromStreamAsync(string fileName, Stream stream)
        {
            var name = Guid.NewGuid().ToString() + "_" + fileName;
            await _gridFSBucket.UploadFromStreamAsync(name, stream);
            var result = new FileInfomation();
            result.FileName = name;
            result.Title = fileName;
            return result;
        }

        public async Task<GridFSDownloadStream<ObjectId>> DownloadFromStream(string fileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            var finData = await _gridFSBucket.FindAsync(filter);
            var firstData = finData.FirstOrDefault();
            var bsonId = firstData.Id;
            var result = await _gridFSBucket.OpenDownloadStreamAsync(bsonId);
            return result;

        }

        public async Task<bool> DeleteFile(string fileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            var finData = await _gridFSBucket.FindAsync(filter);
            var firstData = finData.FirstOrDefault();
            var bsonId = firstData.Id;
            await _gridFSBucket.DeleteAsync(bsonId);
            return true;
        }

    }
}
