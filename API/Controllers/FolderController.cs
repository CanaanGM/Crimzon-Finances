using Application.Core;
using Application.DTOs;
using Application.Folders;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FolderController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return HandleResult(await Mediator.Send(new List.Query {  }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }


        [HttpPost]
        public async Task<IActionResult> Create(FolderWriteDto folder)
            => HandleResult(await Mediator.Send(new Create.Command { Folder = folder }));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] FolderWriteDto folder)
            => HandleResult(await Mediator.Send(new Edit.Command { Id = Id, Folder = folder }));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
         => HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }
}
