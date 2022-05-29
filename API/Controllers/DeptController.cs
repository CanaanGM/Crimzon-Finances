using Application.Core;
using Application.Depts;
using Application.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DeptController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedParams pagedParams)
        {

            return HandlePagedResult(await Mediator.Send(new List.Query { Params = pagedParams }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }


        [HttpPost]
        public async Task<IActionResult> Create(DeptWriteDto dept)
            => HandleResult(await Mediator.Send(new Create.Command { Dept = dept }));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] DeptWriteDto dept)
            => HandleResult(await Mediator.Send(new Edit.Command { Id = Id, Dept = dept}));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
         => HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        
}
