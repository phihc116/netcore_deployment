using Microsoft.AspNetCore.Mvc;

namespace PlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    { 
        [HttpGet("list")]
        public object Get()
        {
            return new List<object>
            {
                new
                {
                    UserId = 1,
                    FullName = "Hoàng Phi"
                },
                new
                {
                    UserId = 2,
                    FullName = "Test Deployment"
                }
            }; 
        }
    }
}
