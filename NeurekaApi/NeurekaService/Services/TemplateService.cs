using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;
using NeurekaDAL.Repositories;

namespace NeurekaService.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        public TemplateService(ITemplateRepository TemplateRepository)
        {
            _templateRepository = TemplateRepository;
        }

        public async Task<IEnumerable<Template>> Get() => await _templateRepository.Get();
        public async Task<IEnumerable<Field>> GetFieldTemplates() => await _templateRepository.GetFieldTemplates();
        public async Task<Template> Get(string id) => await _templateRepository.Get(id);
        public async Task<Field> GetFieldTemplate(string id) => await _templateRepository.GetFieldTemplate(id);
        public async Task<Template> Create(Template Template) => await _templateRepository.Create(Template);
        public async Task<Field> CreateFieldTemplate(Field  field) => await _templateRepository.CreateFieldTemplate(field);
        public async Task Update(string id, Template Template) => await _templateRepository.Update(id, Template);
        public async Task UpdateFieldTemplate(string id, Field  field) => await _templateRepository.UpdateFieldTemplate(id, field);
        public async Task Remove(Template Template) => await _templateRepository.Remove(Template);
        public async Task RemoveFieldTemplate(Field  field) => await _templateRepository.RemoveFieldTemplate(field);
        public async Task Remove(string id) => await _templateRepository.Remove(id);
        public async Task RemoveFieldTemplate(string id) => await _templateRepository.RemoveFieldTemplate(id);

    }
}
