using ArgumentStringNS.Exceptions;
using Xunit;

namespace ArgumentStringNS.Tests
{
    public class ArgumentStringTests
    {
        [Fact]
        public void Constructor_ValidArgumentsWithNoOptions_DoesNotThrow()
        {
            // Arrange & Act
            var exception = Record.Exception(() =>
            {
                _ = new ArgumentString(@"foo=bar");
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_ValidArgumentsWithValidMandatoryOptions_DoesNotThrow()
        {
            // Arrange & Act
            var exception = Record.Exception(() =>
            {
                _ = new ArgumentString(@"foo=bar", (options) =>
                {
                    options.MandatoryKeys = new List<string>() { "foo" };
                });
            });
            
            
            var argumentsExample1 = new ArgumentString("foo=bar;version=1", new ParseOptions("foo") { /* ... */ });

            var argumentsExample2 = new ArgumentString("foo=bar;version=1", options => { 
                options.MandatoryKeys = new List<string> { "foo" };
            });

            var argumentsExample3 = new ArgumentString("foo->bar|version->1", options => { 
                options.ArgumentSeparator = "|";
                options.KeyValueSeparator = "->";
                options.ThrowOnAccessIfKeyNotFound = true;
            });
            var test = argumentsExample3.Get<float>("version");

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_ValidArgumentsWithMissingMandatoryKey_ThrowsException()
        {
            // Arrange, Act & Assert
            Assert.Throws<MissingArgumentException>(() =>
            {
                _ = new ArgumentString(@"foo=bar", (options) =>
                {
                    options.MandatoryKeys = new List<string>() { "missing" };
                });
            });
        }

        [Fact]
        public void IndexByInteger_ValidIndex_ReturnsCorrectResult()
        {
            // Arrange & Act
            var sut = new ArgumentString(@"foo=bar");

            var result = sut[0];

            // Assert
            Assert.Equal("bar", result);
        }

        [Fact]
        public void IndexByInteger_InvalidIndex_ThrowsException()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar");

            // Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
            {
                _ = sut[1];
            });

            // Assert
            Assert.Contains("Index 1", exception.Message);
        }

        // ...

    }
}
