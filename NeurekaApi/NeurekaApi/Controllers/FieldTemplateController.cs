using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeurekaDAL.Models;
using NeurekaService.Services;

namespace NeurekaApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FieldTemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        public FieldTemplateController(ITemplateService TemplateService)
        {
            _templateService = TemplateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFieldTemplates() => Ok(await _templateService.GetFieldTemplates());


        [HttpPost]
        public async Task<IActionResult> CreateFieldtemplate(Field  field)
        {
            await _templateService.CreateFieldTemplate(field);

            return Ok(CreatedAtRoute("FieldTemplate", new { id = field.Id.ToString(), field }));
        }


        [HttpPut("id:length(24)")]
        public async Task<IActionResult> Update(string id, Field  field)
        {
            var p = _templateService.Get(id);
            if (p == null)
                return NotFound();

            await _templateService.UpdateFieldTemplate(id, field);

            return Ok();
        }

        [HttpDelete("id:length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            var p = _templateService.GetFieldTemplate(id);
            if (p == null)
                return NotFound();

            await _templateService.RemoveFieldTemplate(id);

            return Ok();
        }

    }
}
