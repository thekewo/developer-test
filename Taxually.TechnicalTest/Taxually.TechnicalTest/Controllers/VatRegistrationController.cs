using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly IVatRegistrationStrategyFactory _strategyFactory;
        private readonly ILogger<VatRegistrationController> _logger;

        public VatRegistrationController(
            IVatRegistrationStrategyFactory strategyFactory,
            ILogger<VatRegistrationController> logger)
        {
            _strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] VatRegistrationRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Invalid request: request is null");
                return BadRequest("Request body cannot be null.");
            }

            _logger.LogInformation("Received VAT registration request for {CompanyId} in {Country}", request.CompanyId, request.Country);

            try
            {
                if (string.IsNullOrEmpty(request.Country))
                {
                    _logger.LogWarning("Invalid request: Country is missing");
                    return BadRequest("Country is required.");
                }

                var strategy = _strategyFactory.GetStrategy(request.Country);
                await strategy.RegisterAsync(request);

                _logger.LogInformation("Successfully processed VAT registration for {CompanyId} in {Country}", request.CompanyId, request.Country);
                return Ok();
            }
            catch (UnsupportedCountryException ex)
            {
                _logger.LogWarning(ex, "Unsupported country: {Country}", request.Country);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process VAT registration for {CompanyId} in {Country}", request.CompanyId, request.Country);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
