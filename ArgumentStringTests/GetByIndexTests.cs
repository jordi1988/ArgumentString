using ArgumentString.Exceptions;
using ArgumentStringNS.Exceptions;
using Xunit;

namespace ArgumentStringNS.Tests
{
    public class GetByIndexTests
    {
        [Fact]
        public void Get_ValidIndex_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get(0);

            // Assert
            Assert.Equal("bar", result);
        }

        [Fact]
        public void Indexer_ValidIndex_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut[0];

            // Assert
            Assert.Equal("bar", result);
        }

        [Fact]
        public void GetAsInteger_ValidIndex_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get<int>(1);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Get_MissingIndex_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get(2);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Indexer_MissingIndex_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut[2];

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAsInteger_MissingIndex_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>(2);
            });

            Assert.Contains("Index 2", exception.Message);
        }

        [Fact]
        public void Get_EmptyKey_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=;version=1");

            // Act
            var result = sut.Get(0);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Indexer_EmptyKey_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=;version=1");

            // Act
            var result = sut[0];

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAsInteger_EmptyKey_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=");


            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>(1);
            });

            Assert.Contains("version", exception.Message);
        }

        [Fact]
        public void Get_MissingIndexReturnNullOption_ReturnsNull()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut.Get(2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Indexer_MissingIndexReturnNullOption_ReturnsNull()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut[2];

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAsInteger_MissingIndexReturnNullOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>(2);
            });

            Assert.Contains("Index 2", exception.Message);
        }

        [Fact]
        public void Get_MissingIndexThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut.Get(2);
            });

            Assert.Contains("Index 2", exception.Message);
        }

        [Fact]
        public void Indexer_MissingIndexThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut[2];
            });

            Assert.Contains("Index 2", exception.Message);
        }

        [Fact]
        public void GetAsInteger_MissingIndexThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut.Get<int>(2);
            });

            Assert.Contains("Index 2", exception.Message);
        }
    }
}
