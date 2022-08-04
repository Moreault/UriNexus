namespace UriNexus.Tests;

[TestClass]
public class UrlParameterTester
{
    [TestClass]
    public class Name : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNullOrEmpty_Throw(string name)
        {
            //Arrange
            var value = Fixture.Create<object>();

            //Act
            var action = () => new UrlParameter(name, value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Name));
        }

        [TestMethod]
        public void WhenNotNullOrEmpty_SetToValue()
        {
            //Arrange
            var name = Fixture.Create<string>();
            var value = Fixture.Create<object>();

            //Act
            var result = new UrlParameter(name, value);

            //Assert
            result.Name.Should().Be(name);
        }
    }

    [TestClass]
    public class Value : Tester
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenNullOrEmpty_Throw(string value)
        {
            //Arrange
            var name = Fixture.Create<string>();

            //Act
            var action = () => new UrlParameter(name, value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
        }

        [TestMethod]
        public void WhenNotNullOrEmpty_SetToValue()
        {
            //Arrange
            var name = Fixture.Create<string>();
            var value = Fixture.Create<object>();

            //Act
            var result = new UrlParameter(name, value);

            //Assert
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void WhenValueIsNotString_SetToWhateverItIs()
        {
            //Arrange
            var name = Fixture.Create<string>();
            var value = Fixture.Create<int>();

            //Act
            var result = new UrlParameter(name, value);

            //Assert
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void WhenValueIsNullInt_Throw()
        {
            //Arrange
            var name = Fixture.Create<string>();
            int? value = null!;

            //Act
            var action = () => new UrlParameter(name, value);

            //Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
        }
    }

    [TestClass]
    public class ToStringMethod : Tester
    {
        [TestMethod]
        public void Always_ReturnNameAndValueSeparatedByAnEqualSign()
        {
            //Arrange
            var instance = Fixture.Create<UrlParameter>();

            //Act
            var result = instance.ToString();

            //Assert
            result.Should().Be($"{instance.Name}={instance.Value}");
        }
    }
}