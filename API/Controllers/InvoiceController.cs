using Application.DTOs;
using Application.Invoices;


using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InvoiceController : BaseApiController
    {
        [HttpPost("{purchaseId}")]
        public async Task<IActionResult> Create(Guid purchaseId, [FromForm] PhotoWriteDto invoices)
            => HandleResult(await Mediator.Send(new Create.Command { PurchaseId = purchaseId, Invoice = invoices }));


        [HttpPut("{purchaseId}")]
        public async Task<IActionResult>Edit(Guid purchaseId,[FromForm]Guid invoiceId, [FromForm] PhotoUpdateDto invoices)
            => HandleResult(await Mediator.Send(
                new Edit.Command { PurchaseId = purchaseId,InvoiceId = invoiceId, Invoice=invoices }));

        [HttpGet("{purchaseId}")]
        public async Task<ActionResult> Get(Guid purchaseId)
            => HandleResult(await Mediator.Send(new List.Query { PurchaseId = purchaseId }));

        [HttpDelete("{purchaseId}")]
        public async Task<ActionResult> Delete(Guid purchaseId, [FromForm] Guid invoiceId)
            => HandleResult(await Mediator.Send(new Delete.Command { PurchaseId = purchaseId, InvoiceId=invoiceId }));

    }
}
