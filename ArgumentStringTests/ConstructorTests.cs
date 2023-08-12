using ArgumentStringNS.Exceptions;
using Xunit;

namespace ArgumentStringNS.Tests
{
    public class ConstructorTests
    {
        [Fact]
        public void MissingArgumentString_ThrowsException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new ArgumentString(null!);
            });
        }

        [Fact]
        public void EmptyArgumentString_ThrowsException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new ArgumentString(string.Empty);
            });
        }

        [Theory]
        [InlineData(@"foo=bar;version")]
        [InlineData(@"foo=bar;version;")]
        [InlineData(@"foo=bar;version:;")]
        public void MalformedArgumentString_ThrowsException(string arguments)
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<MalformedArgumentException>(() =>
            {
                _ = new ArgumentString(arguments);
            });

            Assert.Contains("version", exception.Message);
            Assert.DoesNotContain("foo", exception.Message);
        }

        [Fact]
        public void ValidArgumentString_DoesNotThrow()
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
        public void ValidMandatoryArgument_DoesNotThrow()
        {
            // Arrange & Act
            var exception = Record.Exception(() =>
                new ArgumentString(@"foo=bar", new ParseOptions("foo"))
            );

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MultipleValidMandatoryArguments_DoesNotThrow()
        {
            // Arrange & Act
            var exception = Record.Exception(() =>
                new ArgumentString(@"foo=bar;version=1;tip=top", new ParseOptions("foo", "version", "tip"))
            );

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MissingMandatoryArgument_ThrowsException()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
                new ArgumentString(@"foo=bar", new ParseOptions("missing"))
            );

            Assert.Contains("mandatory", exception.Message);
            Assert.Contains("missing", exception.Message);
            Assert.DoesNotContain("foo", exception.Message);
        }

        [Fact]
        public void MissingMandatoryArgumentOfMultipleValid_ThrowsException()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
                new ArgumentString(@"foo=bar;version=1;tip=top", new ParseOptions("foo", "missing"))
            );

            Assert.Contains("mandatory", exception.Message);
            Assert.Contains("missing", exception.Message);
            Assert.DoesNotContain("foo", exception.Message);
        }

        [Fact]
        public void MultipleMissingMandatoryArgumentOfMultipleValid_ThrowsException()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<MissingArgumentException>(() =>
                new ArgumentString(@"foo=bar;version=1;tip=top", new ParseOptions("foo", "missing", "alsomissing"))
            );

            Assert.Contains("mandatory", exception.Message);
            Assert.Contains("missing, alsomissing", exception.Message);
            Assert.DoesNotContain("foo", exception.Message);
        }

        [Fact]
        public void IgnoreEmptyArguments_DoesNotThrow()
        {
            // Arrange
            var sut = new ArgumentString(@";;foo=bar;;;");

            // Act
            int argumentCount = sut.Count;

            // Assert
            Assert.Equal(1, argumentCount);
        }

        [Fact]
        public void AnotherArgumentSeparator_WillBeRecognized()
        {
            // Arrange
            var sut = new ArgumentString(@"foo=bar|version=1", 
                (options) => options.ArgumentSeparator = "|");

            // Act
            int argumentCount = sut.Count;

            // Assert
            Assert.Equal(2, argumentCount);
        }

        [Fact]
        public void AnotherKeyValueSeparator_WillBeRecognized()
        {
            // Arrange
            var sut = new ArgumentString(@"foo->bar;version->1",
                (options) => options.KeyValueSeparator = "->");

            // Act
            int argumentCount = sut.Count;

            // Assert
            Assert.Equal(2, argumentCount);
        }

        [Fact]
        public void AnotherArgumentAndKeyValueSeparator_WillBeRecognized()
        {
            // Arrange
            var sut = new ArgumentString(@"foo->bar|version->1",
                (options) => {
                    options.ArgumentSeparator = "|";
                    options.KeyValueSeparator = "->";
                });

            // Act
            int argumentCount = sut.Count;

            // Assert
            Assert.Equal(2, argumentCount);
        }
    }
}
