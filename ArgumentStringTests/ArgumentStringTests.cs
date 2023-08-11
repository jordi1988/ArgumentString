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
                _ = new ArgumentString(@"foo=bar", (options) => { });
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

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_ValidArgumentsWithMissingMandatoryKey_ThrowsException()
        {
            // Arrange, Act & Assert
            Assert.Throws<MissingArgumentsParseException>(() =>
            {
                _ = new ArgumentString(@"foo=bar", (options) =>
                {
                    options.MandatoryKeys = new List<string>() { "missing" };
                });
            });
        }

        // ...

    }
}
