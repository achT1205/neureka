using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;
using MongoDB.Driver;

namespace NeurekaDAL.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly INeurekaDBContext _context;
        public TemplateRepository(INeurekaDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Template>> Get() => await _context.Templates.FindAsync(p => true).Result.ToListAsync();
        public async Task<IEnumerable<Field>> GetFieldTemplates() => await _context.FieldTemplates.FindAsync(p => true).Result.ToListAsync();

        public async Task<Template> Get(string id) => await _context.Templates.FindAsync<Template>(p => p.Id == id).Result.FirstOrDefaultAsync();
        public async Task<Field> GetFieldTemplate(string id) => await _context.FieldTemplates.FindAsync<Field>(p => p.Id == id).Result.FirstOrDefaultAsync();
        public async Task<Template> Create(Template Template)
        {
            await _context.Templates.InsertOneAsync(Template);
            return Template;
        }
        public async Task<Field> CreateFieldTemplate(Field  field)
        {
            await _context.FieldTemplates.InsertOneAsync(field);
            return field;
        }
        public async Task Update(string id, Template Template) => await _context.Templates.ReplaceOneAsync<Template>(p => p.Id == id, Template);
        public async Task UpdateFieldTemplate(string id, Field  field) => await _context.FieldTemplates.ReplaceOneAsync<Field>(p => p.Id == id, field);
        public async Task Remove(Template Template) => await _context.Templates.DeleteOneAsync<Template>(p => p.Id == Template.Id);
        public async Task Remove(string id) => await _context.Templates.DeleteOneAsync<Template>(p => p.Id == id);
        public async Task RemoveFieldTemplate(Field field ) => await _context.FieldTemplates.DeleteOneAsync<Field>(p => p.Id == field.Id);
        public async Task RemoveFieldTemplate(string id) => await _context.FieldTemplates.DeleteOneAsync<Field>(p => p.Id == id);

    }
}
