using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   
    //this requires middleware to get redirected to
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]  //makes Controller error ignored by swagger
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}