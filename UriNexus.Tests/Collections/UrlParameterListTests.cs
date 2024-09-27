namespace UriNexus.Tests.Collections;

[TestClass]
public class UrlParameterListTests : RecordTester<UrlParameterList>
{
    [TestMethod]
    public void Count_WhenIsEmpty_ReturnZero()
    {
        //Arrange
        var instance = new UrlParameterList();

        //Act
        var result = instance.Count;

        //Assert
        result.Should().Be(0);
    }

    [TestMethod]
    public void Count_WhenIsNotEmpty_ReturnNumberOfItems()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>(4).ToUrlParameterList();

        //Act
        var result = instance.Count;

        //Assert
        result.Should().Be(4);
    }

    [TestMethod]
    public void Indexer_WhenThereIsNoParameterWithName_ReturnEmpty()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        //Act
        var result = instance[Dummy.Create<string>()];

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Indexer_WhenThereIsOneParameterWithNameButDifferentCasing_ReturnEmpty()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        //Act
        var result = instance[instance.GetRandom().Name.ToUpperInvariant()];

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Indexer_WhenThereIsOneParameterWithSameNameAndCasing_ReturnItsValue()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var parameter = instance.GetRandom();

        //Act
        var result = instance[parameter.Name];

        //Assert
        result.Should().Be(parameter.Value.ToString());
    }

    [TestMethod]
    public void Constructor_WhenUsingDefault_InstantiateEmpty()
    {
        //Arrange

        //Act
        var result = new UrlParameterList();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Constructor_WhenUsingParams_InstantiateWithParameters()
    {
        //Arrange
        var parameter1 = Dummy.Create<UrlParameter>();
        var parameter2 = Dummy.Create<UrlParameter>();
        var parameter3 = Dummy.Create<UrlParameter>();

        //Act
        var result = new UrlParameterList(parameter1, parameter2, parameter3);

        //Assert
        result.Should().BeEquivalentTo(new List<UrlParameter> { parameter1, parameter2, parameter3 });
    }

    [TestMethod]
    public void Constructor_WhenUsingNullEnumerable_Throw()
    {
        //Arrange
        IEnumerable<UrlParameter> parameters = null!;

        //Act
        var action = () => new UrlParameterList(parameters);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
    }

    [TestMethod]
    public void Constructor_WhenUsingEnumerable_InstantiateWithContents()
    {
        //Arrange
        var parameters = Dummy.CreateMany<UrlParameter>().ToList();

        //Act
        var result = new UrlParameterList(parameters);

        //Assert
        result.Should().BeEquivalentTo(parameters);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void WithNameValue_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        var value = Dummy.Create<object>();

        //Act
        var action = () => instance.With(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Name));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void WithNameValue_WhenValueIsNullOrEmpty_Throw(string value)
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        var name = Dummy.Create<string>();

        //Act
        var action = () => instance.With(name, value);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(UrlParameter.Value));
    }

    [TestMethod]
    public void WithNameValue_WhenAddingNameThatAlreadyExists_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        var name = instance.GetRandom().Name;
        var value = Dummy.Create<object>();

        //Act
        var action = () => instance.With(name, value);

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, name));
    }

    [TestMethod]
    public void WithNameValue_WhenAddingNewName_ReturnNewCollectionWithNameAndValue()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var parameter = Dummy.Create<UrlParameter>();

        //Act
        var result = instance.With(parameter.Name, parameter.Value);

        //Assert
        result.Should().BeEquivalentTo(instance.Concat(new[] { parameter }));
    }

    [TestMethod]
    public void WithNameValue_WhenAddingNewName_DoNotModifyOriginalList()
    {
        //Arrange
        var originalInstance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var instance = originalInstance.ToUrlParameterList();
        var parameter = Dummy.Create<UrlParameter>();

        //Act
        instance.With(parameter.Name, parameter.Value);

        //Assert
        instance.Should().BeEquivalentTo(originalInstance);
    }

    [TestMethod]
    public void WithParams_WhenOneParameterIsNull_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var parameters = Dummy.CreateMany<UrlParameter>().Concat(new UrlParameter[] { null! }).ToArray();

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingNullParameters);
    }

    [TestMethod]
    public void WithParams_WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var existingItem = instance.GetRandom();
        var parameters = Dummy.CreateMany<UrlParameter>().Concat(new[] { existingItem }).ToArray();

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingItem.Name));
    }

    [TestMethod]
    public void WithParams_WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var existingItems = new List<UrlParameter> { instance.First(), instance.Last() };
        var parameters = Dummy.CreateMany<UrlParameter>().Concat(existingItems).ToArray();

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", existingItems.Select(x => x.Name))));
    }

    [TestMethod]
    public void WithParams_WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var parameters = Dummy.CreateMany<UrlParameter>().ToArray();

        //Act
        var result = instance.With(parameters);

        //Assert
        result.Should().BeEquivalentTo(instance.Concat(parameters));
    }

    [TestMethod]
    public void WithParams_WhenAddingNewParameters_DoNotModifyOriginalCollection()
    {
        //Arrange
        var originalInstance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var instance = originalInstance.ToUrlParameterList();
        var parameters = Dummy.CreateMany<UrlParameter>().ToArray();

        //Act
        instance.With(parameters);

        //Assert
        instance.Should().BeEquivalentTo(originalInstance);
    }

    [TestMethod]
    public void WithEnumerable_WhenParametersIsNull_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        IEnumerable<UrlParameter> parameters = null!;

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(parameters));
    }

    [TestMethod]
    public void WithEnumerable_WhenOneParameterIsNull_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var parameters = Dummy.CreateMany<UrlParameter>().Concat(new UrlParameter[] { null! }).ToList();

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage(Exceptions.AddingNullParameters);
    }

    [TestMethod]
    public void WithEnumerable_WhenOneParameterHasNameThatAlreadyExistsInCollection_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var existingItem = instance.GetRandom();
        var parameters = Dummy.CreateMany<UrlParameter>().Concat(new[] { existingItem }).ToList();

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingOneDuplicateParameter, existingItem.Name));
    }

    [TestMethod]
    public void WithEnumerable_WhenMultipleParametersHaveNamesThatAlreadyExistInCollection_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var existingItems = new List<UrlParameter> { instance.First(), instance.Last() };
        var parameters = Dummy.CreateMany<UrlParameter>().Concat(existingItems).ToList();

        //Act
        var action = () => instance.With(parameters);

        //Assert
        action.Should().Throw<Exception>().WithMessage(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", existingItems.Select(x => x.Name))));
    }

    [TestMethod]
    public void WithEnumerable_WhenAddingNewParameters_ReturnNewInstanceWithParametersAtEnd()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var parameters = Dummy.CreateMany<UrlParameter>().ToList();

        //Act
        var result = instance.With(parameters);

        //Assert
        result.Should().BeEquivalentTo(instance.Concat(parameters));
    }

    [TestMethod]
    public void WithEnumerable_WhenAddingNewParameters_DoNotModifyOriginalCollection()
    {
        //Arrange
        var originalInstance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var instance = originalInstance.ToUrlParameterList();
        var parameters = Dummy.CreateMany<UrlParameter>().ToList();

        //Act
        instance.With(parameters);

        //Assert
        instance.Should().BeEquivalentTo(originalInstance);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Without_WhenNameIsNullOrEmpty_Throw(string name)
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        //Act
        var action = () => instance.Without(name);

        //Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName(nameof(name));
    }

    [TestMethod]
    public void Without_WhenThereIsNoItemWithThatNameInCollection_Throw()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();

        var name = Dummy.Create<string>();

        //Act
        var action = () => instance.Without(name);

        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage(string.Format(Exceptions.RemovingNonExistentParameter, name));
    }

    [TestMethod]
    public void Without_WhenThereIsAnItemWithThatName_RemoveIt()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var item = instance.GetRandom();

        //Act
        var result = instance.Without(item.Name);

        //Assert
        result.Should().BeEquivalentTo(instance.Where(x => x != item));
    }

    [TestMethod]
    public void Without_WhenThereIsAnItemWithThatName_DoNotModifyOriginalInstance()
    {
        //Arrange
        var originalInstance = Dummy.CreateMany<UrlParameter>().ToUrlParameterList();
        var instance = originalInstance.ToUrlParameterList();
        var item = instance.GetRandom();

        //Act
        var result = instance.Without(item.Name);

        //Assert
        instance.Should().BeEquivalentTo(originalInstance);
    }

    [TestMethod]
    public void ToString_WhenIsEmpty_ReturnEmpty()
    {
        //Arrange
        var instance = new UrlParameterList();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void ToString_WhenHasOnlyOneItem_ReturnSingleItemSeparatedByEqualsSign()
    {
        //Arrange
        var instance = new UrlParameterList(Dummy.Create<UrlParameter>());

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{instance.Single().Name}={instance.Single().Value}");
    }

    [TestMethod]
    public void ToString_WhenHasMultipleItems_FormatAllItemsSeparatedByAmpersand()
    {
        //Arrange
        var instance = Dummy.CreateMany<UrlParameter>(3).ToUrlParameterList();
        var items = instance.ToList();

        //Act
        var result = instance.ToString();

        //Assert
        result.Should().Be($"{items[0].Name}={items[0].Value}&{items[1].Name}={items[1].Value}&{items[2].Name}={items[2].Value}");
    }

    [TestMethod]
    public void Ensure_IsJsonSerializable() => Ensure.IsJsonSerializable<UrlParameterList>(Dummy, JsonSerializerOptions);
}