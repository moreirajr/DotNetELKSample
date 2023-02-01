using DotNetELKSample.Api.ViewModels;
using DotNetELKSample.Serilog.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetELKSample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private const string _noErrorResult = "Operation finished with no errors.";
        private const string _errorResult = "Something went wrong.";
        private ILogger<SampleController> _logger;

        public SampleController(ILogger<SampleController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException("Logger cannot be null.", nameof(logger));
        }

        [HttpGet]
        [Route("success-simple-sample")]
        public IActionResult GetSimple()
        {
            return Ok(_noErrorResult);
        }

        [HttpGet]
        [Route("success-complex-sample")]
        public async Task<IActionResult> GetComplex()
        {
            await Task.Delay(5000);
            return Ok(_noErrorResult);
        }

        [HttpPost]
        [Route("failure-sample")]
        public async Task<IActionResult> GetFailureSample([FromBody] SampleViewModel vm)
        {
            try
            {
                await Task.Delay(2);

                if (vm == null)
                    return BadRequest("Invalid request. Body data can't be null.");

                if (vm.Name.ToLower() == "error")
                    throw new Exception($"Operation failed for parameter");

                return Ok(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Operation failed for {payload}", JsonSerializer.Serialize(vm));
                return BadRequest(_errorResult);
            }
        }
    }
}
