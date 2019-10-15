using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace RomanianVioletRollsRoyce.Data.UnitTests
{
    public static class MockMemoryCache
    {
        public static Mock<IMemoryCache> CreateMemoryMock(object expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache.Setup(e => e.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);
            return mockMemoryCache;
        }
    }
}