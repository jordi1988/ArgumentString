using ArgumentStringNS.Exceptions;
using Xunit;

namespace ArgumentStringNS.Tests
{
    public class GetByKeyTests
    {
        [Fact]
        public void Get_ValidKey_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get("foo");

            // Assert
            Assert.Equal("bar", result);
        }

        [Fact]
        public void Indexer_ValidKey_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut["foo"];

            // Assert
            Assert.Equal("bar", result);
        }

        [Fact]
        public void GetAsInteger_ValidKey_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get<int>("version");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Get_MissingKey_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get("missing");

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Indexer_MissingKey_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut["missing"];

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAsInteger_MissingKey_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>("missing");
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void Get_EmptyKey_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=;version=1");

            // Act
            var result = sut.Get("foo");

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Indexer_EmptyKey_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=;version=1");

            // Act
            var result = sut["foo"];

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
                _ = sut.Get<int>("version");
            });

            Assert.Contains("version", exception.Message);
        }

        [Fact]
        public void Get_MissingKeyReturnNullOption_ReturnsNull()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut.Get("missing");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Indexer_MissingKeyReturnNullOption_ReturnsNull()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut["missing"];

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAsInteger_MissingKeyReturnNullOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>("missing");
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void Get_MissingKeyThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut.Get("missing");
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void Indexer_MissingKeyThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut["missing"];
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void GetAsInteger_MissingKeyThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut.Get<int>("missing");
            });

            Assert.Contains("missing", exception.Message);
        }
    }
}
