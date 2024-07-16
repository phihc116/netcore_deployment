using Microsoft.AspNetCore.Mvc;
using NewsfeedAPI.Abtractions;

namespace NewsfeedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController(IConfiguration configuration, IPlatformApi platformApi) : ControllerBase
    {          
        [HttpGet("get-config-key")]
        public string GetConfigMap()
        {
            return configuration["ApplicationName"];  
        }

        [HttpGet("get-env-key")]
        public object GetEnvKey()
        {
            return new
            {
                env = Environment.GetEnvironmentVariable("ApplicationName"),
                config = configuration["ApplicationName"]
            };
        }

        [HttpGet("get-user-platform")]
        public async Task<object> GetDataCallFromPlatform()
        {
            var result = await platformApi.GetUserAsync();
            return result;
        }
    }
}
