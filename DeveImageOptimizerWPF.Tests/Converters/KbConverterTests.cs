using DeveImageOptimizerWPF.Converters;
using System.Globalization;
using Xunit;

namespace DeveImageOptimizerWPF.Tests.Converters
{
    public class KbConverterTests
    {
        [Fact]
        public void KbToString()
        {
            //Arrange
            var kbConverter = new KbConverter();

            //Act
            var result = kbConverter.Convert(123456789L, null, null, CultureInfo.InvariantCulture);

            //Assert
            Assert.Equal("117,7MB", result.ToString());
        }
    }
}
