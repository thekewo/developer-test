using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Exceptions;

namespace Taxually.TechnicalTest.Test;

public class VatRegistrationControllerTests
{
    private readonly Mock<IVatRegistrationStrategyFactory> _mockStrategyFactory;
    private readonly Mock<ILogger<VatRegistrationController>> _mockLogger;
    private readonly VatRegistrationController _controller;

    public VatRegistrationControllerTests()
    {
        _mockStrategyFactory = new Mock<IVatRegistrationStrategyFactory>();
        _mockLogger = new Mock<ILogger<VatRegistrationController>>();
        _controller = new VatRegistrationController(_mockStrategyFactory.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Post_UK_ShouldCallUKStrategy()
    {
        // Arrange
        var request = new VatRegistrationRequest { Country = "GB" };
        var mockStrategy = new Mock<IVatRegistrationStrategy>();
        _mockStrategyFactory.Setup(f => f.GetStrategy("GB")).Returns(mockStrategy.Object);

        // Act
        await _controller.PostAsync(request);

        // Assert
        mockStrategy.Verify(s => s.RegisterAsync(request), Times.Once);
    }

    [Fact]
    public async Task Post_FR_ShouldCallFranceStrategy()
    {
        // Arrange
        var request = new VatRegistrationRequest { Country = "FR" };
        var mockStrategy = new Mock<IVatRegistrationStrategy>();
        _mockStrategyFactory.Setup(f => f.GetStrategy("FR")).Returns(mockStrategy.Object);

        // Act
        await _controller.PostAsync(request);

        // Assert
        mockStrategy.Verify(s => s.RegisterAsync(request), Times.Once);
    }

    [Fact]
    public async Task Post_DE_ShouldCallGermanyStrategy()
    {
        // Arrange
        var request = new VatRegistrationRequest { Country = "DE" };
        var mockStrategy = new Mock<IVatRegistrationStrategy>();
        _mockStrategyFactory.Setup(f => f.GetStrategy("DE")).Returns(mockStrategy.Object);

        // Act
        await _controller.PostAsync(request);

        // Assert
        mockStrategy.Verify(s => s.RegisterAsync(request), Times.Once);
    }

    [Fact]
    public async Task Post_UnsupportedCountry_ShouldThrowException()
    {
        // Arrange
        var countryCode = "US";
        var request = new VatRegistrationRequest {  };
        _mockStrategyFactory.Setup(f => f.GetStrategy(countryCode)).Throws(new UnsupportedCountryException(countryCode));

        // Act
        var result = await _controller.PostAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Post_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test Company",
            CompanyId = "12345",
            Country = "GB"
        };

        var mockStrategy = new Mock<IVatRegistrationStrategy>();
        _mockStrategyFactory.Setup(f => f.GetStrategy("GB")).Returns(mockStrategy.Object);

        // Act
        var result = await _controller.PostAsync(request);

        // Assert
        Assert.IsType<OkResult>(result);
        mockStrategy.Verify(s => s.RegisterAsync(request), Times.Once);
    }

    [Fact]
    public async Task Post_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "", // Invalid: CompanyName is required
            CompanyId = "",   // Invalid: CompanyId is required
            Country = ""      // Invalid: Country is required
        };

        // Act
        var result = await _controller.PostAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Post_UnsupportedCountry_ReturnsBadRequest()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = "Test Company",
            CompanyId = "12345",
            Country = "US" // Unsupported country
        };

        _mockStrategyFactory.Setup(f => f.GetStrategy("US")).Throws(new UnsupportedCountryException("US"));

        // Act
        var result = await _controller.PostAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Country not supported: US", badRequestResult.Value);
    }

    [Fact]
    public async Task Post_NullRequest_ReturnsBadRequest()
    {
        // Arrange
        VatRegistrationRequest request = null;

        // Act
        var result = await _controller.PostAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Request body cannot be null.", badRequestResult.Value);
    }

    [Fact]
    public async Task Post_NullProperties_ReturnsBadRequest()
    {
        // Arrange
        var request = new VatRegistrationRequest
        {
            CompanyName = null, // Null property
            CompanyId = null,  // Null property
            Country = null     // Null property
        };

        // Act
        var result = await _controller.PostAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public void Constructor_NullDependencies_ThrowsArgumentNullException()
    {
        // Arrange
        IVatRegistrationStrategyFactory strategyFactory = null;
        ILogger<VatRegistrationController> logger = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new VatRegistrationController(strategyFactory, logger));
    }
}