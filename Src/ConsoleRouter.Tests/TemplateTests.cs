using ConsoleRouter.Routing;
using Xunit;

namespace ConsoleRouter.Tests
{
    public class TemplateTests
    {
        [Fact]
        public void Raw_Should_Contain_Original_Template()
        {
            // Arrange
            var originalTemplate = "{controller} {action}";

            // Act
            var result = Template.Parse(originalTemplate);

            // Assert
            Assert.Equal(originalTemplate, result.Raw);
        }
    }
}
