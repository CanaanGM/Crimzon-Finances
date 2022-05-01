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
        public async Task<IActionResult> Create(Purchase purchase)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Purchase = purchase }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] Purchase purchase)
        {
            purchase.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Purchase = purchase}));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

    }
}
