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
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        public TemplateController(ITemplateService TemplateService)
        {
            _templateService = TemplateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _templateService.Get());

        [Route("{id:length(24)}", Name = "GetTemplate")]
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var p = await _templateService.Get(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Template Template)
        {
            await _templateService.Create(Template);

            return Ok(CreatedAtRoute("Template", new { id = Template.Id.ToString(), Template }));
        }

        [HttpPut("id:length(24)")]
        public async Task<IActionResult> Update(string id, Template Template)
        {
            var p = _templateService.Get(id);
            if (p == null)
                return NotFound();

            await _templateService.Update(id, Template);

            return Ok();
        }

        [HttpDelete("id:length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            var p = _templateService.Get(id);
            if (p == null)
                return NotFound();

            await _templateService.Remove(id);

            return Ok();
        }
    }
}
