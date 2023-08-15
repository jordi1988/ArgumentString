using ArgumentStringNS.Exceptions;
using Xunit;

namespace ArgumentStringNS.Tests
{
    public class GetByKeyWithDefaultValueTests
    {
        [Fact]
        public void Get_ValidKeyWithDefaultValue_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get("foo", "shouldNotSeeThis");

            // Assert
            Assert.Equal("bar", result);
            Assert.NotEqual("shouldNotSeeThis", result);
        }

        [Fact]
        public void Indexer_ValidKeyWithDefaultValue_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut["foo", "shouldNotSeeThis"];

            // Assert
            Assert.Equal("bar", result);
            Assert.NotEqual("shouldNotSeeThis", result);
        }

        [Fact]
        public void GetAsInteger_ValidKeyWithDefaultValue_ReturnsCorrectResult()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get<int>("version", 99);

            // Assert
            Assert.Equal(1, result);
            Assert.NotEqual(99, result);
        }

        [Fact]
        public void Get_MissingKeyWithDefaultValue_ReturnsDefaultValue()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get("missing", "shouldSeeThis");

            // Assert
            Assert.Equal("shouldSeeThis", result);
        }

        [Fact]
        public void Get_MissingKeyWithDefaultValueOfNull_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut.Get("missing", null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Indexer_MissingKeyWithDefaultValue_ReturnsDefaultValue()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut["missing", "shouldSeeThis"];

            // Assert
            Assert.Equal("shouldSeeThis", result);
        }

        [Fact]
        public void Indexer_MissingKeyWithDefaultValueOfNull_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            var result = sut["missing", null];

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAsInteger_MissingKeyWithDefaultValueOfNull_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>("missing", null);
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void GetAsInteger_MissingKeyWithDefaultValue_ReturnsDefaultValue()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1");

            // Act
            int result = sut.Get<int>("missing", 1);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Get_EmptyKeyWithDefaultValue_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=;version=1");

            // Act
            var result = sut.Get("foo", "shouldNotSeeThis");

            // Assert
            Assert.Equal(string.Empty, result);
            Assert.NotEqual("shouldNotSeeThis", result);
        }

        [Fact]
        public void Indexer_EmptyKeyWithDefaultValue_ReturnsEmptyString()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=;version=1");

            // Act
            var result = sut["foo", "shouldNotSeeThis"];

            // Assert
            Assert.Equal(string.Empty, result);
            Assert.NotEqual("shouldNotSeeThis", result);
        }

        [Fact]
        public void GetAsInteger_EmptyKeyWithDefaultValue_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=");


            // Act & Assert
            var exception = Assert.Throws<ConversionException>(() =>
            {
                _ = sut.Get<int>("version", 1);
            });

            Assert.Contains("version", exception.Message);
        }

        [Fact]
        public void Get_MissingKeyWithDefaultValueAndReturnNullOption_ReturnsDefaultValue()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut.Get("missing", "shouldSeeThis");

            // Assert
            Assert.Equal("shouldSeeThis", result);
        }

        [Fact]
        public void Get_MissingKeyWithDefaultValueOfNullAndReturnNullOption_ReturnsNull()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut.Get("missing", null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Indexer_MissingKeyWithDefaultValueAndReturnNullOption_ReturnsDefaultValue()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut["missing", "shouldSeeThis"];

            // Assert
            Assert.Equal("shouldSeeThis", result);
        }

        [Fact]
        public void Indexer_MissingKeyWithDefaultValueOfNullAndReturnNullOption_ReturnsNull()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            var result = sut["missing", null];

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAsInteger_MissingKeyWithDefaultValueAndReturnNullOption_ReturnsDefaultValue()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ReturnEmptyStringInsteadOfNull = false);

            // Act
            int result = sut.Get<int>("missing", 1);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Get_MissingKeyWithDefaultValueAndThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1", 
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut.Get("missing", "shouldNotSeeThis");
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void Indexer_MissingKeyWithDefaultValueAndThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut["missing", "shouldNotSeeThis"];
            });

            Assert.Contains("missing", exception.Message);
        }

        [Fact]
        public void GetAsInteger_MissingKeyWithDefaultValueAndThrowIfNotFoundOption_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar;version=1",
                (options) => options.ThrowOnAccessIfKeyNotFound = true);

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut.Get<int>("missing", 1);
            });

            Assert.Contains("missing", exception.Message);
        }
    }
}
