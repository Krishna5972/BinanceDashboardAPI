using Interfaces.Clients;
using Interfaces.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Models.DTOs;
using Moq;
using Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class GetPositionHistoryAsyncTests 
    {
        private readonly Mock<BinanceService> _binanceServiceMock;
        private readonly Mock<IBinanceApiClient> _binanceApiClientMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly BinanceService _binanceService;

        public GetPositionHistoryAsyncTests()
        {
            _binanceApiClientMock = new Mock<IBinanceApiClient>();
            _cacheMock = new Mock<IMemoryCache>();
            _configurationMock = new Mock<IConfiguration>();
            _tradeRepositoryMock = new Mock<ITradeRepository>();

            // Mock configuration to provide a cache duration value correctly
            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock.Setup(m => m.Value).Returns("10");  // Provide a value as a string

            _configurationMock.Setup(c => c.GetSection("CacheSettings:CacheDurationMinutes"))
                              .Returns(configurationSectionMock.Object);

            // Mock memory cache (simulate no value found in cache)
            object cachedValue;
            _cacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
                      .Returns(false);

            // Create a partial mock of BinanceService and allow base calls for other methods
            _binanceServiceMock = new Mock<BinanceService>(
                _binanceApiClientMock.Object,
                _cacheMock.Object,
                _configurationMock.Object,
                _tradeRepositoryMock.Object
            )
            { CallBase = true };

            _binanceService = _binanceServiceMock.Object;
        }

        [Fact]
        public async Task GetPositionHistoryAsync_ShouldReturnCorrectPositions()
        {
            // Arrange: Load test data from JSON files
            var accountTrades = LoadTestData<List<FuturesAccountTradeResponseDto>>("accountTrades.json");
            var accountTradeDB = LoadTestData<List<FuturesAccountTradeResponseDto>>("accountTradeDB.json");
            var expectedPositions = LoadTestData<List<PositionHistoryResponseDto>>("positions.json");

            // Mock BinanceService's GetAccountTradesAsync method
            _binanceServiceMock.Setup(service => service.GetAccountTradesAsync())
                               .ReturnsAsync(accountTrades);

            // Mock repository response
            _tradeRepositoryMock.Setup(repo => repo.GetAllAccountTradesAsync())
                                .ReturnsAsync(accountTradeDB);

            // Act
            var result = await _binanceService.GetPositionHistoryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPositions.Count, result.Count);
            Assert.Equal(expectedPositions[0].Symbol, result[0].Symbol);
        }

        private T LoadTestData<T>(string fileName)
        {
            string testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", fileName);

            if (!File.Exists(testDataPath))
                throw new FileNotFoundException($"Test data file {fileName} not found.");

            var jsonData = File.ReadAllText(testDataPath);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
