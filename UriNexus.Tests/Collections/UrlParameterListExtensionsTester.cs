namespace UriNexus.Tests.Collections;

[TestClass]
public class UrlParameterListExtensionsTester
{
    [TestClass]
    public class ToQueryParameterList : Tester
    {
        [TestMethod]
        public void WhenIsNull_Throw()
        {
            //Arrange
            IEnumerable<UrlParameter> parameters = null!;

            //Act
            var action = () => parameters.ToUrlParameterList();

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
        }

        [TestMethod]
        public void WhenIsNotNull_ReturnAsUrlParameterList()
        {
            //Arrange
            var parameters = Fixture.CreateMany<UrlParameter>().ToArray();

            //Act
            var result = parameters.ToUrlParameterList();

            //Assert
            result.Should().BeEquivalentTo(parameters);
        }
    }
}