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
        protected IMediator Mediator => _medaitor ??= HttpContext.RequestServices.GetService<IMediator>() ;

        protected ActionResult HandleResult<T>(Result<T> res)
            => res == null 
            ? NotFound()
            : res.IsSuccess && res.Value != null
                ? Ok(res.Value)
                : res.IsSuccess && res.Value == null
                    ? NotFound()
                    : BadRequest(res.Error);
    }
}
