namespace UriNexus.Tests;

[TestClass]
public class UrlParameterTests : RecordTester<UrlParameter>
{
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Name_WhenNullOrEmpty_Throw(string name)
    {
        //Arrange
        var value = Fixture.Create<object>();

        //Act
        var action = () => new UrlParameter(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Name));
    }

    [TestMethod]
    public void Name_WhenNotNullOrEmpty_SetToValue()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var value = Fixture.Create<object>();

        //Act
        var result = new UrlParameter(name, value);

        //Assert
        result.Name.Should().Be(name);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Value_WhenNullOrEmpty_Throw(string value)
    {
        //Arrange
        var name = Fixture.Create<string>();

        //Act
        var action = () => new UrlParameter(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
    }

    [TestMethod]
    public void Value_WhenNotNullOrEmpty_SetToValue()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var value = Fixture.Create<object>();

        //Act
        var result = new UrlParameter(name, value);

        //Assert
        result.Value.Should().Be(value.ToString());
    }

    [TestMethod]
    public void Value_WhenValueIsNotString_SetToWhateverItIs()
    {
        //Arrange
        var name = Fixture.Create<string>();
        var value = Fixture.Create<int>();

        //Act
        var result = new UrlParameter(name, value);

        //Assert
        result.Value.Should().BeEquivalentTo(value.ToString());
    }

    [TestMethod]
    public void Value_WhenValueIsNullInt_Throw()
    {
        //Arrange
        var name = Fixture.Create<string>();
        int? value = null!;

        //Act
        var action = () => new UrlParameter(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
    }

    [TestMethod]
    public void ToString_Always_ReturnNameAndValueSeparatedByAnEqualSign()
    {
        //Arrange
        var instance = Fixture.Create<UrlParameter>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Name}={instance.Value}");
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<UrlParameter>(Fixture);
}