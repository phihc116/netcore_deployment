using Microsoft.AspNetCore.Mvc;
using NewsfeedAPI.Abtractions;
using System.Diagnostics;

namespace NewsfeedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController(IConfiguration configuration, IPlatformApi platformApi, ILogger<TestController> logger) : ControllerBase
    {
        private static readonly Random _random = new Random();
        private readonly ILogger<TestController> _logger = logger;

        [HttpGet("get-config-key")]
        public string GetConfigMap()
        {
            return configuration["TestConfig"];  
        }

        [HttpGet("get-env-key")]
        public object GetEnvKey()
        {
            return new
            {
                env = Environment.GetEnvironmentVariable("TestEnv"),
                config = configuration["TestEnv"]
            };
        }

        [HttpGet("get-user-platform")]
        public async Task<object> GetDataCallFromPlatform()
        {
            var result = await platformApi.GetUserAsync();
            return result;
        }

        [HttpGet("slow")]
        public async Task<IActionResult> SlowApi()
        {
            _logger.LogInformation("SlowApi called at {Time}", DateTime.UtcNow);

            try
            {
                // Simulate a slow API response
                await Task.Delay(5000); // Delay for 5 seconds
                _logger.LogInformation("SlowApi completed successfully.");
                return Ok("This API is intentionally slow.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in SlowApi.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("fast")]
        public IActionResult FastApi()
        {
            _logger.LogDebug("FastApi called at {Time}", DateTime.UtcNow);
            _logger.LogInformation("FastApi executed successfully.");

            return Ok("This API is fast.");
        }

        [HttpGet("memory-leak")]
        public IActionResult MemoryLeak()
        {
            _logger.LogInformation("MemoryLeak called at {Time}", DateTime.UtcNow);

            try
            {
                // Simulate a memory leak
                var leakedList = new List<byte[]>();
                for (int i = 0; i < 1000; i++)
                {
                    leakedList.Add(new byte[1024 * 1024]); // Allocate 1 MB of memory
                }

                _logger.LogInformation("MemoryLeak simulated successfully.");
                return Ok("Memory leak simulated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MemoryLeak.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("cpu-intensive")]
        public IActionResult CpuIntensive()
        {
            _logger.LogInformation("CpuIntensive called at {Time}", DateTime.UtcNow);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Simulate CPU-intensive work
                long result = 0;
                for (long i = 0; i < 1_000_000_000; i++)
                {
                    result += i;
                }
                stopwatch.Stop();

                _logger.LogInformation("CpuIntensive completed in {ElapsedMilliseconds} ms.", stopwatch.ElapsedMilliseconds);
                return Ok($"CPU-intensive task completed in {stopwatch.ElapsedMilliseconds} ms.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CpuIntensive.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("log-error")]
        public IActionResult LogError()
        {
            try
            {
                // Simulate an error
                throw new InvalidOperationException("Simulated exception for logging.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in LogError.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
