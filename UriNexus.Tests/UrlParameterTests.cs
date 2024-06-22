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
        var value = Dummy.Create<object>();

        //Act
        var action = () => new UrlParameter(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Name));
    }

    [TestMethod]
    public void Name_WhenNotNullOrEmpty_SetToValue()
    {
        //Arrange
        var name = Dummy.Create<string>();
        var value = Dummy.Create<object>();

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
        var name = Dummy.Create<string>();

        //Act
        var action = () => new UrlParameter(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
    }

    [TestMethod]
    public void Value_WhenNotNullOrEmpty_SetToValue()
    {
        //Arrange
        var name = Dummy.Create<string>();
        var value = Dummy.Create<object>();

        //Act
        var result = new UrlParameter(name, value);

        //Assert
        result.Value.Should().Be(value.ToString());
    }

    [TestMethod]
    public void Value_WhenValueIsNotString_SetToWhateverItIs()
    {
        //Arrange
        var name = Dummy.Create<string>();
        var value = Dummy.Create<int>();

        //Act
        var result = new UrlParameter(name, value);

        //Assert
        result.Value.Should().BeEquivalentTo(value.ToString());
    }

    [TestMethod]
    public void Value_WhenValueIsNullInt_Throw()
    {
        //Arrange
        var name = Dummy.Create<string>();
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
        var instance = Dummy.Create<UrlParameter>();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Name}={instance.Value}");
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<UrlParameter>(Dummy);
}