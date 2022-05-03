using API.Extensions;

using Application.Core;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _medaitor;
        protected IMediator Mediator => _medaitor ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> res)
            => res == null
            ? NotFound()
            : res.IsSuccess && res.Value != null
                ? Ok(res.Value)
                : res.IsSuccess && res.Value == null
                    ? NotFound()
                    : BadRequest(res.Error);

        protected ActionResult HandlePagedResult<T>(Result<CustomPagedList<T>> res)
        {
            if (res == null) return NotFound();


            if (res.IsSuccess && res.Value != null)
            {
                Response.AddPaginationHeader(
                    res.Value.CurrentPage, res.Value.PageSize, res.Value.TotalCount, res.Value.TotalPages
                    );
               return Ok(res.Value);
            }


            if (res.IsSuccess && res.Value == null)
               return NotFound();
            return BadRequest(res.Error);
        }
    }
}
