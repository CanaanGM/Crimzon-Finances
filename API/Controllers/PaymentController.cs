using Application.Payments;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentController : BaseApiController
    {
        [HttpPost("{deptId}")]
        public async Task<IActionResult> Create(Guid deptId, [FromForm] PaymentWriteDto payment)
             => HandleResult(await Mediator.Send(new Create.Command { DeptId = deptId, Payment = payment }));


        [HttpPut("{deptId}")]
        public async Task<IActionResult> Edit(Guid deptId, [FromForm] Guid invoiceId, [FromForm] PaymentWriteDto payment)
            => HandleResult(await Mediator.Send(
                new Edit.Command { DeptId = deptId, PaymentId = invoiceId, Pyament = payment }));

        [HttpGet("{deptId}")]
        public async Task<ActionResult> Get(Guid deptId)
            => HandleResult(await Mediator.Send(new List.Query { Id = deptId }));

        [HttpDelete("{deptId}")]
        public async Task<ActionResult> Delete(Guid deptId, [FromForm] Guid paymentId)
            => HandleResult(await Mediator.Send(new Delete.Command { DeptId = deptId, PaymentId = paymentId }));
    }
}
