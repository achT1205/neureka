using System.Collections.Generic;
using System.Threading.Tasks;
using NeurekaDAL.Models;

namespace NeurekaDAL.Repositories
{
    public interface ITemplateRepository
    {
        Task<Template> Create(Template Template);
        Task<IEnumerable<Template>> Get();
        Task<Template> Get(string id);
        Task Remove(Template Template);
        Task Remove(string id);
        Task Update(string id, Template Template);
    }
}