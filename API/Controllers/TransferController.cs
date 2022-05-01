﻿using Application.Transfers;

using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TransferController : BaseApiController
    {


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transfer transfer)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Transfer = transfer }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Transfer transfer)
        {
            transfer.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Transfer = transfer }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}
