using Application.DTOs;
using Application.Purchases;
using Domain;


using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PurchasesController : BaseApiController
    {


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return HandleResult(await Mediator.Send(new List.Query())) ;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm]PhotoWriteDto photos, [FromForm] PurchaseWriteDto purchase)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Photos=photos,  Purchase = purchase }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromForm] PurchaseUpdateDto purchase)
        {
           
            return HandleResult(await Mediator.Send(new Edit.Command {Id = id, Purchase = purchase}));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

    }
}
